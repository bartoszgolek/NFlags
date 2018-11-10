using System;
using System.Linq;
using System.Reflection;

namespace NFlags.GenericCommandExtension
{
    internal class ProcessTArgsTemplate
    {
        public static void Process<TArguments>(Action<MemberInfo, FlagAttribute> flagAction, Action<MemberInfo, OptionAttribute> optionAction, Action<MemberInfo, ParameterAttribute> parameterAction,
            Action<MemberInfo, ParameterSeriesAttribute> parameterSeriesAction)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            var type = typeof(TArguments);
            var members = type.GetFields(bindingFlags).Cast<MemberInfo>()
                .Concat(type.GetProperties(bindingFlags)).ToArray();

            foreach (var member in members)
            {
                var flagAttribute = member.GetCustomAttribute<FlagAttribute>();
                if (flagAttribute != null)
                {
                    flagAction(member, flagAttribute);
                    continue;
                }


                var optionAttribute = member.GetCustomAttribute<OptionAttribute>();
                if (optionAttribute != null)
                {
                    optionAction(member, optionAttribute);
                    continue;
                }


                var parameterAttribute = member.GetCustomAttribute<ParameterAttribute>();
                if (parameterAttribute != null)
                {
                    parameterAction(member, parameterAttribute);
                    continue;
                }


                var parameterSeriesAttribute = member.GetCustomAttribute<ParameterSeriesAttribute>();
                if (parameterSeriesAttribute != null)
                {
                    parameterSeriesAction(member, parameterSeriesAttribute);
                }
            }
        }

    }
}