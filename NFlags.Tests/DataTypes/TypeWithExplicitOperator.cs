namespace NFlags.Tests.DataTypes
{
    public class TypeWithExplicitOperator
    {
        public string S;

        public static explicit operator TypeWithExplicitOperator(string s)
        {
            return new TypeWithExplicitOperator
            {
                S = s
            };
        }
        
    }
}