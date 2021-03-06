# NFlags

Simple yet powerfull library to made parsing CLI arguments easy.
Library also allow to print usage help and application version "out of box".

For example of usage check **Examples** directory.

## QuickStart

1. Install NFLags from NuGet.
1. Start new console project.
1. Configure NFLags:
```c#
Cli.Configure(configure => configure
    .SetDialect(Dialect.Gnu)
    .SetName("QuickStart")
    .SetDescription("This is NFlags")
    .ConfigureVersion(vc => vc.Enable())
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
        QuickStart [COMMAND] [OPTIONS]... [PARAMETERS]...

This is NFlags

        Commands:
        command Sub command Description

        Parameters:
        <param> Param description (Default: 'ParamDefaultValue')
        <paramSeries...>        paramSeriesDescription

        Options:
        --flag1, -f     Flag description
        --option <option>, -o <option>  Option description (Default: 'optionDefaultValue')
        --help, -h      Prints this help
        --version, -v      Prints application version



$> dotnet NFlags.QuickStart.dll subcommand
This is subcommand: SubParameterValue
$> dotnet NFlags.QuickStart.dll subcommand yyy
This is subcommand: yyy
$> dotnet NFlags.QuickStart.dll command --help
Usage:
        QuickStart command [OPTIONS]... [PARAMETERS]...

This is NFlags

        Parameters:
        <Parameter>     Sub parameter description (Default: 'SubParameterValue')

        Options:
        --help, -h      Prints this help
        --version, -v      Prints application version

$>

```
## Basics

### Global NFlags Configuration

#### Set app name
Name set with following code, will be printed in help.
By default `AppDomain.CurrentDomain.FriendlyName` is used.
```c#
Cli.Configure(configurator => configurator.SetName("Custom Name"));
```

#### Set app description
Description set with following code, will be printed in help.
```c#
Cli.Configure(configurator => configurator.SetDescription("App description"));
```

#### Set output
Output adapter set by this method is used to produce output. Default `Console` is used.
```c#
Cli.Configure(configurator => configurator.SetOutput(Output.Console));
```

#### Set environment
Environment adapter set by this method is used to read environment variables. Default `System` is used.
Additional Prefixed adapter is provided within library and can be used when all variabels should by started with same prefix to not define prefixed name in every usage.
```c#
Cli.Configure(configurator => configurator.SetEnvironment(Environment.System);
```

#### Set config
Configuration adapter set by this method is used to read values from configuration. If not set, `ConfigPath` is not used when reading walue of argument.
```c#
Cli.Configure(configurator => configurator.SetConfiguration(...);
```

