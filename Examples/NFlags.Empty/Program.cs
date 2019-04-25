namespace NFlags.Empty
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return Cli.Configure(c => { }).Root(c => { }).Run(args);
        }
    }
}