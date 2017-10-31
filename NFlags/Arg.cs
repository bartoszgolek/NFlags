using System;

namespace NFlags 
{
    public class Arg
    {
        public string Name;

        public string Description;

        public Action<string> Action;
    }
}