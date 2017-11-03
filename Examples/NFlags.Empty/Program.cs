namespace NFlags.Empty
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            NFlags.Configure(c => { }).Root(c => { }).Run(args);
        }
    }
}