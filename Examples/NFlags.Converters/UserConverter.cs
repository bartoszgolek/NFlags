using System;
using NFlags.TypeConverters;

namespace NFlags.Converters
{
    public class UserConverter : IArgumentConverter
    {
        public bool CanConvert(Type type)
        {
            return typeof(User) == type;
        }

        public object Convert(Type type, string value)
        {
            var strings = value.Split(";");
            if (strings.Length != 3)
                throw new ArgumentValueException(type, value);

            return new User
            {
                UserName = strings[0],
                Name = strings[1],
                Password = strings[2]
            };
        }
    }
}