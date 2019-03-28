namespace NFlags.ValueProviders
{
    internal interface IValueProvider
    {
        T ReadValue<T>();
    }
}