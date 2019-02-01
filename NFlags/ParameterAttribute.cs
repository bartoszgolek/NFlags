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
        /// <param name="name">Name of parameter</param>
        /// <param name="description">Description of parameter for help</param>
        /// <param name="defaultValue">Default value if parameter is not passed trough arguments</param>
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
        /// <param name="environmentVariable">Name of environment variable to use before defaultValue when argument is not passed.</param>
        /// <param name="defaultValue">Default value if parameter is not passed trough arguments</param>
        public ParameterAttribute(string name, string description, string environmentVariable, object defaultValue)
        {
            Name = name;
            Description = description;
            EnvironmentVariable = environmentVariable;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Name of parameter
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Description of parameter for help
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Name of environment variable to use before defaultValue when argument is not passed.
        /// </summary>
        public string EnvironmentVariable { get; }

        /// <summary>
        /// Default value if parameter is not passed trough arguments
        /// </summary>
        public object DefaultValue { get; }
    }
}