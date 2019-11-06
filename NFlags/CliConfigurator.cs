using System;
using System.Collections.Generic;
using NFlags.TypeConverters;

namespace NFlags
{
    /// <summary>
    /// Represents NFlags configuration ability.
    /// </summary>
    public class CliConfigurator
    {
        private string _name = AppDomain.CurrentDomain.FriendlyName;

        private string _description = "";

        private Dialect _dialect = Dialect.Win;

        private IOutput _output = Output.Console;

        private IEnvironment _environment = Environment.System;

        private bool _exceptionHandling = true;

        private IConfig _config;

        private IGenericConfig _genericConfig;


        private readonly List<IArgumentConverter> _baseArgumentConverters = new List<IArgumentConverter> {
            new CommonTypeConverter(),
            new ConstructorConverter(),
            new ImplicitOperatorConverter()
        };

        private readonly List<IArgumentConverter> _argumentConverters = new List<IArgumentConverter>();
        private readonly HelpConfigurator _helpConfigurator = new HelpConfigurator();
        private VersionConfigurator _versionConfigurator = new VersionConfigurator();

        /// <summary>
        /// Set name of application, for help printing.
        /// </summary>
        /// <param name="name">Application name</param>
        /// <returns>Self instance</returns>
        public CliConfigurator SetName(string name)
        {
            _name = name;

            return this;
        }

        /// <summary>
        /// Set description of application, for help printing.
        /// </summary>
        /// <param name="description">Application description</param>
        /// <returns>Self instance</returns>
        public CliConfigurator SetDescription(string description)
        {
            _description = description;

            return this;
        }

        /// <summary>
        /// Sets NFlags dialect for parsing purposes.
        /// </summary>
        /// <param name="dialect">NFlags dialect</param>
        /// <returns>Self instance</returns>
        public CliConfigurator SetDialect(Dialect dialect)
        {
            _dialect = dialect;

            return this;
        }

        /// <summary>
        /// Sets NFlags output function. Default is Console.Write
        /// </summary>
        /// <param name="output">Output printing interface</param>
        /// <returns>Self instance</returns>
        public CliConfigurator SetOutput(IOutput output)
        {
            _output = output;

            return this;
        }

        /// <summary>
        /// Sets NFlags environment variables provider.
        /// </summary>
        /// <param name="environment">Environment variables provider</param>
        /// <returns>Self instance</returns>
        public CliConfigurator SetEnvironment(IEnvironment environment)
        {
            _environment = environment;

            return this;
        }

        /// <summary>
        /// Sets NFlags configuration values provider.
        /// </summary>
        /// <param name="config">Configuration values provider</param>
        /// <returns>Self instance</returns>
        public CliConfigurator SetConfiguration(IConfig config)
        {
            _config = config;

            return this;
        }

        /// <summary>
        /// Sets NFlags generic configuration values provider.
        /// </summary>
        /// <param name="genericConfig">Configuration values provider</param>
        /// <returns>Self instance</returns>
        public CliConfigurator SetConfiguration(IGenericConfig genericConfig)
        {
            _genericConfig = genericConfig;

            return this;
        }

        /// <summary>
        /// Sets help printer to generate help text.
        /// </summary>
        /// <param name="helpPrinter">Help printer</param>
        /// <returns>Self instance</returns>
        [Obsolete("SetHelpPrinter method is obsolete. Use CliConfig.ConfigureHelp() instead.")]
        public CliConfigurator SetHelpPrinter(IHelpPrinter helpPrinter)
        {
            _helpConfigurator.SetPrinter(helpPrinter);

            return this;
        }

        /// <summary>
        /// Configure help generation
        /// </summary>
        /// <param name="configureHelp">Configure help action</param>
        /// <returns>Self instance</returns>
        public CliConfigurator ConfigureHelp(Action<HelpConfigurator> configureHelp)
        {
            configureHelp(_helpConfigurator);

            return this;
        }

        /// <summary>
        /// Registers Converter to convert argument values
        /// </summary>
        /// <param name="argumentConverter">Param Converter to register</param>
        /// <returns>Self instance</returns>
        public CliConfigurator RegisterConverter(IArgumentConverter argumentConverter)
        {
            _argumentConverters.Add(argumentConverter);

            return this;
        }

        /// <summary>
        /// Disables exception handling. Id used NFlags will throw exceptions instead of return exit code;
        /// </summary>
        /// <returns>Self instance</returns>
        public CliConfigurator DisableExceptionHandling()
        {
            _exceptionHandling = false;
            return this;
        }

        /// <summary>
        /// Enables default version option;
        /// </summary>
        /// <returns>Self instance</returns>
        [Obsolete("EnableVersionOption method is obsolete. Use CliConfig.ConfigureVersion() instead.")]
        public CliConfigurator EnableVersionOption()
        {
            _versionConfigurator.Enable();
            return this;
        }

        /// <summary>
        /// Configure version option
        /// </summary>
        /// <param name="configureVersion">Configure version option action</param>
        /// <returns>Self instance</returns>
        public CliConfigurator ConfigureVersion(Action<VersionConfigurator> configureVersion)
        {
            configureVersion(_versionConfigurator);

            return this;
        }

        internal Cli CreateCli()
        {
            var converters = new List<IArgumentConverter>(_argumentConverters);
            converters.AddRange(_baseArgumentConverters);
            return new Cli(
                new CliConfig( _name, _description, _dialect, _output, _environment, _config, _genericConfig, _helpConfigurator.GetHelpConfig(), _exceptionHandling, _versionConfigurator.GetVersionConfig(), converters.ToArray())
            );
        }
    }
}