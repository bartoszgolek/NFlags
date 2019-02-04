using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NFlags.Commands;

namespace NFlags.GenericCommandExtension
{
    internal class GenericArgsBuilder<TArguments>
    {
        private readonly CommandArgs _args;
        private readonly TArguments _tArgs;
        private readonly ICollection<string> _flagsSet = new HashSet<string>();
        private readonly ICollection<string> _optionsSet = new HashSet<string>();

        public GenericArgsBuilder(CommandArgs args, TArguments tArgs)
        {
            _args = args;
            _tArgs = tArgs;
        }

        public void SetOptionValue(MemberInfo member, OptionAttribute optionAttribute)
        {
            if (_optionsSet.Contains(member.Name))
                return;

            var method = typeof(CommandArgs).GetMethod("GetOption");
            var generic = method.MakeGenericMethod(TypeHelper.GetMemberType(member));
            var value = generic.Invoke(_args, new object[] {optionAttribute.Name});
            TypeHelper.SetValue(member, _tArgs, value);
            _optionsSet.Add(member.Name);
        }

        public void SetFlagValue(MemberInfo member, FlagAttribute flagAttribute)
        {
            if (_flagsSet.Contains(member.Name))
                return;

            TypeHelper.SetValue(member, _tArgs, _args.GetFlag(flagAttribute.Name));
            _flagsSet.Add(member.Name);
        }

        public void SetParameterValue(MemberInfo member, ParameterAttribute parameterAttribute)
        {
            var method = typeof(CommandArgs).GetMethod("GetParameter");
            var generic = method.MakeGenericMethod(TypeHelper.GetMemberType(member));
            var value = generic.Invoke(_args, new object[] {parameterAttribute.Name});
            TypeHelper.SetValue(member, _tArgs, value);
        }

        public void SetParameterSeriesValues(MemberInfo member, ParameterSeriesAttribute parameterSeriesAttribute)
        {
            var method = typeof(CommandArgs).GetMethod("GetParameterSeries");
            var memberType = TypeHelper.GetMemberType(member);
            var generic = method.MakeGenericMethod(memberType.GetElementType());
            var value = generic.Invoke(_args, null);

            var itemValues = (IEnumerable) value;
            var length = itemValues.Cast<object>().Count();

            var array = Array.CreateInstance(memberType.GetElementType(), length);
            var i = 0;
            foreach (var itemValue in itemValues)
                array.SetValue(itemValue, i++);

            TypeHelper.SetValue(member, _tArgs, array);
        }
    }
}