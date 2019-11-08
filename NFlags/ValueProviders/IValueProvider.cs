namespace NFlags.ValueProviders
{
    internal interface IValueProvider
    {
        bool HasValue();
        object ReadValue();
    }
}