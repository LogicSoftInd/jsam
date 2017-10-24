# jsam
> The fast json sampler

## Overview

We live in an age where APIs are all around us. Integration with an API means
being able to send requests and parse responses. In order to do this, in the
statically typed world, we need to create structures that represent each
request/response pair that we're handling.

Hand coding the classes becomes _very_ tedious. Enough thanks can't be
said to Jon Keith for making the great [json2csharp](http://json2csharp.com/)
tool, that when given a json, will spit out a set of classes that represent the
structure of that json -- Simple and beautiful.

(Aside: It looks as if json2csharp is joining forces with
[Quicktype](https://quicktype.io/) and a quick look at Quicktype says that they
support a much more wider variety of languages; kudos, Jon!)

So given a json, getting a set of class scaffolding in C#/Java/et al. isn't
difficult. So what's the real problem?

The real problem comes when the json is **huge**. When it is huge, we run into the
natural browser limits of pasting text more than a few MB and we are forcing
quicktype / json2csharp to process that info for us.

**Enter jsam.**

jsam is a simple and fast json sampler. The aim of jsam is to give you **just
enough** a json that can be thrown to quicktype or json2csharp to generate the
set of classes required to parse it.

## Features

- Cross platform support OSX/Linux/Windows
- Simple installation (unzip!)
- Zero dependencies
- Simple CLI

## Usage

Run jsam on a json file

```
$ jsam /path/to/file.json
```

Indent the output json

```
$ jsam --indent-output /path/to/file.json
```

Time the whole process

```
$ jsam --time /path/to/file.json
```

## Working

The way jsam works is by looking into the key value pairs and:

- If a value is an array, it picks only the first one out of this
- If a value is a dictionary, it recurses and repeats the entire process
- If a value is a basic value, it just adds it back

## TODO

- [ ] Support a `--depth <n>` flag that, if an array is found would pick out
  `n` elements instead of just one
- [ ] Support a `--url <url>` flag that would fetch the json from a given URL
  instead of a file location
