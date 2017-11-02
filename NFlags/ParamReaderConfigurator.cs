using System;

namespace NFlags
{
    public class ParamReaderConfigurator
    {
        private string _name = AppDomain.CurrentDomain.FriendlyName;
        
        private string _description = "";
        
        private Dialect _dialect = Dialect.Win;

        public ParamReaderConfigurator SetName(string name)
        {
            _name = name;

            return this;
        }

        public ParamReaderConfigurator SetDescription(string description)
        {
            _description = description;

            return this;
        }

        public ParamReaderConfigurator SetDialect(Dialect dialect)
        {
            _dialect = dialect;

            return this;
        }

        internal ParamReader CreateParamReader()
        {
            return new ParamReader(
                new NFlagsConfig( _name, _description, _dialect)
            );
        }
    }
}