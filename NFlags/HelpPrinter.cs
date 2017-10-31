using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFlags
{
    public class HelpPrinter
    {
        private readonly string _name;
        private readonly string _description;
        private readonly Dialect _dialect;
        private readonly List<Flag> _flags;
        private readonly List<Arg> _parameters;
        private readonly List<Option> _options;

        public HelpPrinter(
            string name,
            string description,
            Dialect dialect, 
            List<Flag> flags, 
            List<Arg> parameters, 
            List<Option> options)
        {
            _name = name;
            _description = description;
            _dialect = dialect;
            _flags = flags;
            _parameters = parameters;
            _options = options;
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

            line += _name;           
            if (_flags.Any())
                line += " [FLAGS]...";
            if (_options.Any())
                line += " [OPTIONS]...";
            if (_parameters.Any())
                line += " [PARAMETERS]...";

            writeLine(line);
            writeLine("");

            if (string.IsNullOrEmpty(_description)) 
                return;
            
            writeLine(_description);
            writeLine("");
        }

        private void PrintFlags(Action<string> writeLine)
        {
            if (!_flags.Any()) 
                return;

            writeLine("\tFlags:");
            foreach (var flag in _flags)
            {
                var line = "\t" + _dialect.Prefix + flag.Name;
                if (flag.Abr != null)
                    line += ", " + _dialect.AbrPrefix + flag.Abr;
                line += "\t" + flag.Description;
                writeLine(line);
            }

            writeLine("");
        }

        private void PrintOptions(Action<string> writeLine)
        {
            if (!_options.Any()) 
                return;

            var optionFormatter = OptionFormatter.GetFormatter(_dialect);
            
            writeLine("\tOptions:");
            foreach (var option in _options)
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
            if (!_parameters.Any()) 
                return;

            writeLine("\tParameters:");
            foreach (var parameter in _parameters)
            {
                var line = "\t<" + parameter.Name;
                line += ">\t" + parameter.Description;
                writeLine(line);
            }
            
            writeLine("");
        }
    }
}