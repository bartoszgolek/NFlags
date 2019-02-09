using System;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Mark field or property as parameter to register in command
    /// </summary>
    /// <remarks>If property, then has to be settable.</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of ParameterAttribute
        /// </summary>
        public ParameterAttribute()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of ParameterAttribute
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="description">Description of parameter for help</param>
        /// <param name="defaultValue">Default value</param>
        public ParameterAttribute(string name, string description, object defaultValue)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of ParameterAttribute
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="description">Description of parameter for help</param>
        /// <param name="environmentVariable">Name of environment variable to use.</param>
        /// <param name="defaultValue">Default value</param>
        public ParameterAttribute(string name, string description, string environmentVariable, object defaultValue)
        {
            Name = name;
            Description = description;
            EnvironmentVariable = environmentVariable;
            DefaultValue = defaultValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of ParameterAttribute
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="description">Description of parameter for help</param>
        /// <param name="environmentVariable">Name of environment variable to use.</param>
        /// <param name="configPath">Path to configuration value to use.</param>
        /// <param name="defaultValue">Default value</param>
        public ParameterAttribute(string name, string description, string environmentVariable, string configPath, object defaultValue)
        {
            Name = name;
            Description = description;
            EnvironmentVariable = environmentVariable;
            ConfigPath = configPath;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Name of parameter
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// Description of parameter for help
        /// </summary>
        public string Description { get;  set; }

        /// <summary>
        /// Name of environment variable to use.
        /// </summary>
        public string EnvironmentVariable { get;  set; }

        /// <summary>
        /// Path to configuration value to use.
        /// </summary>
        public string ConfigPath { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        public object DefaultValue { get;  set; }
    }
}