namespace NFlags.Utils
{
    /// <summary>
    /// Provides options for DefaultHelpPrinter
    /// </summary>
    public class DefaultHelpPrinterOptions
    {
        /// <summary>
        /// Determines if default value should be added to parameter/option description.
        /// </summary>
        public bool PrintDefaultValues { get; set; } = true;

        /// <summary>
        /// Formatting string used when printing default value.
        /// </summary>
        public string DefaultValueText { get; set; } = "Default: {0}";

        /// <summary>
        /// Determines if bound environment variable should be added to parameter/option description.
        /// </summary>
        public bool PrintEnvironmentBindings { get; set; } = true;

        /// <summary>
        /// Formatting string used when printing environment binding.
        /// </summary>
        public string EnvironmentBindingsText { get; set; } = "Environment variable: {0}";

        /// <summary>
        /// Determines if bound config path should be added to parameter/option description.
        /// </summary>
        public bool PrintConfigurationBindings { get; set; } = true;

        /// <summary>
        /// Formatting string used when printing config path binding.
        /// </summary>
        public string ConfigurationBindingsText { get; set; } = "Config path: {0}";
    }
}