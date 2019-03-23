# NFlags

[![Build Status](https://travis-ci.org/bartoszgolek/NFlags.svg?branch=master)](https://travis-ci.org/bartoszgolek/NFlags)
[![NuGet](https://img.shields.io/nuget/dt/NFlags.svg)](https://www.nuget.org/packages/NFlags)
[![License](http://img.shields.io/badge/license-mit-blue.svg?style=flat-square)](https://raw.githubusercontent.com/labstack/echo/master/LICENSE)

Simple yet powerfull library to made parsing CLI arguments easy. 
Library also allow to print usage help "out of box".

For example of usage check **Examples** directory.

## QuickStart

1. Install NFLags from NuGet.
1. Start new console project.
1. Configure NFLags:
```c#
NFlags.Configure(configure => configure
    .SetDialect(Dialect.Gnu)
    .SetName("QuickStart")
    .SetDescription("This is NFlags")
)
.Root(rc => rc
    .RegisterFlag("flag1", "f", "Flag description", false)
    .RegisterOption("option", "o", "Option description", "optionDefaultValue")
    .RegisterParameter("param", "Param description", "ParamDefaultValue")
    .RegisterCommand("subcommand", "Subcommand Description", sc => sc
            .SetExecute((commandArgs, output) => output.WriteLine("This is subcommand: " + commandArgs.GetParameter<string>("SubParameter")))
            .RegisterParameter("SubParameter", "SubParameter description", "SubParameterValue")
    )
    .RegisterParamSeries("paramSeries", "paramSeriesDescription")
    .SetExecute((commandArgs, output) => output.WriteLine("This is root command: " + commandArgs.GetParameter<string>("param")))
).
Run(args);
```
Run application and enjoy:
```
$> dotnet NFlags.QuickStart.dll
This is root command: ParamDefaultValue%
$> dotnet NFlags.QuickStart.dll xxx
This is root command: xxx
$> dotnet NFlags.QuickStart.dll --help
Usage:
        QuickStart [COMMAND] [FLAGS]... [OPTIONS]... [PARAMETERS]...

This is NFlags

        Commands:
        subcommand      Subcommand Description

        Parameters:
        <param> Param description
        <paramSeries...>        paramSeriesDescription

        Options:
        --flag1, -f     Flag description
        --option <option>, -o <option>  Option description
        --help, -h      Prints this help


$> dotnet NFlags.QuickStart.dll subcommand
This is subcommand: SubParameterValue
$> dotnet NFlags.QuickStart.dll subcommand yyy
This is subcommand: yyy
$> dotnet NFlags.QuickStart.dll subcommand --help
Usage:
        QuickStart [FLAGS]... [PARAMETERS]...

This is NFlags

        Parameters:
        <SubParameter>  SubParameter description

        Options:
        --help, -h      Prints this help

$> 

```

## Documentation

[See details on NFlags GitHub pages](https://bartoszgolek.github.io/NFlags/)
