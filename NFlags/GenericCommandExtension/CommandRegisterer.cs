using System.Linq;
using System.Reflection;
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

            var method = typeof(CommandConfigurator).GetMethods().Single(m => m.IsGenericMethod && m.Name == "RegisterParameterSeries");
            var generic = method.MakeGenericMethod(TypeHelper.GetMemberType(member).GetElementType());
            generic.Invoke(_commandConfigurator, new object[] {parameterSeriesAttribute.Name, parameterSeriesAttribute.Description});
        }

        public void RegisterParameter(MemberInfo member, ParameterAttribute parameterAttribute)
        {
            ValidatePropertySetter(member);
                
            var method = typeof(CommandConfigurator).GetMethod("RegisterParam");
            var generic = method.MakeGenericMethod(TypeHelper.GetMemberType(member));
            generic.Invoke(_commandConfigurator, new[] {parameterAttribute.Name, parameterAttribute.Description, parameterAttribute.DefaultValue});
        }

        public void RegisterFlag(MemberInfo member, FlagAttribute flagAttribute)
        {
            ValidatePropertySetter(member);
            ValidateFlagMemberType(member);

            if (flagAttribute.Abr != null)
                _commandConfigurator.RegisterFlag(flagAttribute.Name, flagAttribute.Abr, flagAttribute.Description, flagAttribute.DefaultValue);
            else
                _commandConfigurator.RegisterFlag(flagAttribute.Name, flagAttribute.Description, flagAttribute.DefaultValue);
        }

        public void RegisterOption(MemberInfo member, OptionAttribute optionAttribute)
        {
            ValidatePropertySetter(member);
            
            var parametersCount = optionAttribute.Abr != null ? 4 : 3;
            var method = typeof(CommandConfigurator).GetMethods()
                .Single(info => info.Name == "RegisterOption" && info.IsGenericMethod && info.GetParameters().Length == parametersCount);

            var parameters = new[]
            {
                optionAttribute.Name,
                optionAttribute.Description,
                optionAttribute.DefaultValue
            };

            if (optionAttribute.Abr != null)
            {
                parameters = new[]
                {
                    optionAttribute.Name,
                    optionAttribute.Abr,
                    optionAttribute.Description,
                    optionAttribute.DefaultValue
                };
            }

            var generic = method.MakeGenericMethod(TypeHelper.GetMemberType(member));
            generic.Invoke(_commandConfigurator, parameters);
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
            if (memberType != typeof(bool))
                throw new IncorrectFlagMemberTypeException(member.Name, memberType);
        }
    }
}