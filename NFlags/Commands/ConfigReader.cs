using System.Reflection;
using NFlags.Arguments;

namespace NFlags.Commands
{
    internal class ConfigReader
    {
        private readonly ArgumentValueReader _argumentValueReader;
        private readonly IConfig _config;
        private readonly IGenericConfig _genericConfig;

        public ConfigReader(ArgumentValueReader argumentValueReader, IConfig config, IGenericConfig genericConfig)
        {
            _argumentValueReader = argumentValueReader;
            _config = config;
            _genericConfig = genericConfig;
        }

        public object ReadConfigValue(DefaultValueArgument argument)
        {
            return _argumentValueReader.Read(argument, _config?.Get(argument.ConfigPath));
        }

        public object ReadConfigGenericValue(DefaultValueArgument argument)
        {
            if (_genericConfig == null)
                return null;

            if (!_genericConfig.Has(argument.ConfigPath))
                return null;

            return _genericConfig.GetType()
                .GetMethod("Get", BindingFlags.Public | BindingFlags.Instance)
                ?.MakeGenericMethod(argument.ValueType)
                .Invoke(_genericConfig, new object[] { argument.ConfigPath });
        }
    }
}