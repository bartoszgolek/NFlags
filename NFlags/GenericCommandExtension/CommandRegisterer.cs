using System;
using System.Reflection;
using NFlags.Arguments;
using NFlags.Commands;

namespace NFlags.GenericCommandExtension
{
    internal class CommandRegisterer
    {
        private readonly CommandConfigurator _commandConfigurator;

        public CommandRegisterer(CommandConfigurator commandConfigurator)
        {
            _commandConfigurator = commandConfigurator;
        }

        public void RegisterParameterSeries(MemberInfo member, ParameterSeriesAttribute parameterSeriesAttribute)
        {
            ValidatePropertySetter(member);
            ValidateParameterSeriesMemberType(member);

            const string parameterSeriesRegistrationType = "ParameterSeries";
            RegisterArgumentInstance(
                parameterSeriesRegistrationType, 
                TypeHelper.GetMemberType(member).GetElementType(), 
                new ParameterSeries
                {
                    Name = parameterSeriesAttribute.Name, 
                    Description = parameterSeriesAttribute.Description,
                    ValueType = TypeHelper.GetMemberType(member).GetElementType()
                }
            );
        }

        public void RegisterParameter(MemberInfo member, ParameterAttribute parameterAttribute)
        {
            ValidatePropertySetter(member);

            const string parameterRegistrationType = "Parameter";
            RegisterArgumentInstance(
                parameterRegistrationType, 
                TypeHelper.GetMemberType(member), 
                new Parameter
                {
                    Name = parameterAttribute.Name,
                    Description = parameterAttribute.Description,
                    DefaultValue = parameterAttribute.DefaultValue,
                    EnvironmentVariable = parameterAttribute.EnvironmentVariable,
                    IsEnvironmentVariableLazy = TypeHelper.IsLazy(TypeHelper.GetMemberType(member)),
                    ConfigPath = parameterAttribute.ConfigPath,
                    ValueType = GetArgumentType(TypeHelper.GetMemberType(member))
                }
            );
        }

        public void RegisterFlag(MemberInfo member, FlagAttribute flagAttribute)
        {
            ValidatePropertySetter(member);
            ValidateFlagMemberType(member);
            
            var flag = new Flag
            {
                Name = flagAttribute.Name,
                Abr = flagAttribute.Abr,
                Description = flagAttribute.Description,
                ValueType = typeof(bool),
                DefaultValue = flagAttribute.DefaultValue,
                EnvironmentVariable = flagAttribute.EnvironmentVariable,
                IsEnvironmentVariableLazy = TypeHelper.IsLazy(TypeHelper.GetMemberType(member)),
                ConfigPath = flagAttribute.ConfigPath
            }; 

            _commandConfigurator.RegisterFlagInstance(flag);
        }

        public void RegisterOption(MemberInfo member, OptionAttribute optionAttribute)
        {
            ValidatePropertySetter(member);
            const string optionRegistrationType = "Option";
            RegisterArgumentInstance(
                optionRegistrationType, 
                TypeHelper.GetMemberType(member), 
                new Option
                {
                    Name = optionAttribute.Name,
                    Description = optionAttribute.Description,
                    DefaultValue = optionAttribute.DefaultValue,
                    Abr = optionAttribute.Abr,
                    EnvironmentVariable = optionAttribute.EnvironmentVariable,
                    IsEnvironmentVariableLazy = TypeHelper.IsLazy(TypeHelper.GetMemberType(member)),
                    ConfigPath = optionAttribute.ConfigPath,
                    ValueType = GetArgumentType(TypeHelper.GetMemberType(member))
                }
            );
        }

        private void RegisterArgumentInstance(string registrationType, Type memberType, object value)
        {
            typeof(CommandConfigurator)
                .GetMethod("Register" + registrationType + "Instance", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.MakeGenericMethod(memberType)
                .Invoke(
                    _commandConfigurator,
                    new[] {value});
        }

        private static void ValidatePropertySetter(MemberInfo member)
        {
            var propertyInfo = member as PropertyInfo;
            if (propertyInfo != null && propertyInfo.GetSetMethod() == null)
                throw new PropertyWithoutSetterException(member.Name);
        }

        private static void ValidateParameterSeriesMemberType(MemberInfo member)
        {
            var memberType = TypeHelper.GetMemberType(member);
            if (!memberType.IsArray)
                throw new IncorrectParameterSeriesMemberTypeException(member.Name, memberType);
        }

        private static void ValidateFlagMemberType(MemberInfo member)
        {
            var memberType = TypeHelper.GetMemberType(member);
            var flagType = GetArgumentType(memberType);
            
            if (flagType != typeof(bool))
                throw new IncorrectFlagMemberTypeException(member.Name, memberType);
        }

        private static Type GetArgumentType(Type memberType)
        {
            return TypeHelper.IsLazy(memberType)
                ? memberType.GenericTypeArguments[0]
                : memberType;
        }
    }
}