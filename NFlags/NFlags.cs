using System;

namespace NFlags
{
    public static class NFlags
    {
        private static readonly ParamReaderConfigurator Configurator = new ParamReaderConfigurator();

        public static ParamReader Configure(Action<ParamReaderConfigurator> configurator)
        {
            var paramReaderConfigurator = new ParamReaderConfigurator();
            configurator(paramReaderConfigurator);
            return paramReaderConfigurator.CreateParamReader();
        }

        public static void Read(string[] args)
        {
            Configurator.CreateParamReader().Read(args);
        }
    }
}