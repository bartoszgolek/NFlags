using System;

namespace NFlags
{
    public class Option
    {
        public string Name;

        public string Abr;

        public string Description;

        public Action<string> Action;
    }
}