using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFlags.Arguments;
using NFlags.Commands;

namespace NFlags.Utils
{
    /// <inheritdoc />
    /// <summary>
    /// Default print helper for NFlags.
    /// </summary>
    public class DefaultHelpPrinter : IHelpPrinter
    {
        private readonly DefaultHelpPrinterOptions _options;

        /// <summary>
        /// Create new instance of DefaultHelpPrinter
        /// </summary>
        public DefaultHelpPrinter()
        {
            _options = new DefaultHelpPrinterOptions();
        }

        /// <summary>
        /// Create new instance of DefaultHelpPrinter
        /// </summary>
        /// <param name="options">Help printer options</param>
        public DefaultHelpPrinter(DefaultHelpPrinterOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public string PrintHelp(CommandConfig commandConfig)
        {
            return new HelpPrinterInt(commandConfig, _options).Print();
        }

        private class HelpPrinterInt
        {
            private readonly CommandConfig _commandConfig;
            private readonly DefaultHelpPrinterOptions _options;

            public HelpPrinterInt(CommandConfig commandConfig, DefaultHelpPrinterOptions options)
            {
                _commandConfig = commandConfig;
                _options = options;
            }


            public string Print()
            {
                var sb = new StringBuilder();
                PrintUsage(sb);
                PrintCommands(sb);
                PrintParameters(sb);
                PrintOptions(sb);

                return sb.ToString();
            }

            private void PrintUsage(StringBuilder builder)
            {
                builder.AppendLine("Usage:");
                var line = "\t";

                line += _commandConfig.NFlagsConfig.Name;
                line += string.Join(" ", _commandConfig.Parents.ToArray());

                if (!string.IsNullOrEmpty(_commandConfig.Name))
                    line += " " + _commandConfig.Name;
                if (_commandConfig.Commands.Any())
                    line += " [COMMAND]";
                if (_commandConfig.Options.Any())
                    line += " [OPTIONS]...";
                if (_commandConfig.Parameters.Any() || _commandConfig.ParameterSeries != null)
                    line += " [PARAMETERS]...";

                builder.AppendLine(line);
                builder.AppendLine("");

                if (string.IsNullOrEmpty(_commandConfig.NFlagsConfig.Description))
                    return;

                builder.AppendLine(_commandConfig.NFlagsConfig.Description);
                builder.AppendLine("");
            }

            private void PrintCommands(StringBuilder builder)
            {
                if (!_commandConfig.Commands.Any())
                    return;

                builder.AppendLine("\tCommands:");
                foreach (var command in _commandConfig.Commands)
                {
                    var line = "\t" + command.Name;
                    line += "\t" + command.Description;
                    builder.AppendLine(line);
                }

                builder.AppendLine("");
            }

            private void PrintOptions(StringBuilder builder)
            {
                if (!_commandConfig.Options.Any())
                    return;

                var groups = new Dictionary<string, StringBuilder>();

                var optionFormatter = OptionFormatter.GetFormatter(_commandConfig.NFlagsConfig.Dialect);

                builder.AppendLine("\tOptions:");
                foreach (var option in _commandConfig.Options)
                {
                    var indentation = "\t";
                    var optionGroup = option.Group ?? string.Empty;
                    if (optionGroup != "")
                        indentation = "\t\t";

                    var line = indentation + optionFormatter.FormatName(option);
                    if (option.Abr != null)
                        line += ", " + optionFormatter.FormatAbr(option);

                    line += PrintDescription(option);

                    if (optionGroup != "")
                    {
                        if (!groups.ContainsKey(optionGroup))
                            groups.Add(optionGroup, new StringBuilder().AppendLine($"\t{optionGroup}:"));

                        groups[optionGroup].AppendLine(line);
                    }
                    else
                        builder.AppendLine(line);
                }

                foreach (var kvp in groups)
                {
                    builder.AppendLine("");
                    builder.Append(kvp.Value);
                }

                builder.AppendLine("");
            }

            private void PrintParameters(StringBuilder builder)
            {
                if (!_commandConfig.Parameters.Any() && _commandConfig.ParameterSeries == null)
                    return;

                builder.AppendLine("\tParameters:");
                foreach (var parameter in _commandConfig.Parameters)
                {
                    var line = $"\t<{parameter.Name}>";

                    line += PrintDescription(parameter);

                    builder.AppendLine(line);
                }

                if (_commandConfig.ParameterSeries != null)
                {
                    var line = $"\t<{_commandConfig.ParameterSeries.Name}...>";
                    if (!string.IsNullOrEmpty(_commandConfig.ParameterSeries.Description))
                        line += "\t" + _commandConfig.ParameterSeries.Description;
                    builder.AppendLine(line);
                }

                builder.AppendLine("");
            }

            private string PrintDescription(DefaultValueArgument argument)
            {
                var description = "";
                var separator = "\t";
                if (!string.IsNullOrEmpty(argument.Description))
                {
                    description += separator + argument.Description;
                    separator = " ";
                }

                if ((!argument.RequireValue || argument.DefaultValue == null) &&
                    string.IsNullOrEmpty(argument.EnvironmentVariable) && string.IsNullOrEmpty(argument.ConfigPath))
                    return description;

                var additionalDescription = PrintAdditionalDescription(argument);
                if (additionalDescription != String.Empty)
                    description += separator + "(" + additionalDescription + ")";

                return description;
            }

            private string PrintAdditionalDescription(DefaultValueArgument argument)
            {
                var description = string.Empty;

                var separator = "";
                if (argument.RequireValue && argument.DefaultValue != null)
                {
                    description += separator + PrintDefaultValue(argument);
                    separator = ", ";
                }

                if (!string.IsNullOrEmpty(argument.EnvironmentVariable))
                {
                    description += separator + PrintEnvironmentVariable(argument);
                    separator = ", ";
                }

                if (!string.IsNullOrEmpty(argument.ConfigPath))
                    description += separator + PrintConfigPath(argument);

                return description;
            }

            private string PrintDefaultValue(DefaultValueArgument argument)
            {
                return _options.PrintDefaultValues
                    ? string.Format(_options.DefaultValueText, Enquote(argument.ValueType, argument.DefaultValue))
                    : string.Empty;
            }

            private string PrintEnvironmentVariable(DefaultValueArgument argument)
            {
                return _options.PrintEnvironmentBindings
                    ? string.Format(_options.EnvironmentBindingsText, Enquote(typeof(string), argument.EnvironmentVariable))
                    : string.Empty;
            }

            private string PrintConfigPath(DefaultValueArgument argument)
            {
                return _options.PrintConfigurationBindings
                    ? string.Format(_options.ConfigurationBindingsText, Enquote(typeof(string), argument.ConfigPath))
                    : string.Empty;
            }

            private static string Enquote(Type type, object value)
            {
                return type == typeof(string)
                    ? $"'{value}'"
                    : value.ToString();
            }
        }
    }
}