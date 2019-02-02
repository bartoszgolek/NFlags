# NFlags

[![Build Status](https://travis-ci.org/bartoszgolek/NFlags.svg?branch=master)](https://travis-ci.org/bartoszgolek/NFlags)
[![NuGet](https://img.shields.io/nuget/dt/NFlags.svg)](https://www.nuget.org/packages/NFlags)
[![License](http://img.shields.io/badge/license-mit-blue.svg?style=flat-square)](https://raw.githubusercontent.com/labstack/echo/master/LICENSE)

Simple library to made parsing CLI arguments easy.
Library allows to print usage help "out of box".

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

        Flags:
        --flag1, -f     Flag description
        --help, -h      Prints this help

        Options:
        --option <option>, -o <option>  Option description

        Parameters:
        <param> Param description
        <paramSeries...>        paramSeriesDescription


$> dotnet NFlags.QuickStart.dll subcommand
This is subcommand: SubParameterValue
$> dotnet NFlags.QuickStart.dll subcommand yyy
This is subcommand: yyy
$> dotnet NFlags.QuickStart.dll subcommand --help
Usage:
        QuickStart [FLAGS]... [PARAMETERS]...

This is NFlags

        Flags:
        --help, -h      Prints this help

        Parameters:
        <SubParameter>  SubParameter description

$> 

```
## Basics

### Global NFlags Configuration

#### Set app name
Name set with following code, will be printed in help.
By default `AppDomain.CurrentDomain.FriendlyName` is used.
```c#
NFlags.Configure(configurator => configurator.SetName("Custom Name"));
```

#### Set app description
Description set with following code, will be printed in help.
```c#
NFlags.Configure(configurator => configurator.SetDescription("App description"));
```

#### Set output
Output adapter set by this method is used to produce output. Default `Console` is used.
```c#
NFlags.Configure(configurator => configurator.SetOutput(Output.Console));
```

#### Set environment
Environmant adapter set by this method is used to read environment variables. Default `System` is used.
Additional Prefixed adapter is provided within library and can be used when all variabels should by started with same prefix to not define prefixed name in every usage.
```c#
NFlags.Configure(configurator => configurator.SetEnvironment(Environment.System);
```

#### Set dialect
Dialect defines how flags and options are prefixed and how option value follows option.
```c#
NFlags.Configure(configurator => configurator.SetDialect(Dialect.Gnu));
```

### Command configuration

NFlags always has at least one root command

#### Set flag
Flag is an prefixed argument without value. It can be turned on or off.
Flag abbreviation can be also set.
There is also default value which will be negated when flag will be passed as argument.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterFlag("flag", "f", "Flag description", true));
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterFlag("flag", "Flag description", false));
```

Alternative way of setting flag is to use flagBuilder.
```c#
NFlags
    .Configure(c => {})
    .Root(configurator => configurator
        .RegisterFlag(flagBuilder => flagBuilder
            .Name("flag")
            .Abr("f")
            .Description("Flag description")
            .EnvironmentVariable("NFLAGS_FLAG")
            .Persistent()
            .DefaultValue(true)
        )
    );
```

### Set option
Option is an prefixed argument with value.
Option abbreviation can be also set.
Values are converted to type T. CLR types, classes with implicit operator from string and classes with string argument constructor are supported by default. For other types see Converters section.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterOption("option", "o", "option description", "defaultOptionValue"));
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterOption("option", "option description", "defaultOptionValue"));
```

Alternative way of setting option is to use optionBuilder.
```c#
NFlags
    .Configure(c => {})
    .Root(configurator => configurator
        .RegisterOption<double>(optionBuilder => optionBuilder
            .Name("option")
            .Abr("o")
            .Description("option description")
            .EnvironmentVariable("NFLAGS_OPTION")
            .Persistent()
            .DefaultValue(1.1)
        )
    );
```

### Set parameter
Parameter is an unprefixed value argument. Parameters are read by registration order.
Values are converted to type T. CLR types, classes with implicit operator from string and classes with string argument constructor are supported by default. For other types see Converters section.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterParameter("param", "Param description", "paramDefaultValue"));
```

Alternative way of setting parameter is to use parameterBuilder.
```c#
NFlags
    .Configure(c => {})
    .Root(configurator => configurator
        .RegisterParameter<double>(parameterBuilder => parameterBuilder
            .Name("parameter")
            .Description("parameter description")
            .EnvironmentVariable("NFLAGS_PARAMETER")
            .DefaultValue(1.2)
        )
    );
```

### Set parameter series
Parameter series is a collection of parameters after last named parameter.
Parameter series can be used to parse unknown count of parameters to process i.e. strings to concat.
Values are converted to type T. CLR types, classes with implicit operator from string and classes with string argument constructor are supported by default. For other types see Converters section.

```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterParamSeries<int>("paramSeries", "Param series description"));
```
There is also non-generic method where argument type is string
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterParamSeries("paramSeries", "Param series description"));
``` 

### Attach code to execution

To attach code to execution by command, simply call `SetExecution` method of command configurator and pass `Action<CommandArgs, Action<string>>` callback.
First argument of action contains all registered Flags, Options and Parameters with default or given values. The second one is callback to print output from command.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.SetExecute((commandArgs, output) => output.WriteLine("This is command output: " + commandArgs.GetParameter<string>("param")));
```
If execute is of type `Func<CommandArgs, IOutput, int>` result will be returned by `Bootstrap.Run` to be used as exit code.

### Attach sub commands

To attach subcommand, call `RegisterCommand` method of command configurator.
The third parameter is a configurator for the sub command and can be used in the same Way as the one for root command..
```c#
NFlags.Configure(c => {}).
    Root(configurator => configurator.
        RegisterCommand("subcommand", "Subcommand Description", sc => sc.
                SetExecute((commandArgs, output) => output.WriteLine("This is subcommand: " + commandArgs.GetParameter<string>("SubParameter"))).
                RegisterParam("SubParameter", "SubParameter description", "SubParameterValue")
        )
    );
```


### Define default command
Default command can be defined, when registering command with `RegisterDefaultCommand`. If default command is registered, it will be executed, when application is called to execute parent command. 
```c#
NFlags.Configure(c => {}).
    Root(configurator => configurator.
        RegisterDefaultCommand("default", "Default command Description", sc => sc.
                SetExecute((commandArgs, output) => output.WriteLine("This is default command"))
        )
    );
```

### Parsing arguments
To parse arguments and execute requested command call:
```c#
NFlags.Configure(c => {}).Root(configurator => {}).Run(args);
```


## Dialects
Dialect defines how flags and options are prefixed and how option value follows option.
By default 2 dialect are supported: **Gnu**, **Win**

Dialect can be set (default is Win) using:
```c#
NFlags.Configure(configurator => configurator.SetDialect(Dialect.Gnu));
```

Dialects can be easily extended. 
To create new dialect simply create new class inherited from Dialect class.

```c#
    public class CustomDialect : Dialect
    {      
        public override string Prefix => "x";

        public override string AbrPrefix => "a";

        public override OptionSeparator OptionSeparator => OptionSeparator.ArgSeparator;
    }
```
and configure NFlags using it
```c#
NFlags.Configure(configurator => configurator.SetDialect(new CustomDialect()));
```

### Win dialect

Win dialect, follows MS Windows standards for defining console app arguments:

- Flags and flags abbreviations are prefixed by '/' 
- Options and options abbreviations are prefixed by '/' 
- Value follow option after '='


### Gnu dialect

Gnu dialect, follows Gnu Linux standards for defining console app arguments:

- Flags are prefixed by '--' 
- Flags abbreviations are prefixed by '-' 
- Options are prefixed by '--' 
- Options abbreviations are prefixed by '-' 
- Value follow option as next argument after option

## Help

Help generation is supported "out of box" and it follows dialect rules.
To print help use:
```c#
Console.WriteLine(NFlags.Configure(configurator => {}).PrintHelp());
```

Example help for Win dialect:

```
Usage:
        NFlags.Win [COMMAND] [FLAGS]... [OPTIONS]... [PARAMETERS]...

Application description

        Commands:
        subcommand      Subcommand Description

        Flags:
        /help, /h       Print this help
        /verbose, /v    Verbose description
        /clear  Clear description

        Options:
        /option1=<option1>, /o1=<option1>       Option 1 description
        /option2=<option2>      Option 2 description

        Parameters:
        <param1>        Parameter 1 description

```

and for Gnu dialect:
```
Usage:
        NFlags.Gnu [COMMAND] [FLAGS]... [OPTIONS]... [PARAMETERS]...

Application description

        Commands:
        subcommand      Subcommand Description

        Flags:
        --help, -h      Print this help
        --verbose, -v   Verbose description
        --clear Clear description

        Options:
        --option1 <option1>, -o1 <option1>      Option 1 description
        --option2 <option2>     Option 2 description

        Parameters:
        <param1>        Parameter 1 description

```

Command can be also configured to print help on execute. This is useful when creating command who aggregates set of sub commands.
The following code:
```c#
NFlags
    .Configure(c => c.SetDialect(Dialect.Gnu))
    .Root<RootCommandArguments>(c => c.PrintHelpOnExecute())
    .Run(args);
```
will print following output:
```
$> dotnet NFlags.Generics.dll
Usage:
        NFlags.Generics [FLAGS]... [OPTIONS]... [PARAMETERS]...

        Flags:
        --flag, -f      flag description
        --help, -h      Prints this help

        Options:
        --option <option>, -o <option>  option description

        Parameters:
        <parameter>     parameter description
        <parameterSeries...>    parameter series description
```

## Generics
Generics are an alternative method of registering commands arguments. Generic cannot be mixed with basic methods to configure command.
To use generic way of registering commands with arguments custom type with fields or set properties with dedicated attributes is required.
```c#
    public class RootCommandArguments
    {
        [Option("option", "o", "option description", 3)]
        public int Option;

        [Flag("flag", "f", "flag description", true)]
        public bool Flag;

        [Parameter("parameter", "parameter description", 1.1)]
        public double Parameter;

        [ParameterSeries("parameterSeries", "parameter series description")]
        public int[] ParameterSeries;
    }
    
    public class Command2Arguments
    {
        [Option("option2", "o2", "option description", 3)]
        public int Option { get; set; }

        [Flag("flag2", "f2", "flag description", true)]
        public bool Flag { get; set; }

        [Parameter("parameter2", "parameter description", 1.1)]
        public double Parameter { get; set; }

        [ParameterSeries("parameterSeries2", "parameter series description")]
        public int[] ParameterSeries { get; set; }
    }
```
Then command can be configured using generic versions of methods:
```c#
NFlags
    .Configure(c => c
        .SetDialect(Dialect.Gnu)
    )
    .Root<RootCommandArguments>(c => c
        .PrintHelpOnExecute()
        .RegisterCommand<RootCommandArguments>("command1", "this is command 1", configurator => configurator
            .PrintHelpOnExecute()
        )
    )
```

See `NFlags.Generics` example. 

## Converters

Converters are used to convert argument (option or parameter) value to expected type. 
Converter must implement interface `NFlags.TypeConverters.IArgumentConverter` interface.
```c#
    public class UserConverter : IArgumentConverter
    {
        public bool CanConvert(Type type)
        {
            return typeof(User) == type;
        }

        public object Convert(Type type, string value)
        {
            var strings = value.Split(";");
            if (strings.Length != 3)
                throw new ArgumentValueException(type, value);

            return new User
            {
                UserName = strings[0],
                Name = strings[1],
                Password = strings[2]
            };
        }
    }
```
When registering parameter/option, existence of converter is checked, otherwise `NFlags.TypeConverters.MissingConverterException` is thrown.
When parsing arguments, first matching converter is used (in registration order).
```c#
NFlags.Configure(configurator => configurator.RegisterConverter(new UserConverter()));
```

## Exception handling

Default policy of exception handling in NFlags is to not throw exception when user use CLI incorrectly.
If Exception is thrown during parsing arguments, exception message and help is printed and exit code 255 is returned.
For the following application:
```c#
public static int Main(string[] args)
{
    return NFlags
        .Configure(c => { })
        .Root(c => c
            .RegisterParam("param", "param description", 1)
            .SetExecute((commandArgs, output) => { }))
        .Run(args);
}
```
Execution of:
```
$> dotnet Program.dll asd
```
Will print:
```
Cannot convert value 'as' to type 'System.Int32'

Usage:
        NFlags.Empty [FLAGS]... [PARAMETERS]...

        Flags:
        /help, /h       Prints this help

        Parameters:
        <param> param description


Process finished with exit code 255.
```

Exception handling can be disabled using configuration:
```c#
NFlags.Configure(c => c.DisableExceptionHandling());
```
If exception handling is disabled NFlags can throw `NFlags.TooManyParametersException` or `NFlags.TypeConverters.ArgumentValueException`.
