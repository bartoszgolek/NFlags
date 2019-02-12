using System.Text; 

namespace NFlags.Tests.TestImplementations
{
    public class OutputAggregator : IOutput
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void Write(bool value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(char value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(char[] buffer)
        {
            _stringBuilder.Append(buffer);
        }

        public void Write(char[] buffer, int index, int count)
        {
            _stringBuilder.Append(buffer, index, count);
        }

        public void Write(decimal value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(double value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(int value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(long value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(object value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(float value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(string value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(string format, params object[] arg)
        {
            _stringBuilder.AppendFormat(format, arg);
        }

        public void Write(uint value)
        {
            _stringBuilder.Append(value);
        }

        public void Write(ulong value)
        {
            _stringBuilder.Append(value);
        }

        public void WriteLine()
        {
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(bool value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(char value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(char[] buffer)
        {
            _stringBuilder.Append(buffer);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(char[] buffer, int index, int count)
        {
            _stringBuilder.Append(buffer, index, count);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(decimal value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(double value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(int value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(long value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(object value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(float value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(string value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(string format, params object[] arg)
        {
            _stringBuilder.AppendFormat(format, arg);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(uint value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public void WriteLine(ulong value)
        {
            _stringBuilder.Append(value);
            _stringBuilder.Append(System.Environment.NewLine);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}