using System;
using System.Reflection;

namespace NFlags.GenericCommandExtension
{
    internal static class TypeHelper
    {
        public static Type GetMemberType(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                        "Input MemberInfo must be if type FieldInfo, PropertyInfo"
                    );
            }
        }

        public static void SetValue<TArguments>(MemberInfo member, TArguments instance, object value)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo)member).SetValue(instance, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo) member).SetValue(instance, value);
                    break;
                default:
                    throw new ArgumentException
                    (
                        "Input MemberInfo must be if type FieldInfo, PropertyInfo"
                    );
            }
        }
    }
}