using System;

namespace NFlags.ValueProviders
{
    internal class EmptyValueProvider : IValueProvider
    {
        public bool HasValue()
        {
            return false;
        }

        public object ReadValue()
        {
            throw new ValueNotProvidedException();
        }
    }

    internal class ValueNotProvidedException : Exception
    {
    }
}