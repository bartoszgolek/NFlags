namespace NFlags.Arguments
{
    /// <summary>
    /// Builder for option arguments
    /// </summary>
    /// <typeparam name="T">Type of option value</typeparam>
    public class OptionBuilder<T>
    {
        private string _name;
        private string _description;
        private string _abr;
        private bool _isPersistent;
        private T _defaultValue;
        private string _environmentVariable;
        private string _configPath;
        private bool _isEnvironmentVariableLazy;
        private bool _isConfigPathLazy;

        /// <summary>
        /// Set name of the argument.
        /// </summary>
        /// <param name="name">Name of the argument</param>
        /// <returns>Self instance</returns>
        public OptionBuilder<T> Name(string name)
        {
            _name = name;

            return this;
        }

        /// <summary>
        /// Set description of the argument.
        /// </summary>
        /// <param name="description">Description of the argument</param>
        /// <returns>Self instance</returns>
        public OptionBuilder<T> Description(string description)
        {
            _description = description;

            return this;
        }

        /// <summary>
        /// Set flag abbreviation
        /// </summary>
        /// <param name="abr">Flag abbreviation</param>
        /// <returns>Self instance</returns>
        public OptionBuilder<T> Abr(string abr)
        {
            _abr = abr;

            return this;
        }

        /// <summary>
        /// Make flag persistent.
        /// </summary>
        /// <returns>Self instance</returns>
        public OptionBuilder<T> Persistent()
        {
            _isPersistent = true;

            return this;
        }

        /// <summary>
        /// Set default value of the argument
        /// </summary>
        /// <param name="defaultValue">Default value of the argument</param>
        /// <returns>Self instance</returns>
        public OptionBuilder<T> DefaultValue(T defaultValue)
        {
            _defaultValue = defaultValue;

            return this;
        }

        /// <summary>
        /// Set environment variable name for the argument value
        /// </summary>
        /// <param name="environmentVariable">Environment variable name for the argument value</param>
        /// <returns>Self instance</returns>
        public OptionBuilder<T> EnvironmentVariable(string environmentVariable)
        {
            _environmentVariable = environmentVariable;
            _isEnvironmentVariableLazy = false;

            return this;
        }

        /// <summary>
        /// Set lazy environment variable name for the argument value. Environment variable will be resolved on each read.
        /// </summary>
        /// <param name="environmentVariable">Environment variable name for the argument value</param>
        /// <returns>Self instance</returns>
        public OptionBuilder<T> LazyEnvironmentVariable(string environmentVariable)
        {
            _environmentVariable = environmentVariable;
            _isEnvironmentVariableLazy = true;

            return this;
        }

        /// <summary>
        /// Set environment variable name for the argument value
        /// </summary>
        /// <param name="config">Config value path for the argument value</param>
        /// <returns>Self instance</returns>
        public OptionBuilder<T> ConfigPath(string config)
        {
            _configPath = config;
            _isConfigPathLazy = false;

            return this;
        }

        /// <summary>
        /// Set lazy environment variable name for the argument value
        /// </summary>
        /// <param name="config">Config value path for the argument value</param>
        /// <returns>Self instance</returns>
        public OptionBuilder<T> LazyConfigPath(string config)
        {
            _configPath = config;
            _isConfigPathLazy = true;

            return this;
        }

        /// <summary>
        /// Build option
        /// </summary>
        /// <returns>Option</returns>
        public Option Build()
        {
            return new Option
            {
                Name = _name,
                Description = _description,
                ValueType = typeof(T),
                DefaultValue = _defaultValue,
                EnvironmentVariable = _environmentVariable,
                IsEnvironmentVariableLazy = _isEnvironmentVariableLazy,
                ConfigPath = _configPath,
                IsConfigPathLazy = _isConfigPathLazy,
                Abr = _abr,
                IsPersistent = _isPersistent
            };
        }
    }
}