# NFlags

Simple library to made parsing CLI arguments easy.
Library also allow You to print usage help "out of box".

For example of usage check **Examples** directory.

## Basics

### Set app name
Name set with folowing code, will be printed in help.
By default `AppDomain.CurrentDomain.FriendlyName` is used.
```c#
NFlags.Configure(configurator => configurator.SetName("Custom Name"));
```

### Set app description
Description set with folowing code, will be printed in help.
```c#
NFlags.Configure(configurator => configurator.SetDescription("App description"));
```

### Set flag
Flag is an prefixed argument without value. It can be turned on or off.
Flag abreviation can be also set.
```c#
NFlags.Configure(configurator => configurator.RegiasterFlag("flag", "f", "Flag description", () => flagVariable = true));
NFlags.Configure(configurator => configurator.RegiasterFlag("flag", "Flag description", () => flagVariable = true));
```

### Set option
Option is an prefixed argument with value.
Flag abreviation can be also set.
```c#
NFlags.Configure(configurator => configurator.RegiasterOption("option", "o", "option description", v => optionVariable = v));
NFlags.Configure(configurator => configurator.RegiasterOption("option", "option description", v => optionVariable = v));
```

### Set parameter
Parameter is an unprefixed value argument. Parameters are read by registration order.
```c#
NFlags.Configure(configurator => configurator.RegiasterParam("param", "Param description", v => paramVariable = v));
```


### Parsing arguments
To parse arguments call:
```c#
NFlags.Configure(configurator => {}).Read(args);
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
        NFlags.Win [FLAGS]... [OPTIONS]... [PARAMETERS]...

Application description

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
        NFlags.Gnu [FLAGS]... [OPTIONS]... [PARAMETERS]...

Application description

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