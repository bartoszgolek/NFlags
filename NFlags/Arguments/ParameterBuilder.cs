namespace NFlags.Arguments
{
    /// <summary>
    /// Builder for parameter arguments
    /// </summary>
    /// <typeparam name="T">Type of parameter value</typeparam>
    public class ParameterBuilder<T>
    {
        private string _name;
        private string _description;
        private T _defaultValue;
        private string _environmentVariable;
        private string _config;

        /// <summary>
        /// Set name of the argument.
        /// </summary>
        /// <param name="name">Name of the argument</param>
        /// <returns>Self instance</returns>
        public ParameterBuilder<T> Name(string name)
        {
            _name = name;

            return this;
        }

        /// <summary>
        /// Set description of the argument.
        /// </summary>
        /// <param name="description">Description of the argument</param>
        /// <returns>Self instance</returns>
        public ParameterBuilder<T> Description(string description)
        {
            _description = description;

            return this;
        }

        /// <summary>
        /// Set default value of the argument
        /// </summary>
        /// <param name="defaultValue">Default value of the argument</param>
        /// <returns>Self instance</returns>
        public ParameterBuilder<T> DefaultValue(T defaultValue)
        {
            _defaultValue = defaultValue;

            return this;
        }

        /// <summary>
        /// Set environment variable name for the argument value
        /// </summary>
        /// <param name="environmentVariable">Environment variable name for the argument value</param>
        /// <returns>Self instance</returns>
        public ParameterBuilder<T> EnvironmentVariable(string environmentVariable)
        {
            _environmentVariable = environmentVariable;

            return this;
        }

        /// <summary>
        /// Set environment variable name for the argument value
        /// </summary>
        /// <param name="config">Config value path for the argument value</param>
        /// <returns>Self instance</returns>
        public ParameterBuilder<T> Config(string config)
        {
            _config = config;

            return this;
        }

        /// <summary>
        /// Build parameter
        /// </summary>
        /// <returns>Parameter</returns>
        public Parameter Build()
        {
            return new Parameter
            {
                Name = _name,
                Description = _description,
                ValueType = typeof(T),
                DefaultValue = _defaultValue,
                EnvironmentVariable = _environmentVariable,
                Config = _config
            };
        }
    }
}