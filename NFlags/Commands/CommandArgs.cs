using System;
using System.Collections.Generic;
using NFlags.ValueProviders;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents parsed command arguments.
    /// </summary>
    public class CommandArgs
    {
        private readonly Dictionary<string, ValueProvidersCollection> _options;
        private readonly Dictionary<string, ValueProvidersCollection> _parameters;
        private readonly Dictionary<string, ValueProvidersCollection> _flags;
        private readonly List<object> _parameterSeries;

        /// <summary>
        /// Creates new instance of CommandArgs
        /// </summary>
        public CommandArgs()
        {
            _flags = new Dictionary<string, ValueProvidersCollection>();
            _options = new Dictionary<string, ValueProvidersCollection>();
            _parameters = new Dictionary<string, ValueProvidersCollection>();
            _parameterSeries = new List<object>();
        }

        /// <summary>
        /// Add flag
        /// </summary>
        /// <param name="name">Flag name</param>
        /// <param name="value">Flag value</param>
        public void AddFlag(string name, bool value)
        {
            AddFlagValueProvider(name, new ConstValueProvider(value));
        }
        
        internal void AddFlagValueProvider(string name, IValueProvider valueProvider)
        {
            if (!_flags.ContainsKey(name))
                _flags.Add(name, new ValueProvidersCollection());
            
            _flags[name].RegisterValueProvider(valueProvider);
        }

        /// <summary>
        /// Returns flag value;
        /// </summary>
        /// <param name="name">Flag name</param>
        /// <returns>Flag value</returns>
        public bool GetFlag(string name)
        {
            return (bool) _flags[name].GetValue();
        }

        /// <summary>
        /// Add option
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="value">Option value</param>
        /// <typeparam name="T">Option value type</typeparam>
        public void AddOption<T>(string name, T value)
        {
            AddOptionValueProvider(name, new ConstValueProvider(value));
        }

        internal void AddOptionValueProvider(string name, IValueProvider valueProvider)
        {
            if (!_options.ContainsKey(name))
                _options.Add(name, new ValueProvidersCollection());
            
            _options[name].RegisterValueProvider(valueProvider);
        }

        /// <summary>
        /// Returns option value;
        /// </summary>
        /// <param name="name">Option name</param>
        /// <typeparam name="T">Type of option value</typeparam>
        /// <returns>Option value</returns>
        public T GetOption<T>(string name)
        {
            return (T)_options[name].GetValue();
        }

        /// <summary>
        /// Add parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <typeparam name="T">Parameter value type</typeparam>
        public void AddParameter<T>(string name, T value)
        {
            AddParameterValueProvider(name, new ConstValueProvider(value));
        }

        internal void AddParameterValueProvider(string name, IValueProvider valueProvider)
        {
            if (!_parameters.ContainsKey(name))
                _parameters.Add(name, new ValueProvidersCollection());
            
            _parameters[name].RegisterValueProvider(valueProvider);
        }

        /// <summary>
        /// Returns parameter value;
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <typeparam name="T">Type of parameter value</typeparam>
        /// <returns>Parameter value</returns>
        public T GetParameter<T>(string name)
        {
            return (T)_parameters[name].GetValue();
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