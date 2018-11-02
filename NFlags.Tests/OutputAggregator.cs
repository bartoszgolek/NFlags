using System.Text;

namespace NFlags.Tests
{
    public class OutputAggregator : IOutput
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void Write(bool value)
        {
            throw new System.NotImplementedException();
        }

        public void Write(char value)
        {
            throw new System.NotImplementedException();
        }

        public void Write(char[] buffer)
        {
            throw new System.NotImplementedException();
        }

        public void Write(char[] buffer, int index, int count)
        {
            throw new System.NotImplementedException();
        }

        public void Write(decimal value)
        {
            throw new System.NotImplementedException();
        }

        public void Write(double value)
        {
            throw new System.NotImplementedException();
        }

        public void Write(int value)
        {
            throw new System.NotImplementedException();
        }

        public void Write(long value)
        {
            throw new System.NotImplementedException();
        }

        public void Write(object value)
        {
            throw new System.NotImplementedException();
        }

        public void Write(float value)
        {
            throw new System.NotImplementedException();
        }

        public void Write(string value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(string format, params object[] arg)
        {
            throw new System.NotImplementedException();
        }

        public void Write(uint value)
        {
            throw new System.NotImplementedException();
        }

        public void Write(ulong value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine()
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(bool value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(char value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(char[] buffer)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(char[] buffer, int index, int count)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(decimal value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(double value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(int value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(long value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(object value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(float value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(string value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(string format, params object[] arg)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(uint value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(ulong value)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}