using System;
using NFlags.Commands;

namespace NFlags
{
    /// <summary>
    /// NFLags bootstrap
    /// </summary>
    public class Bootstrap
    {
        private readonly NFlagsConfig _nFlagsConfig;
        private readonly Command _rootCommand;

        /// <summary>
        /// Creates new instance of NFlags bootstrap
        /// </summary>
        /// <param name="rootCommand">Root command</param>
        /// <param name="nFlagsConfig">NFlags configuration</param>
        internal Bootstrap(NFlagsConfig nFlagsConfig, Command rootCommand)
        {
            _nFlagsConfig = nFlagsConfig;
            _rootCommand = rootCommand;
        }

        /// <summary>
        /// Parse arguments and run requested command
        /// </summary>
        /// <param name="args">Application arguments</param>
        public void Run(string[] args)
        {
            try
            {
                var c = _rootCommand.Read(args);
                c.Execute?.Invoke(c.Args, _nFlagsConfig.Output);
            }
            catch (Exception e)
            {
                _nFlagsConfig.Output(e.ToString());
            }
        }
    }
}