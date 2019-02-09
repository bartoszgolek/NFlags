namespace NFlags.Arguments
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for arguments with default values
    /// </summary>
    public abstract class DefaultValueArgument : Argument
    {
        /// <summary>
        /// Default value
        /// </summary>
        public object DefaultValue;

        /// <summary>
        /// Name of environment variable to use before defaultValue when argument is not passed.
        /// </summary>
        public string EnvironmentVariable { get; internal set; }

        /// <summary>
        /// Path to configuration value
        /// </summary>
        public string ConfigPath { get; set; }
    }
}