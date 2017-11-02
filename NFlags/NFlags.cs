using System;

namespace NFlags
{
    /// <summary>
    /// Provides API for NFlags usage.
    /// </summary>
    public static class NFlags
    {
        /// <summary>
        /// Configure NFLags param reader.
        /// </summary>
        /// <param name="configurator">Param reader configurator.</param>
        /// <returns>Configured param reader.</returns>
        public static ParamReader Configure(Action<ParamReaderConfigurator> configurator)
        {
            var paramReaderConfigurator = new ParamReaderConfigurator();
            configurator(paramReaderConfigurator);
            return paramReaderConfigurator.CreateParamReader();
        }
    }
}