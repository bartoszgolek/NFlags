namespace NFlags.Tests.DataTypes
{
    public class TypeWithImplicitOperator
    {
        public string S;

        public static implicit operator TypeWithImplicitOperator(string s)
        {
            return new TypeWithImplicitOperator
            {
                S = s
            };
        }
        
    }
}