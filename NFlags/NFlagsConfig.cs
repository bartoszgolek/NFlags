namespace NFlags
{
    /// <summary>
    /// Represents configuration of NFlags.
    /// </summary>
    public class NFlagsConfig
    {
        /// <summary>
        /// Creates new instance of NFlags config
        /// </summary>
        /// <param name="name">Application name</param>
        /// <param name="description">Application description</param>
        /// <param name="dialect">NSpec arguments dialect</param>
        /// <param name="output"></param>
        public NFlagsConfig(string name, string description, Dialect dialect, IOutput output)
        {
            Name = name;
            Description = description;
            Dialect = dialect;
            Output = output;
        }

        /// <summary>
        /// Application name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Application description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// NFlags arguments dialect
        /// </summary>
        public Dialect Dialect { get; }

        /// <summary>
        /// NFlags output handler. 
        /// </summary>
        public IOutput Output { get; }
    }
}