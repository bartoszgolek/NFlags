using System;
using System.Linq;
using System.Text;

namespace NFlags
{
    public class HelpPrinter
    {
        private readonly NFlagsConfig _readerConfig;

        private readonly CommandConfig _commandConfig;

        public HelpPrinter(NFlagsConfig readerConfig, CommandConfig commandConfig)
        {
            _readerConfig = readerConfig;
            _commandConfig = commandConfig;
        }

        public string Print()
        {
            var sb = new StringBuilder();
            PrintUsage(line => sb.Append(line + Environment.NewLine));
            PrintFlags(line => sb.Append(line + Environment.NewLine));
            PrintOptions(line => sb.Append(line + Environment.NewLine));
            PrintParameters(line => sb.Append(line + Environment.NewLine));

            return sb.ToString();
        }

        private void PrintUsage(Action<string> writeLine)
        {
            writeLine("Usage:");
            var line = "\t";

            line += _readerConfig.Name;           
            if (_commandConfig.Flags.Any())
                line += " [FLAGS]...";
            if (_commandConfig.Options.Any())
                line += " [OPTIONS]...";
            if (_commandConfig.Parameters.Any())
                line += " [PARAMETERS]...";

            writeLine(line);
            writeLine("");

            if (string.IsNullOrEmpty(_readerConfig.Description)) 
                return;
            
            writeLine(_readerConfig.Description);
            writeLine("");
        }

        private void PrintFlags(Action<string> writeLine)
        {
            if (!_commandConfig.Flags.Any()) 
                return;

            writeLine("\tFlags:");
            foreach (var flag in _commandConfig.Flags)
            {
                var line = "\t" + _readerConfig.Dialect.Prefix + flag.Name;
                if (flag.Abr != null)
                    line += ", " + _readerConfig.Dialect.AbrPrefix + flag.Abr;
                line += "\t" + flag.Description;
                writeLine(line);
            }

            writeLine("");
        }

        private void PrintOptions(Action<string> writeLine)
        {
            if (!_commandConfig.Options.Any()) 
                return;

            var optionFormatter = OptionFormatter.GetFormatter(_readerConfig.Dialect);
            
            writeLine("\tOptions:");
            foreach (var option in _commandConfig.Options)
            {
                var line = "\t" + optionFormatter.FormatName(option);
                if (option.Abr != null)
                    line += ", " + optionFormatter.FormatAbr(option);
                
                line += "\t" + option.Description;
                writeLine(line);
            }
            
            writeLine("");
        }

        private void PrintParameters(Action<string> writeLine)
        {
            if (!_commandConfig.Parameters.Any()) 
                return;

            writeLine("\tParameters:");
            foreach (var parameter in _commandConfig.Parameters)
            {
                var line = "\t<" + parameter.Name;
                line += ">\t" + parameter.Description;
                writeLine(line);
            }
            
            writeLine("");
        }
    }
}