There are two types of configuration providers `IConfig` and `IGenericConfig`.
`IGenericConfig` require to return value in expected type (for argument using it), where  When using `IConfig` value returned by `Get` method is parsed using Converters. See [Converters](#converters) section.

When both `IConfig` and `IGenericConfig` are provided, generic one takes precedence.

#### Set dialect
Dialect defines how flags and options are prefixed and how option value follows option.
```c#
Cli.Configure(configurator => configurator.SetDialect(Dialect.Gnu));
```

### Command configuration

NFlags always has at least one root command

#### Set flag
Flag is an prefixed argument with boolean value. It can be turned on or off.
Flag abbreviation can be also set.
There is also default value which will be negated when flag will be passed as argument.
```c#
Cli.Configure(c => {}).Root(configurator => configurator.RegisterFlag("flag", "f", "Flag description", true));
Cli.Configure(c => {}).Root(configurator => configurator.RegisterFlag("flag", "Flag description", false));
```

Alternative way of setting flag is to use flagBuilder.
```c#
Cli
    .Configure(c => {})
    .Root(configurator => configurator
        .RegisterFlag(flagBuilder => flagBuilder
            .Name("flag")
            .Abr("f")
            .Description("Flag description")
            .EnvironmentVariable("NFLAGS_FLAG") //.LazyEnvironmentVariable("NFLAGS_FLAG")
            .ConfigPath("app.settings.flag") //.LazyConfigPath("app.settings.flag")
            .Persistent()
            .DefaultValue(true)
        )
    );
```

When registering flag, builder contains either `EnvironmentVariable` and `LazyEnvironmentVariable` methods. If `LazyEnvironmentVariable` is used, the variable will be resolved using provider when accessing command arg value, otherwise during initialisation.
Same goes to setting config path, builder contains either `EnvironmentVariable` and `LazyEnvironmentVariable` methods. If `LazyEnvironmentVariable` is used, the variable will be resolved using provider when accessing command arg value, otherwise during initialisation.

If flag default value is set to `True`, falg will be set to `False` when passed from CLI.

### Set option
Option is an prefixed argument with value.
Option abbreviation can be also set.
Values are converted to type T. CLR types, classes with implicit operator from string and classes with string argument constructor are supported by default. For other types see [converters](#converters) section.
```c#
Cli.Configure(c => {}).Root(configurator => configurator.RegisterOption("option", "o", "option description", "defaultOptionValue"));
Cli.Configure(c => {}).Root(configurator => configurator.RegisterOption("option", "option description", "defaultOptionValue"));
```

Alternative way of setting option is to use optionBuilder.
```c#
Cli
    .Configure(c => {})
    .Root(configurator => configurator
        .RegisterOption<double>(optionBuilder => optionBuilder
            .Name("option")
            .Abr("o")
            .Description("option description")
            .EnvironmentVariable("NFLAGS_OPTION") // .LazyEnvironmentVariable("NFLAGS_OPTION")
            .ConfigPath("app.settings.option") // .LazyConfigPath("app.settings.flag")
            .Persistent()
            .DefaultValue(1.1)
            .Converter(new ValueConverter())
        )
    );
```

When registering option, builder contains either `EnvironmentVariable` and `LazyEnvironmentVariable` methods. If `LazyEnvironmentVariable` is used, the variable will be resolved using provider when accessing command arg value, otherwise during initialisation.
Same goes to setting config path, builder contains either `EnvironmentVariable` and `LazyEnvironmentVariable` methods. If `LazyEnvironmentVariable` is used, the variable will be resolved using provider when accessing command arg value, otherwise during initialisation.

### Set parameter
Parameter is an unprefixed value argument. Parameters are read by registration order.
Values are converted to type T. CLR types, classes with implicit operator from string and classes with string argument constructor are supported by default. For other types see [converters](#converters) section.
```c#
Cli.Configure(c => {}).Root(configurator => configurator.RegisterParameter("param", "Param description", "paramDefaultValue"));
```

Alternative way of setting parameter is to use parameterBuilder.
```c#
Cli
    .Configure(c => {})
    .Root(configurator => configurator
        .RegisterParameter<double>(parameterBuilder => parameterBuilder
            .Name("parameter")
            .Description("parameter description")
            .EnvironmentVariable("NFLAGS_PARAMETER") // .LazyEnvironmentVariable("NFLAGS_PARAMETER")
            .ConfigPath("app.settings.parameter") // .LazyConfigPath("app.settings.parameter")
            .DefaultValue(1.2)
            .Converter(new ValueConverter())
        )
    );
```

When registering parameter, builder contains either `EnvironmentVariable` and `LazyEnvironmentVariable` methods. If `LazyEnvironmentVariable` is used, the variable will be resolved using provider when accessing command arg value, otherwise during initialisation.
Same goes to setting config path, builder contains either `EnvironmentVariable` and `LazyEnvironmentVariable` methods. If `LazyEnvironmentVariable` is used, the variable will be resolved using provider when accessing command arg value, otherwise during initialisation.

### Set parameter series
Parameter series is a collection of parameters after last named parameter.
Parameter series can be used to parse unknown count of parameters to process i.e. strings to concat.
Values are converted to type T. CLR types, classes with implicit operator from string and classes with string argument constructor are supported by default. For other types see [converters](#converters) section.

```c#
Cli.Configure(c => {}).Root(configurator => configurator.RegisterParamSeries<int>("paramSeries", "Param series description"));
```
There is also non-generic method where argument type is string
```c#
Cli.Configure(c => {}).Root(configurator => configurator.RegisterParamSeries("paramSeries", "Param series description"));
```

### Argument grouping
Both flags and options can attached to groups and printed in separate section in help.
```c#
Cli.Configure(c => {}).Root(c => c.RegisterFlag(b => b.Name("flag").Group("group"));
Cli.Configure(c => {}).Root(c => c.RegisterOption<string>(b => b.Name("option").Group("group"));
```

The following code:
```c#
    Cli
        .Configure(c => c
            .SetDialect(Dialect.Gnu)
        )
        .Root(c => c
            .RegisterParameter<string>(b => b.Name("param1"))
            .RegisterFlag(b => b.Name("flag1"))
            .RegisterFlag(b => b.Name("group1-flag1").Group("group1"))
            .RegisterFlag(b => b.Name("group2-flag1").Group("group2"))
            .RegisterFlag(b => b.Name("group1-flag2").Group("group1"))
            .RegisterFlag(b => b.Name("flag2"))
            .RegisterOption<string>(b => b.Name("option1"))
            .RegisterOption<string>(b => b.Name("group1-option1").Group("group1"))
            .RegisterOption<string>(b => b.Name("group2-option1").Group("group2"))
            .RegisterOption<string>(b => b.Name("group1-option2").Group("group1"))
            .RegisterOption<string>(b => b.Name("option2"))
            .PrintHelpOnExecute()
        )
        .Run(args);
```
will print following help test
```
$> dotnet NFlags.Groups.dll
Usage:
        NFlags.Groups [OPTIONS]... [PARAMETERS]...

        Parameters:
        <param1>

        Options:
        --flag1
        --flag2
        --option1 <option1>
        --option2 <option2>
        --help, -h      Prints this help

        group1:
                --group1-flag1
                --group1-flag2
                --group1-option1 <group1-option1>
                --group1-option2 <group1-option2>

        group2:
                --group2-flag1
                --group2-option1 <group2-option1>
```

### Attach code to execution

To attach code to execution by command, simply call `SetExecution` method of command configurator and pass `Action<CommandArgs, Action<string>>` callback.
First argument of action contains all registered Flags, Options and Parameters with default or given values. The second one is callback to print output from command.
```c#
Cli.Configure(c => {}).Root(configurator => configurator.SetExecute((commandArgs, output) => output.WriteLine("This is command output: " + commandArgs.GetParameter<string>("param")));
```
If execute is of type `Func<CommandArgs, IOutput, int>` result will be returned by `Bootstrap.Run` to be used as exit code.

### Attach sub commands

To attach subcommand, call `RegisterCommand` method of command configurator.
The third parameter is a configurator for the sub command and can be used in the same Way as the one for root command..
```c#
Cli.Configure(c => {}).
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
Cli.Configure(c => {}).
    Root(configurator => configurator.
        RegisterDefaultCommand("default", "Default command Description", sc => sc.
                SetExecute((commandArgs, output) => output.WriteLine("This is default command"))
        )
    );
```

### Parsing arguments
To parse arguments and execute requested command call:
```c#
Cli.Configure(c => {}).Root(configurator => {}).Run(args);
```


## Dialects
Dialect defines how flags and options are prefixed and how option value follows option.
By default 2 dialect are supported: **Gnu**, **Win**

Dialect can be set (default is Win) using:
```c#
Cli.Configure(configurator => configurator.SetDialect(Dialect.Gnu));
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
Cli.Configure(configurator => configurator.SetDialect(new CustomDialect()));
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
Console.WriteLine(Cli.Configure(configurator => {}).PrintHelp());
```

Example help for Win dialect:

```
Usage:
        NFlags.Win [COMMAND] [OPTIONS]... [PARAMETERS]...

Application description

        Commands:
        show    Show somethig
        list    List something

        Parameters:
        <param1>        Parameter 1 description (Default: '.')

        Options:
        /verbose, /v    Verbose description
        /clear  Clear description
        /option1=<option1>, /o1=<option1>       Option 1 description (Default: 'default')
        /option2=<option2>      Option 2 description (Default: 'default2')
        /help, /h       Prints this help



```

and for Gnu dialect:
```
Usage:
        NFlags.Gnu [COMMAND] [OPTIONS]... [PARAMETERS]...

Application description

        Commands:
        show    Show somethig
        list    List something

        Parameters:
        <param1>        Parameter 1 description (Default: '.')

        Options:
        --verbose, -v   Verbose description
        --clear Clear description
        --option1 <option1>, -o1 <option1>      Option 1 description (Default: 'default')
        --option2 <option2>     Option 2 description (Default: 'default2')
        --help, -h      Prints this help


```

Command can be also configured to print help on execute. This is useful when creating command who aggregates set of sub commands.
The following code:
```c#
Cli
    .Configure(c => c.SetDialect(Dialect.Gnu))
    .Root(c => c.PrintHelpOnExecute())
    .Run(args);
```
will print following output:
```
$> dotnet NFlags.Gnu.dll
Usage:
        NFlags.Gnu [OPTIONS]... [PARAMETERS]...

        Parameters:
        <parameter>     parameter description
        <parameterSeries...>    parameter series description

        Options:
        --flag, -f      flag description
        --option <option>, -o <option>  option description
        --help, -h      Prints this help
```
### Configuring help

#### Option flag and abbreviation
By default help option use `help` (with `h` abbreviation) to define print help option. This can be customized in help configuration
```c#
Cli.Configure(c => c.ConfigureHelp(hc => hc.SetOptionFlag("xhelp").SetOptionAbr("x")))
```

```
$> dotnet NFlags.CustomHelpFlags.dll -x
Usage:
        NFlags.CustomHelpFlags [OPTIONS]...

        Options:
        --xhelp, -x      Prints this help
```

#### Help flag help description text
Default help text for help option is `Prints this help`. This can be customized in help configuration
```c#
Cli.Configure(c => c.ConfigureHelp(hc => hc.SetOptionDescription("custom description")))
```

```
$> dotnet NFlags.CustomHelpDesc.dll
Usage:
        NFlags.CustomHelpFlags [OPTIONS]...

        Options:
        --help, -h       custom description
```

#### HelpPrinter
HelpPrinter is used to generate text output from NFlags configuration represented by `CommandConfig`. The default implementation can be replaced trough configuration to customize help printing.
```c#
Cli.Configure(c => c.ConfigureHelp(hc => hc.SetPrinter(new CustomHelpPrinter())))
```
`CustomHelpPrinter` must implement `IHelpPrinter` interface.

## Version

Printing application Version is supported "out of box". By default versions are disabled.

#### Enabling Version option
When enabled version option allows to print application version using special argument.
```c#
Cli.Configure(configurator => configurator.ConfigureVersion(vc => vc.Enable()));
```

#### Option flag and abbreviation
By default version option use `version` (with `v` abbreviation) to define print version option. This can be customized in version configuration
```c#
Cli.Configure(c => c.ConfigureVersion(vc => vc.Enable().SetOptionFlag("xversion").SetOptionAbr("x")))
```

```
$> dotnet NFlags.CustomVersionFlags.dll -x
Usage:
        NFlags.CustomVersionFlags [OPTIONS]...

        Options:
        --help, -h      Prints this help
        --xversion, -x      Prints application version
```

#### Version flag help description text
Default help text for version option is `Prints application version`. This can be customized in version configuration
```c#
Cli.Configure(c => c.ConfigureVersion(vc => vc.Enable().SetOptionDescription("custom version description")))
```

```
$> dotnet NFlags.CustomVersionFlags.dll
Usage:
        NFlags.CustomVersionFlags [OPTIONS]...

        Options:
        --help, -h       Prints this help
        --version, -v      custom version description
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
Cli
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

Generics also supports lazy environment and configuration binding. When property is of type `Lazy<>` the value will be
resolved when accessed by `Value` property of `Lazy<>` type.

```c#
    public class RootCommandArguments
    {
        [Option(Name = "option", ConfigPath = "config.option", EnvironmentVariable = "NFLAGS_OPTION_TEST_ENV", DefaultValue = 1)]
        public Lazy<int> Option;

        [Flag(Name = "flag", ConfigPath = "config.flag", EnvironmentVariable = "NFLAGS_FLAG_TEST_ENV", DefaultValue = true)]
        public Lazy<bool> Flag;

        [Flag(Name = "parameter", ConfigPath = "config.parameter", EnvironmentVariable = "NFLAGS_FLAG_PARAMETER_ENV", DefaultValue = 1.1)]
        public Lazy<double> Parameter;
    }
```

## Array support

When option is registered with array type NFlags allows to pass argument multiple times from CLI and aggregate it into array.

Running the following code:
```c#
    Cli.Configure(c => c
            .SetDescription("Application description")
            .SetDialect(Dialect.Gnu)
            .SetOutput(Output.Console))
        .Root(c => c
            .RegisterOption<string[]>(b => b.Name("array-option").DefaultValue(new string[0]))
            .SetExecute((commandArgs, output) =>
            {
                foreach (var option in commandArgs.GetOption<string[]>("carray-option"))
                    output.WriteLine(option);
            })
        )
        .Run(args);
```
Will result:
```
$> dotnet NFlags.Array.dll --array-option a --array-option b --array-option c
a
b
c
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
Cli.Configure(configurator => configurator.RegisterConverter(new UserConverter()));
```

Converter can be also set directly for argument trough builder method. If passed the converter from argument definition is used ,intstead of global registerd converters.
```c#
Cli
    .Configure(c => {})
    .Root(configurator => configurator
        .RegisterOption<double>(b => b
            .Name("option")
            .Converter(new ValueConverter())
        )
        .RegisterParameter<double>(b => b
            .Name("parameter")
            .Converter(new ValueConverter())
        )
        .RegisterParameterSeries<double>(b => b
            .Name("parameterSeries")
            .Converter(new ValueConverter())
        )
    );
```

## Exception handling

Default policy of exception handling in NFlags is to not throw exception when user use CLI incorrectly.
If Exception is thrown during parsing arguments, exception message and help is printed and exit code 255 is returned.
For the following application:
```c#
public static int Main(string[] args)
{
    return Cli
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
        NFlags.Empty [OPTIONS]... [PARAMETERS]...

        Parameters:
        <param> param description

        Options:
        /help, /h       Prints this help


Process finished with exit code 255.
```

Exception handling can be disabled using configuration:
```c#
Cli.Configure(c => c.DisableExceptionHandling());
```
If exception handling is disabled NFlags can throw `NFlags.TooManyParametersException` or `NFlags.TypeConverters.ArgumentValueException`.
