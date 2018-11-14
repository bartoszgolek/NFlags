using System;
using System.Reflection;

namespace NFlags.TypeConverters
{
    /// <inheritdoc />
    /// <summary>
    /// Convert string to type using implicit operator.
    /// </summary>
    public class ImplicitOperatorConverter : IArgumentConverter
    {
        private const string ImplicitOperatorMethodName = "op_Implicit";

        /// <inheritdoc />
        public bool CanConvert(Type type)
        {
            return GetImplicitOperatorInfo(type) != null;
        }

        /// <inheritdoc />
        public object Convert(Type type, string value)
        {
            var implicitOperatorInfo = GetImplicitOperatorInfo(type);
            if (implicitOperatorInfo != null)
                return implicitOperatorInfo.Invoke(null, new object[] {value});
            
            throw new ArgumentValueException(type, value);
        }

        private static MethodInfo GetImplicitOperatorInfo(IReflect type)
        {
            return type.GetMethod(
                ImplicitOperatorMethodName,
                BindingFlags.Static | BindingFlags.Public,
                null,
                new []{ typeof(string) }, 
                new ParameterModifier[0]
            );
        }
    }
}