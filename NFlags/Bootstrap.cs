using NFlags.Commands;

namespace NFlags
{
    /// <summary>
    /// NFLags bootstrap
    /// </summary>
    public class Bootstrap
    {
        private readonly CliConfig _cliConfig;
        private readonly CommandConfig _rootCommandConfig;

        /// <summary>
        /// Creates new instance of NFlags bootstrap
        /// </summary>
        /// <param name="rootCommandConfigConfig">Root command config</param>
        /// <param name="cliConfig">NFlags configuration</param>
        internal Bootstrap(CliConfig cliConfig, CommandConfig rootCommandConfigConfig)
        {
            _cliConfig = cliConfig;
            _rootCommandConfig = rootCommandConfigConfig;
        }

        /// <summary>
        /// Parse arguments and run requested command
        /// </summary>
        /// <param name="args">Application arguments</param>
        public int Run(string[] args)
        {
            var c = new CommandExecutionContextProvider(
                _rootCommandConfig,
                args
            ).GetFromArgs();

            if (c.Execute == null)
                throw new MissingCommandImplementationException();

            return c.Execute.Invoke(c.Args, _cliConfig.Output);
        }
    }
}