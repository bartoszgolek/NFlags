using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NFlags.Commands;

namespace NFlags.GenericCommandExtension
{
    public class CommandConfigurator<TArguments>
    {
        private readonly CommandConfigurator _commandConfigurator;

        public CommandConfigurator(CommandConfigurator commandConfigurator)
        {
            _commandConfigurator = commandConfigurator;
        }

        internal Command CreateCommand()
        {
            RegisterArguments();
            return _commandConfigurator.CreateCommand();
        }

        public void SetExecute(Action<TArguments, IOutput> execute)
        {
            _commandConfigurator.SetExecute((args, output) => { execute(BuildGenericType(_commandConfigurator, args), output); });
        }

        private TArguments BuildGenericType(CommandConfigurator commandConfigurator, CommandArgs args)
        {
            var tArgs = (TArguments)Activator.CreateInstance(typeof(TArguments));
            ProcessTArgs(
                (member, flagAttribute) =>
                {
                    SetValue(member, tArgs, args.GetFlag(flagAttribute.Name));
                }, 
                (member, optionAttribute) =>
                {
                    var method = typeof(CommandArgs).GetMethod("GetOption");
                    var generic = method.MakeGenericMethod(GetMemberType(member).GetElementType());
                    var value = generic.Invoke(this, new object[]
                    {
                        optionAttribute.Name
                    });
                    SetValue(member, tArgs, value);
                }, 
                (member, parameterAttribute) =>
                {
                    var method = typeof(CommandArgs).GetMethod("GetParameter");
                    var generic = method.MakeGenericMethod(GetMemberType(member).GetElementType());
                    var value = generic.Invoke(this, new object[]
                    {
                        parameterAttribute.Name
                    });
                    SetValue(member, tArgs, value);
                }, 
                (member, parameterSeriesAttribute) =>
                {
                    var method = typeof(CommandArgs).GetMethod("GetParameterSeries");
                    var memberType = GetMemberType(member);
                    var generic = method.MakeGenericMethod(memberType.GetElementType());
                    var value = generic.Invoke(this, null);
    
                    var itemValues = (IEnumerable)value;
                    var array = Array.CreateInstance(memberType, 1);
                    var i = 0;
                    foreach (var itemValue in itemValues)
                    {
                        array.SetValue(itemValue, i++);                                                                                                                                                
                    }
                        
                    SetValue(member, tArgs, array);
                }
            );

            return tArgs;
        }

        private static void ProcessTArgs(Action<MemberInfo, FlagAttribute> flagAction, Action<MemberInfo, OptionAttribute> optionAction, Action<MemberInfo, ParameterAttribute> parameterAction,
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

        private void RegisterArguments()
        {
            ProcessTArgs(
                (member, flagAttribute) =>
                {
                    _commandConfigurator.RegisterFlag(
                        flagAttribute.Name,
                        flagAttribute.Description,
                        flagAttribute.DefaultValue
                    );                    
                },
                OptionAction,
                (member, parameterAttribute) =>
                {
                    var method = typeof(CommandConfigurator).GetMethod("RegisterParam");
                    var generic = method.MakeGenericMethod(GetMemberType(member));
                    generic.Invoke(this, new object[]
                    {
                        parameterAttribute.Name,
                        parameterAttribute.Description,
                        parameterAttribute.DefaultValue
                    });                    
                },
                (member, parameterSeriesAttribute) =>
                {
                    var method = typeof(CommandConfigurator).GetMethods().Single(m => m.IsGenericMethod && m.Name == "RegisterParameterSeries");
                    var generic = method.MakeGenericMethod(GetMemberType(member).GetElementType());
                    generic.Invoke(this, new object[]
                    {
                        parameterSeriesAttribute.Name,
                        parameterSeriesAttribute.Description
                    });                    
                }
            );
        }

        private void OptionAction(MemberInfo member, OptionAttribute optionAttribute)
        {
            var method = typeof(CommandConfigurator).GetMethods().Single(m => m.IsGenericMethod && m.Name == "RegisterOption");
            var generic = method.MakeGenericMethod(GetMemberType(member));
            generic.Invoke(this, new object[] {optionAttribute.Name, optionAttribute.Description, optionAttribute.DefaultValue});
        }

        private Type GetMemberType(MemberInfo member)
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

        private void SetValue(MemberInfo member, TArguments instance, object value)
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