﻿using System.Linq;
using System.Text;
using NFlags.Arguments;
using NFlags.Commands;

namespace NFlags.Utils
{
    internal class HelpPrinter
    {
        private readonly CommandConfig _commandConfig;

        public HelpPrinter(CommandConfig commandConfig)
        {
            _commandConfig = commandConfig;
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

            var optionFormatter = OptionFormatter.GetFormatter(_commandConfig.NFlagsConfig.Dialect);

            builder.AppendLine("\tOptions:");
            foreach (var option in _commandConfig.Options)
            {
                var line = "\t" + optionFormatter.FormatName(option);
                if (option.Abr != null)
                    line += ", " + optionFormatter.FormatAbr(option);

                var separator = "\t";
                if (!string.IsNullOrEmpty(option.Description))
                {
                    line += separator + option.Description;
                    separator = " ";
                }

                if (option.RequireValue && option.DefaultValue != null)
                    line += separator + "(" + PrintDefaultValue(option) + ")";

                builder.AppendLine(line);
            }

            builder.AppendLine("");
        }

        private static object PrintDefaultValue(DefaultValueArgument argument)
        {
            if (argument.ValueType == typeof(string))
                return "Default: '" + argument.DefaultValue + "'";

            return "Default: " + argument.DefaultValue;
        }

        private void PrintParameters(StringBuilder builder)
        {
            if (!_commandConfig.Parameters.Any() && _commandConfig.ParameterSeries == null)
                return;

            builder.AppendLine("\tParameters:");
            foreach (var parameter in _commandConfig.Parameters)
            {
                var line = "\t<" + parameter.Name + ">";

                var separator = "\t";
                if (!string.IsNullOrEmpty(parameter.Description))
                {
                    line += separator + parameter.Description;
                    separator = " ";
                }

                if (parameter.DefaultValue != null)
                    line += separator + "(" + PrintDefaultValue(parameter) + ")";

                builder.AppendLine(line);
            }

            if (_commandConfig.ParameterSeries != null)
            {
                var line = "\t<" + _commandConfig.ParameterSeries.Name + "...>";
                if (!string.IsNullOrEmpty(_commandConfig.ParameterSeries.Description))
                    line += "\t" + _commandConfig.ParameterSeries.Description;
                builder.AppendLine(line);
            }

            builder.AppendLine("");
        }
    }
}