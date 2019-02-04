namespace NFlags.GenericAliases
{
    public class CommandArguments
    {
        [Option("option", "o", "option description", 3)]
        [Option("option-alias", "oa", "option description", 3)]
        public int Option { get; set; }

        [Flag("flag", "f", "flag description", false)]
        [Flag("flag-alias", "fa", "flag description", false)]
        public bool Flag { get; set; }
    }
}