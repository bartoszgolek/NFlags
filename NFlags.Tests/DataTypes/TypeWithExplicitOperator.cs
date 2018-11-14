namespace NFlags.Tests.DataTypes
{
    public class TypeWithExplicitOperator
    {
        public static explicit operator TypeWithExplicitOperator(string s)
        {
            return new TypeWithExplicitOperator();
        }
        
    }
}