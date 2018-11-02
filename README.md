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
).
Root(rc => rc.
    RegisterFlag("flag1", "f", "Flag description", false).
    RegisterOption("option", "o", "Option description", "optionDefaultValue").
    RegisterParam<string>("param", "Param description", "ParamDefaultValue").
    RegisterSubcommand("subcommand", "Subcommand Description", sc => sc.
            SetExecute((commandArgs, output) => output("This is subcommand: " + commandArgs.Parameters["SubParameter"])).
            RegisterParam<string>("SubParameter", "SubParameter description", "SubParameterValue")
    ).
    RegisterParamSeries("paramSeries", "paramSeriesDescription").
    SetExecute((commandArgs, output) => output("This is root command: " + commandArgs.Parameters["param"]))
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
Name set with folowing code, will be printed in help.
By default `AppDomain.CurrentDomain.FriendlyName` is used.
```c#
NFlags.Configure(configurator => configurator.SetName("Custom Name"));
```

#### Set app description
Description set with folowing code, will be printed in help.
```c#
NFlags.Configure(configurator => configurator.SetDescription("App description"));
```

#### Set output
Output action set by this method is used to produce output. Default `Console.Write` is used.
```c#
NFlags.Configure(configurator => configurator.SetOutpu(Console.WriteLine));
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
Flag abreviation can be also set.
There is also default value which will be inversed when flag will be passed as argument.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterFlag("flag", "f", "Flag description", true));
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterFlag("flag", "Flag description", false));
```

### Set option
Option is an prefixed argument with value.
Option abreviation can be also set.
Values are converted to type T. CLR types, classess with implicit operator from string and classes with string argument constructor are supported by default. For other types see Converters section.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterOption<T>("option", "o", "option description", "defaultOptionValue"));
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterOption<T>("option", "option description", "defaultOptionValue"));
```

### Set parameter
Parameter is an unprefixed value argument. Parameters are read by registration order.
Values are converted to type T. CLR types, classess with implicit operator from string and classes with string argument constructor are supported by default. For other types see Converters section.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterParam<T>("param", "Param description", "paramDefaultValue"));
```

### Set parameter series
Parameter series is a collection of parameters after last named parameter.
Parameter series can be used to parse unknown count of parameters to process i.e. strings to concat.
Values are converted to type T. CLR types, classess with implicit operator from string and classes with string argument constructor are supported by default. For other types see Converters section.

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
NFlags.Configure(c => {}).Root(configurator => configurator.SetExecute((commandArgs, output) => output("This is command output: " + commandArgs.Parameters["param"]));
```
If execute is of type `Func<CommandArgs, IOutput, int>` result will be returned by `Bootstrap.Run` to be used as exit code.

### Attach subcommands

To attach subcommand, call `RegisterSubcommand` method of command configurator.
The third parameter is a configurator for the subcommand and can be used in the same Way as the one for root command..
```c#
NFlags.Configure(c => {}).
    Root(configurator => configurator.
        RegisterSubcommand("subcommand", "Subcommand Description", sc => sc.
                SetExecute((commandArgs, output) => output("This is subcommand: " + commandArgs.Parameters["SubParameter"])).
                RegisterParam("SubParameter", "SubParameter description", "SubParameterValue")
        )
    );
```

### Parsing arguments
To parse arguments and call requested command call:
```c#
NFlags.Configure(c => {}).Root(configurator => {}).Run(args);
```


## Dialects
Dialect defines how flags and options are prefixed and how option value follows option.
By default 2 dialect are suported: **Gnu**, **Win**

Dialect can be set (default is Win) using:
```c#
NFlags.Configure(configurator => configurator.SetDialect(Dialect.Gnu));
```

Dialects can be easly extended. 
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

Win dialect, follows MS Windows standards for definig console app arguments:

- Flags and flags abreviations are prefixed by '/' 
- Options and options abreviations are prefixed by '/' 
- Value follow option after '='


### Gnu dialect

Gnu dialect, follows Gnu Linux standards for definig console app arguments:

- Flags are prefixed by '--' 
- Flags abreviations are prefixed by '-' 
- Options are prefixed by '--' 
- Options abreviations are prefixed by '-' 
- Value follow option as next argument after option

## Help

Help generation is suported "out of box" and it follows dialect rules.
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
For the following aplication:
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

Excpetion handling can be disabled using configuration:
```c#
NFlags.Configure(c => c.DisableExceptionHandling());
```
If exception handling is disabled NFlags can throw `NFlags.TooManyParametersException` or `NFlags.TypeConverters.ArgumentValueException`.
