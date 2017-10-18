using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace jsam
{
    class Options
    {
        /*
        [Option('d', "depth", Required = false,
            HelpText = "If arrays are discovered, how many elements to take?")]
        public int? Depth { get; set; }
        */

        [Option('u', "url", Required = false,
            HelpText = "Treat the argument as a URL")]
        public bool URL { get; set; }

        [Option("indent-output", Required = false,
            HelpText = "Indent the output?")]
        public bool IndentOutput { get; set; }

        [Option("time", Required = false,
            HelpText = "Show the time taken prompt at the end")]
        public bool Time { get; set; }

        [Value(0, MetaName = "Input file", Required = true,
            HelpText = "Input json file to sample")]
        public string Path { get; set; }
    }

    class Program
    {
        static int Main(string[] args)
        {
            var exitCode = Parser
                            .Default
                            .ParseArguments<Options>(args)
                            .MapResult(
                                options => RunAndExit(options),
                                _ => (int)ExitCodes.ImproperArguments
                            );

            //Console.ReadKey();

            return exitCode;
        }

        private static int RunAndExit(Options options)
        {
            try
            {
                var path = options.Path;
                var indentOutput = options.IndentOutput;
                var time = options.Time;
                var url = options.URL;

                /*
                var depth = options.Depth;
                var hasDepth = depth.HasValue;
                */

                var content = "";

                if (!url)
                {
                    if (!File.Exists(path))
                    {
                        Console.Error.WriteLine($"No file found at '{path}'");
                        return 1;
                    }

                    content = File.ReadAllText(path);
                }
                else
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = httpClient.GetAsync(path).Result;
                        content = response.Content.ReadAsStringAsync().Result;
                    }
                }

                var input = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

                var start = DateTime.Now;
                var sampledOutput = Sample(input);
                var end = DateTime.Now;

                var outputFormatting = Formatting.None;
                if (indentOutput)
                {
                    outputFormatting = Formatting.Indented;
                }

                Console.WriteLine(JsonConvert.SerializeObject(sampledOutput, outputFormatting));

                if (time)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Took { (end - start).TotalSeconds }s");
                }

                return (int)ExitCodes.Success;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return (int)ExitCodes.InternalError;
            }
        }

        static Dictionary<string, object> Sample(Dictionary<string, object> input)
        {
            var result = new Dictionary<string, object>();

            foreach (var kv in input)
            {
                var processedDict = new Dictionary<string, object>();

                var key = kv.Key;
                var value = kv.Value;

                var element = value;

                var isArray = false;
                var isDictionary = false;

                if (value.GetType() == typeof(JArray))
                {
                    element = ((JArray)value).First;
                    isArray = true;
                }

                if (element != null && element.GetType() == typeof(JObject))
                {
                    var elementAsDict = ((JObject)element).ToObject<Dictionary<string, object>>();
                    element = Sample(elementAsDict);
                    isDictionary = true;
                }

                if (isArray)
                {
                    if (isDictionary)
                    {
                        result.Add(key, new JArray(JObject.FromObject(element)));
                    }
                    else if (element != null)
                    {
                        result.Add(key, new JArray(element));
                    }
                    else
                    {
                        result.Add(key, new JArray());
                    }
                }
                else
                {
                    result.Add(key, element);
                }
            }

            return result;
        }
    }

    enum ExitCodes
    {
        Success = 0,
        ImproperArguments,
        InternalError,
    }
}

