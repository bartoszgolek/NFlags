using System;
using System.Collections.Generic;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents parsed command arguments.
    /// </summary>
    public class CommandArgs
    {
        private readonly Dictionary<string, object> _options;
        private readonly Dictionary<string, object> _parameters;
        private readonly Dictionary<string, bool> _flags;
        private readonly List<object> _parameterSeries;

        /// <summary>
        /// Creates new instance of CommandArgs
        /// </summary>
        public CommandArgs()
        {
            _flags = new Dictionary<string, bool>();
            _options = new Dictionary<string, object>();
            _parameters = new Dictionary<string, object>();
            _parameterSeries = new List<object>();
        }

        /// <summary>
        /// All registered flags with values
        /// </summary>
        [Obsolete("This property is obsolete and will be removed. Use AddFlag, GetFlag methods instead.")]
        public Dictionary<string, bool> Flags => _flags;

        /// <summary>
        /// All registered options with values
        /// </summary>
        [Obsolete("This property is obsolete and will be removed. Use AddOption, GetOption methods instead.")]
        public Dictionary<string, object> Options => _options;

        /// <summary>
        /// All registered parameters with values
        /// </summary>
        [Obsolete("This property is obsolete and will be removed. Use AddParameter, GetParameter methods instead.")]
        public Dictionary<string, object> Parameters => _parameters;

        /// <summary>
        /// Registered parameter series with values
        /// </summary>
        [Obsolete("This property is obsolete and will be removed. Use AddParameterToSeries, GetParameterFromSeries, GetParameterSeries methods instead.")]
        public List<object> ParameterSeries => _parameterSeries;

        /// <summary>
        /// Add flag
        /// </summary>
        /// <param name="name">Flag name</param>
        /// <param name="value">Flag value</param>
        public void AddFlag(string name, bool value)
        {
            _flags[name] = value;
        }

        /// <summary>
        /// Returns flag value;
        /// </summary>
        /// <param name="name">Flag name</param>
        /// <returns>Flag value</returns>
        public bool GetFlag(string name)
        {
            return _flags[name];
        }

        /// <summary>
        /// Add option
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="value">Option value</param>
        /// <typeparam name="T">Option value type</typeparam>
        public void AddOption<T>(string name, T value)
        {
            _options[name] = value;
        }

        /// <summary>
        /// Returns option value;
        /// </summary>
        /// <param name="name">Option name</param>
        /// <typeparam name="T">Type of option value</typeparam>
        /// <returns>Option value</returns>
        public T GetOption<T>(string name)
        {
            return (T)_options[name];
        }

        /// <summary>
        /// Add parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <typeparam name="T">Parameter value type</typeparam>
        public void AddParameter<T>(string name, T value)
        {
            _parameters[name] = value;
        }

        /// <summary>
        /// Returns parameter value;
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <typeparam name="T">Type of parameter value</typeparam>
        /// <returns>Parameter value</returns>
        public T GetParameter<T>(string name)
        {
            return (T)_parameters[name];
        }

        /// <summary>
        /// Add parameter to parameters series
        /// </summary>
        /// <param name="value">Parameter value</param>
        /// <typeparam name="T">Type of parameter</typeparam>
        public void AddParameterToSeries<T>(T value)
        {
            _parameterSeries.Add(value);
        }

        /// <summary>
        /// Returns parameter of index 'index' from parameter series.
        /// </summary>
        /// <param name="index">Index of parameter series</param>
        /// <typeparam name="T">Type of parameter value</typeparam>
        /// <returns>Value of parameter under 'index'</returns>
        public T GetParameterFromSeries<T>(int index)
        {
            return (T)_parameterSeries[index];
        }

        /// <summary>
        /// Iterator over all parameters in series
        /// </summary>
        /// <typeparam name="T">Type of parameter</typeparam>
        /// <returns>Parameter series enumerable</returns>
        public IEnumerable<T> GetParameterSeries<T>()
        {
            foreach (var parameter in _parameterSeries)
            {
                yield return (T) parameter;
            }
        }
    }
}