using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NFlags.Commands;

namespace NFlags.GenericCommandExtension
{
    internal class GenericArgsBuilder<TArguments>
    {
        private readonly CommandArgs _args;
        private readonly TArguments _tArgs;

        public GenericArgsBuilder(CommandArgs args, TArguments tArgs)
        {
            _args = args;
            _tArgs = tArgs;
        }

        public void SetOptionValue(MemberInfo member, OptionAttribute optionAttribute)
        {
            var memberType = TypeHelper.GetMemberType(member);
            var method = typeof(CommandArgs).GetMethod("GetOption");

            if (TypeHelper.IsLazy(memberType))
            {
                var activator = GetType().GetMethod("ActivateLazy", BindingFlags.NonPublic | BindingFlags.Instance);
                var activate = activator.MakeGenericMethod(memberType.GenericTypeArguments[0]);

                var generic = method.MakeGenericMethod(memberType.GenericTypeArguments[0]);
                var lazyValue = activate.Invoke(this, new object[] { generic, new object[] { optionAttribute.Name }});
                TypeHelper.SetValue(member, _tArgs, lazyValue);
            }
            else
            {
                var generic = method.MakeGenericMethod(memberType);
                var value = generic.Invoke(_args, new object[] {optionAttribute.Name});
                TypeHelper.SetValue(member, _tArgs, value);                
            }
        }

        public void SetFlagValue(MemberInfo member, FlagAttribute flagAttribute)
        {
            var memberType = TypeHelper.GetMemberType(member);
            if (TypeHelper.IsLazy(memberType))
                TypeHelper.SetValue(member, _tArgs, new Lazy<bool>(() => _args.GetFlag(flagAttribute.Name)));
            else
                TypeHelper.SetValue(member, _tArgs, _args.GetFlag(flagAttribute.Name));
        }

        public void SetParameterValue(MemberInfo member, ParameterAttribute parameterAttribute)
        {
            var memberType = TypeHelper.GetMemberType(member);
            var method = typeof(CommandArgs).GetMethod("GetParameter");

            if (TypeHelper.IsLazy(memberType))
            {
                var activator = GetType().GetMethod("ActivateLazy", BindingFlags.NonPublic | BindingFlags.Instance);
                var activate = activator.MakeGenericMethod(memberType.GenericTypeArguments[0]);

                var generic = method.MakeGenericMethod(memberType.GenericTypeArguments[0]);
                var lazyValue = activate.Invoke(this, new object[] { generic, new object[] { parameterAttribute.Name }});
                TypeHelper.SetValue(member, _tArgs, lazyValue);
            }
            else
            {
                var generic = method.MakeGenericMethod(TypeHelper.GetMemberType(member));
                var value = generic.Invoke(_args, new object[] {parameterAttribute.Name});
                TypeHelper.SetValue(member, _tArgs, value);                
            }
        }

        private Lazy<T> ActivateLazy<T>(MethodInfo method, params object[] args)
        {
            return new Lazy<T>(() => (T)method.Invoke(_args, args));
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