namespace NFlags.Tests.DataTypes
{
    public class NotBooleanFlag
    {
        [Flag("non_bool_flag", "non boolean flag desc", true)]
        public int Flag;        
    }
}