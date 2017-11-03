using System;

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

        private Action<string> _output = Console.Write;

        /// <summary>
        /// Set name of aplication, for help printing.
        /// </summary>
        /// <param name="name">Application name</param>
        /// <returns>Self instance</returns>
        public NFlagsConfigurator SetName(string name)
        {
            _name = name;

            return this;
        }

        /// <summary>
        /// Set description of aplication, for help printing.
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
        /// <param name="output">Function used to print output</param>
        /// <returns>Self instance</returns>
        public NFlagsConfigurator SetOutput(Action<string> output)
        {
            _output = output;

            return this;
        }

        internal NFlags CreateNFlags()
        {
            return new NFlags(
                new NFlagsConfig( _name, _description, _dialect, _output)
            );
        }
    }
}