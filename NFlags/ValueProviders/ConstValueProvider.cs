namespace NFlags.ValueProviders
{
    internal class ConstValueProvider : IValueProvider
    {
        private readonly object _value;

        public ConstValueProvider(object value)
        {
            _value = value;
        }

        public T ReadValue<T>()
        {
            return (T)_value;
        }
    }
}