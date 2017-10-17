# jsam
> The fast json sampler

## Motivation

We live in an age where APIs are all around us. Integration with an API means
being able to send requests and parse responses. In order to do this, in the
statically typed world, we need to create structures that represent each
request/response pair that we're handling.

Hand coding the classes becomes tedious very fast and that's when we ended up
finding the great [json2csharp](http://json2csharp.com/) tool, that when given
a json, will spit out a set of classes that represent the structure of that
json -- Simple enough.

(Aside: It looks as if json2csharp is joining forces with
[Quicktype](https://quicktype.io/) and a quick look at Quicktype says that they
support a much more wider variety of languages; kudos!)

So given a json, getting a scaffolding containing C#/Java classes isn't
difficult. So what's the real problem?

The real problem comes when the json is **huge**. When it is huge, we run into the
natural browser limits of pasting text more than a few MB and we are forcing
quicktype / json2csharp to process that info for us.

Enter jsam.

jsam is a simple and fast json sampler. The aim of jsam is to give you **just
enough** a json that can be thrown to quicktype or json2csharp to generate the
set of classes required to parse it.
