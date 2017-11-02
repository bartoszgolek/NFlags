namespace NFlags
{
    /// <summary>
    /// Defines how value follows option label.
    /// </summary>
    public enum OptionValueMode
    {
        /// <summary>
        /// Option label is followed by value after equal char.
        /// </summary>
        AfterEqual,
        /// <summary>
        /// Option label is followed by value as next argument.
        /// </summary>
        NextArgument
    }

}