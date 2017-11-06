# NFlags

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
    RegisterParam("param", "Param description", "ParamDefaultValue").
    RegisterSubcommand("subcommand", "Subcommand Description", sc => sc.
            SetExecute((commandArgs, output) => output("This is subcommand: " + commandArgs.Parameters["SubParameter"])).
            RegisterParam("SubParameter", "SubParameter description", "SubParameterValue")
    ).
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
Flag abreviation can be also set.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterOption("option", "o", "option description", "defaultOptionValue"));
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterOption("option", "option description", "defaultOptionValue"));
```

### Set parameter
Parameter is an unprefixed value argument. Parameters are read by registration order.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.RegisterParam("param", "Param description", "paramDefaultValue"));
```

### Attach code to execution

To attach code to execution by command, simply call `SetExecution` method of command configurator and pass `Action<CommandArgs, Action<string>>` callback.
First argument of action contains all registered Flags, Options and Parameters with default or given values. The second one is callback to print output from command.
```c#
NFlags.Configure(c => {}).Root(configurator => configurator.SetExecute((commandArgs, output) => output("This is command output: " + commandArgs.Parameters["param"]));
```

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