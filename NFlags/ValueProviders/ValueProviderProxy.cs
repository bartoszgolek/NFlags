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

        public bool HasValue()
        {
            return true;
        }

        public object ReadValue()
        {
            return _proxy();
        }
    }
}