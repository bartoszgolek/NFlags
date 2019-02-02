using System;

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
        public OptionAttribute()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of OptionAttribute
        /// </summary>
        /// <param name="name">Name of option</param>
        /// <param name="abr">Abbreviation of option</param>
        /// <param name="description">Description of option for help</param>
        /// <param name="defaultValue">Default value if option is not passed trough arguments</param>
        public OptionAttribute(string name, string abr, string description, object defaultValue)
        {
            Name = name;
            Abr = abr;
            Description = description;
            DefaultValue = defaultValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of OptionAttribute
        /// </summary>
        /// <param name="name">Name of option</param>
        /// <param name="abr">Abbreviation of option</param>
        /// <param name="description">Description of option for help</param>
        /// <param name="environmentVariable">Name of environment variable to use before defaultValue when argument is not passed.</param>
        /// <param name="defaultValue">Default value if option is not passed trough arguments</param>
        public OptionAttribute(string name, string abr, string description, string environmentVariable, object defaultValue)
        {
            Name = name;
            Abr = abr;
            Description = description;
            EnvironmentVariable = environmentVariable;
            DefaultValue = defaultValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates new instance of OptionAttribute
        /// </summary>
        /// <param name="name">Name of option</param>
        /// <param name="description">Description of option for help</param>
        /// <param name="defaultValue">Default value if option is not passed trough arguments</param>
        public OptionAttribute(string name, string description, object defaultValue)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
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
        /// Name of environment variable to use before defaultValue when argument is not passed.
        /// </summary>
        public string EnvironmentVariable { get; set; }

        /// <summary>
        /// Default value if option is not passed trough arguments
        /// </summary>
        public object DefaultValue { get;  set; }
    }
}