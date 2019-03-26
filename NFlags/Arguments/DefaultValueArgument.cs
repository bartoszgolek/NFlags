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
        public object DefaultValue { get; internal set; }

        /// <summary>
        /// Name of environment variable to use before defaultValue when argument is not passed.
        /// </summary>
        public string EnvironmentVariable { get; internal set; }

        /// <summary>
        /// Determines, if environment variable should be read during initialization or on each access
        /// </summary>
        public bool IsEnvironmentVariableLazy { get; internal set; }

        /// <summary>
        /// Path to configuration value
        /// </summary>
        public string ConfigPath { get; internal set; }

        /// <summary>
        /// Determines, if config path value should be read during initialization or on each access
        /// </summary>
        public bool IsConfigPathLazy { get; internal set; }

        /// <summary>
        /// Determines if argument require passing value.
        /// </summary>
        public abstract bool RequireValue { get; }
    }
}