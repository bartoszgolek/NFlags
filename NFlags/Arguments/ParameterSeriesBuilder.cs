namespace NFlags.Arguments
{
    /// <summary>
    /// Builder for parameter series arguments
    /// </summary>
    /// <typeparam name="T">Type of parameter</typeparam>
    public class ParameterSeriesBuilder<T>
    {
        private string _name;
        private string _description;

        /// <summary>
        /// Set name of the argument.
        /// </summary>
        /// <param name="name">Name of the argument</param>
        /// <returns>Self instance</returns>
        public ParameterSeriesBuilder<T> Name(string name)
        {
            _name = name;

            return this;
        }

        /// <summary>
        /// Set description of the argument.
        /// </summary>
        /// <param name="description">Description of the argument</param>
        /// <returns>Self instance</returns>
        public ParameterSeriesBuilder<T> Description(string description)
        {
            _description = description;

            return this;
        }

        /// <summary>
        /// Build parameter series
        /// </summary>
        /// <returns>Parameter series</returns>
        public ParameterSeries Build()
        {
            return new ParameterSeries
            {
                Name = _name,
                Description = _description,
                ValueType = typeof(T)
            };
        }
    }
}