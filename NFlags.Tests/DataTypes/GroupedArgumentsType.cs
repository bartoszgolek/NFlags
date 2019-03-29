namespace NFlags.Tests.DataTypes
{
    public class GroupedArgumentsType
    {
        [Flag(Name="flag1", Group="group1")]
        public bool Flag1 { get; set; }

        [Flag(Name="flag2")]
        public bool Flag2 { get; set; }

        [Flag(Name="flag3", Group="group2")]
        public bool Flag3 { get; set; }

        [Flag(Name="flag4")]
        public bool Flag4 { get; set; }

        [Flag(Name="flag6", Group="group3")]
        public bool Flag6 { get; set; }

        [Flag(Name="flag5", Group="group1")]
        public bool Flag5 { get; set; }

        [Flag(Name="flag7")]
        public bool Flag7 { get; set; }

        [Option(Name="option1", Group="group1")]
        public int Option1 { get; set; }

        [Option(Name="option2")]
        public int Option2 { get; set; }

        [Option(Name="option3", Group="group1")]
        public int Option3 { get; set; }

        [Option(Name="option4")]
        public int Option4 { get; set; }

        [Option(Name="option5", Group="group2")]
        public int Option5 { get; set; }

        [Option(Name="option6", Group="group3")]
        public int Option6 { get; set; }

        [Option(Name="option7")]
        public int Option7 { get; set; }

        [Parameter(Name="param1")]
        public string Param1 { get; set; }

        [Parameter(Name="param2")]
        public string Param2 { get; set; }

        [Parameter(Name="param3")]
        public string Param3 { get; set; }

    }
}