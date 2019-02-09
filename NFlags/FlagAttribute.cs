using System;

namespace NFlags
{
    /// <inheritdoc />
    /// <summary>
    /// Mark field or property as flag to register in command
    /// </summary>
    /// <remarks>If property, then has to be settable.</remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FlagAttribute : Attribute
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of FlagAttribute
        /// </summary>
        public FlagAttribute()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of FlagAttribute
        /// </summary>
        /// <param name="name">Name of flag</param>
        /// <param name="abr">Abbreviation of flag</param>
        /// <param name="description">Description of flag for help.</param>
        /// <param name="defaultValue">Default value</param>
        public FlagAttribute(string name, string abr, string description, bool defaultValue)
        {
            Name = name;
            Abr = abr;
            Description = description;
            DefaultValue = defaultValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of FlagAttribute
        /// </summary>
        /// <param name="name">Name of flag</param>
        /// <param name="description">Description of flag for help</param>
        /// <param name="defaultValue">Default value</param>
        public FlagAttribute(string name, string description, bool defaultValue)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of FlagAttribute
        /// </summary>
        /// <param name="name">Name of flag</param>
        /// <param name="abr">Abbreviation of flag</param>
        /// <param name="description">Description of flag for help.</param>
        /// <param name="environmentVariable">Name of environment variable to use.</param>
        /// <param name="defaultValue">Default value</param>
        public FlagAttribute(string name, string abr, string description, string environmentVariable, bool defaultValue)
        {
            Name = name;
            Abr = abr;
            Description = description;
            EnvironmentVariable = environmentVariable;
            DefaultValue = defaultValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of FlagAttribute
        /// </summary>
        /// <param name="name">Name of flag</param>
        /// <param name="abr">Abbreviation of flag</param>
        /// <param name="description">Description of flag for help.</param>
        /// <param name="environmentVariable">Name of environment variable to use.</param>
        /// <param name="configPath">Path to configuration value to use.</param>
        /// <param name="defaultValue">Default value</param>
        public FlagAttribute(string name, string abr, string description, string environmentVariable, string configPath, bool defaultValue)
        {
            Name = name;
            Abr = abr;
            Description = description;
            EnvironmentVariable = environmentVariable;
            ConfigPath = configPath;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Name of flag
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// Abbreviation of flag
        /// </summary>
        public string Abr { get;  set; }

        /// <summary>
        /// Description of flag for help
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
        public bool DefaultValue { get;  set; }
    }
}