namespace NFlags.ValueProviders
{
    public class ConstValueProvider : IValueProvider
    {
        private object _value;

        public ConstValueProvider(object value)
        {
            _value = value;
        }

        public object readValue()
        {
            return _value;
        }
    }
}