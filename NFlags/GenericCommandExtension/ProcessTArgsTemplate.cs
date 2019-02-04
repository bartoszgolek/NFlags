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
                var flagRegistered = false;
                var flagAttributes = member.GetCustomAttributes<FlagAttribute>();
                foreach (var flagAttribute in flagAttributes)
                {
                    flagAction(member, flagAttribute);
                    flagRegistered = true;
                }
                if (flagRegistered)
                    continue;


                var optionRegistered = false;
                var optionAttributes = member.GetCustomAttributes<OptionAttribute>();
                foreach (var optionAttribute in optionAttributes)
                {
                    optionRegistered = true;
                    optionAction(member, optionAttribute);
                }
                if (optionRegistered)
                    continue;


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