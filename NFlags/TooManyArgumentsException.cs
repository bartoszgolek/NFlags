using System;

namespace NFlags
{
    public class TooManyArgumentsException : Exception
    {
        public TooManyArgumentsException(string value)
            :base(string.Format("Two many parameters. Can't handle {0} value.", value))
        {
        }
    } 
}