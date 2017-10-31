namespace NFlags.Tests
{
    public interface IArgMock
    {
        void Option(string value);
        void Param(string value);
        void Flag();
    }
}
