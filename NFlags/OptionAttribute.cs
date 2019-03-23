using System;
using System.Runtime.CompilerServices;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Mark field or property as option to register in command
    /// </summary>
    /// <remarks>If property, then has to be settable.</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class OptionAttribute : Attribute
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of OptionAttribute
        /// </summary>
        /// <param name="order">Used to order members in help.</param>
        public OptionAttribute([CallerLineNumber]int order = 0)
        {
            Order = order;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of OptionAttribute
        /// </summary>
        /// <param name="name">Name of option</param>
        /// <param name="abr">Abbreviation of option</param>
        /// <param name="description">Description of option for help</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="order">Used to order members in help.</param>
        public OptionAttribute(string name, string abr, string description, object defaultValue, [CallerLineNumber]int order = 0)
        {
            Name = name;
            Abr = abr;
            Description = description;
            DefaultValue = defaultValue;
            Order = order;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of OptionAttribute
        /// </summary>
        /// <param name="name">Name of option</param>
        /// <param name="description">Description of option for help</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="order">Used to order members in help.</param>
        public OptionAttribute(string name, string description, object defaultValue, [CallerLineNumber]int order = 0)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
            Order = order;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of OptionAttribute
        /// </summary>
        /// <param name="name">Name of option</param>
        /// <param name="abr">Abbreviation of option</param>
        /// <param name="description">Description of option for help</param>
        /// <param name="environmentVariable">Name of environment variable to use.</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="order">Used to order members in help.</param>
        public OptionAttribute(string name, string abr, string description, string environmentVariable, object defaultValue, [CallerLineNumber]int order = 0)
        {
            Name = name;
            Abr = abr;
            Description = description;
            EnvironmentVariable = environmentVariable;
            DefaultValue = defaultValue;
            Order = order;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of OptionAttribute
        /// </summary>
        /// <param name="name">Name of option</param>
        /// <param name="abr">Abbreviation of option</param>
        /// <param name="description">Description of option for help</param>
        /// <param name="environmentVariable">Name of environment variable to use.</param>
        /// <param name="configPath">Path to configuration value to use.</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="order">Used to order members in help.</param>
        public OptionAttribute(string name, string abr, string description, string environmentVariable, string configPath, object defaultValue, [CallerLineNumber]int order = 0)
        {
            Name = name;
            Abr = abr;
            Description = description;
            EnvironmentVariable = environmentVariable;
            ConfigPath = configPath;
            DefaultValue = defaultValue;
            Order = order;
        }

        /// <summary>
        /// Name of option
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Abbreviation of option
        /// </summary>
        public string Abr { get; set; }

        /// <summary>
        /// Description of option for help
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of environment variable to use.
        /// </summary>
        public string EnvironmentVariable { get; set; }

        /// <summary>
        /// Path to configuration value to use.
        /// </summary>
        public string ConfigPath { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        public object DefaultValue { get;  set; }

        internal int Order { get; }
    }
}