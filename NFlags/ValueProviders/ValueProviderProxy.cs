using System;

namespace NFlags.ValueProviders
{
    internal class ValueProviderProxy : IValueProvider
    {
        private readonly Func<object> _proxy;

        public ValueProviderProxy(Func<object> proxy)
        {
            _proxy = proxy;
        }

        public T ReadValue<T>()
        {
            return (T)_proxy();
        }
    }
}