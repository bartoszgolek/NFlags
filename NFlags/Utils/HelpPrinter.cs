using System.Linq;
using System.Text;
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
            PrintFlags(sb);
            PrintOptions(sb);
            PrintParameters(sb);

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
            if (_commandConfig.Flags.Any())
                line += " [FLAGS]...";
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

        private void PrintFlags(StringBuilder builder)
        {
            if (!_commandConfig.Flags.Any())
                return;

            builder.AppendLine("\tFlags:");
            foreach (var flag in _commandConfig.Flags)
            {
                var line = "\t" + _commandConfig.NFlagsConfig.Dialect.Prefix + flag.Name;
                if (flag.Abr != null)
                    line += ", " + _commandConfig.NFlagsConfig.Dialect.AbrPrefix + flag.Abr;
                line += "\t" + flag.Description;
                builder.AppendLine(line);
            }

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

                line += "\t" + option.Description;
                builder.AppendLine(line);
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
                var line = "\t<" + parameter.Name;
                line += ">\t" + parameter.Description;
                builder.AppendLine(line);
            }

            if (_commandConfig.ParameterSeries != null)
            {
                var line = "\t<" + _commandConfig.ParameterSeries.Name + "...";
                line += ">\t" + _commandConfig.ParameterSeries.Description;
                builder.AppendLine(line);
            }

            builder.AppendLine("");
        }
    }
}