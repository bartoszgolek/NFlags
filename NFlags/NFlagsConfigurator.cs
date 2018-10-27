using System;
using System.Collections.Generic;
using System.Linq;
using NFlags.TypeConverters;

namespace NFlags
{
    /// <summary>
    /// Represents NFlags configuration ability. 
    /// </summary>
    public class NFlagsConfigurator
    {
        private string _name = AppDomain.CurrentDomain.FriendlyName;
        
        private string _description = "";
        
        private Dialect _dialect = Dialect.Win;

        private IOutput _output = Output.Console;
        
        private readonly List<IArgumentConverter> _argumentConverters = new List<IArgumentConverter> {
            new CommonTypeConverter(),
            new ConstructorConverter(),
            new ImplicitOperatorConverter(),
        };

        /// <summary>
        /// Set name of application, for help printing.
        /// </summary>
        /// <param name="name">Application name</param>
        /// <returns>Self instance</returns>
        public NFlagsConfigurator SetName(string name)
        {
            _name = name;

            return this;
        }

        /// <summary>
        /// Set description of application, for help printing.
        /// </summary>
        /// <param name="description">Application description</param>
        /// <returns>Self instance</returns>
        public NFlagsConfigurator SetDescription(string description)
        {
            _description = description;

            return this;
        }

        /// <summary>
        /// Sets NFlags dialect for parsing purposes.
        /// </summary>
        /// <param name="dialect">NFlags dialect</param>
        /// <returns>Self instance</returns>
        public NFlagsConfigurator SetDialect(Dialect dialect)
        {
            _dialect = dialect;

            return this;
        }

        /// <summary>
        /// Sets NFlags output function. Default is Console.Write
        /// </summary>
        /// <param name="output">Output printing interface</param>
        /// <returns>Self instance</returns>
        public NFlagsConfigurator SetOutput(IOutput output)
        {
            _output = output;

            return this;
        }

        /// <summary>
        /// Registers Convrter to convert argument values
        /// </summary>
        /// <param name="argumentConverter">Param Converter to register</param>
        /// <returns>Self instance</returns>
        public NFlagsConfigurator RegisterConverter(IArgumentConverter argumentConverter)
        {
            _argumentConverters.Add(argumentConverter);            

            return this;
        }

        internal NFlags CreateNFlags()
        {
            return new NFlags(
                new NFlagsConfig( _name, _description, _dialect, _argumentConverters.ToArray(), _output)
            );
        }
    }
}