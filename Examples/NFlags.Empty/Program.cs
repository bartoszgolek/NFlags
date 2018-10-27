namespace NFlags.Empty
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return NFlags.Configure(c => { }).Root(c => { }).Run(args);
        }
    }
}