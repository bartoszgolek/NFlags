using NFlags.TypeConverters;

namespace NFlags
{
    /// <summary>
    /// Represents configuration of NFlags.
    /// </summary>
    public class NFlagsConfig
    {
        /// <summary>
        /// Creates new instance of NFlags config
        /// </summary>
        /// <param name="name">Application name</param>
        /// <param name="description">Application description</param>
        /// <param name="dialect">NSpec arguments dialect</param>
        /// <param name="output">Output writing interface</param>
        /// <param name="environment">Environment variables provider</param>
        /// <param name="isExceptionHandlingEnabled">Is Exception handling enabled. Use exit code if enabled, otherwise throw exceptions. Default False</param>
        /// <param name="argumentConverters">List of param converters</param>
        public NFlagsConfig(string name, string description, Dialect dialect, IOutput output, IEnvironment environment, bool isExceptionHandlingEnabled, IArgumentConverter[] argumentConverters)
        {
            Name = name;
            Description = description;
            Dialect = dialect;
            IsExceptionHandlingEnabled = isExceptionHandlingEnabled;
            Output = output;
            Environment = environment;
            ArgumentConverters = argumentConverters;
        }

        /// <summary>
        /// Application name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Application description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// NFlags arguments dialect
        /// </summary>
        public Dialect Dialect { get; }

        /// <summary>
        /// Is Exception handling enabled. Use exit code if enabled, otherwise throw exceptions. Default False.
        /// </summary>
        public bool IsExceptionHandlingEnabled { get; }

        /// <summary>
        /// List of parameter converters
        /// </summary>
        public IArgumentConverter[] ArgumentConverters { get; }

        /// <summary>
        /// NFlags output handler.
        /// </summary>
        public IOutput Output { get; }

        /// <summary>
        /// Environments variable provider
        /// </summary>
        public IEnvironment Environment { get; }
    }
}