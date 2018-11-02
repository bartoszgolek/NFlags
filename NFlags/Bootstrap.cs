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
        public int Run(string[] args)
        {
            var c = _rootCommand.Read(args);
            if (c.Execute == null)
                throw new MissingCommandImplementationException();

            return c.Execute.Invoke(c.Args, _nFlagsConfig.Output);
        }
    }
}