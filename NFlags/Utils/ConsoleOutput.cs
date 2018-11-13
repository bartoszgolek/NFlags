using System;

namespace NFlags.Utils
{
    /// <inheritdoc />
    /// <summary>
    /// Output implementation to print to standard output stream.
    /// </summary>
    public class ConsoleOutput : IOutput
    {
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified Boolean value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(bool value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the specified Unicode character value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(char value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the specified array of Unicode characters to the standard output stream.</summary>
        /// <param name="buffer">A Unicode character array.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(char[] buffer)
        {
            Console.Write(buffer);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the specified subarray of Unicode characters to the standard output stream.</summary>
        /// <param name="buffer">An array of Unicode characters.</param>
        /// <param name="index">The starting position in buffer.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index">index</paramref> or <paramref name="count">count</paramref> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="index">index</paramref> plus <paramref name="count">count</paramref> specify a position that is not within <paramref name="buffer">buffer</paramref>.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(char[] buffer, int index, int count)
        {
            Console.Write(buffer, index, count);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified <see cref="T:System.Decimal"></see> value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(decimal value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified double-precision floating-point value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(double value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified 32-bit signed integer value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(int value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified 64-bit signed integer value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(long value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified object to the standard output stream.</summary>
        /// <param name="value">The value to write, or null.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(object value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified single-precision floating-point value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(float value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the specified string value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(string value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified array of objects to the standard output stream using the specified format information.</summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using format.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> or <paramref name="arg">arg</paramref> is null.</exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
        public void Write(string format, params object[] arg)
        {
            Console.Write(format, arg);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified 32-bit unsigned integer value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(uint value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified 64-bit unsigned integer value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void Write(ulong value)
        {
            Console.Write(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the current line terminator to the standard output stream.</summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine()
        {
            Console.WriteLine();
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified Boolean value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(bool value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(char value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the specified array of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="buffer">A Unicode character array.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(char[] buffer)
        {
            Console.WriteLine(buffer);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the specified subarray of Unicode characters, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="buffer">An array of Unicode characters.</param>
        /// <param name="index">The starting position in buffer.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index">index</paramref> or <paramref name="count">count</paramref> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="index">index</paramref> plus <paramref name="count">count</paramref> specify a position that is not within <paramref name="buffer">buffer</paramref>.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(char[] buffer, int index, int count)
        {
            Console.WriteLine(buffer, index, count);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified <see cref="T:System.Decimal"></see> value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(decimal value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified double-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(double value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(int value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(long value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(object value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified single-precision floating-point value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(float value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the specified string value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified array of objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using format.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> or <paramref name="arg">arg</paramref> is null.</exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
        public void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified 32-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(uint value)
        {
            Console.WriteLine(value);
        }
    
        /// <inheritdoc />
        /// <summary>Writes the text representation of the specified 64-bit unsigned integer value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void WriteLine(ulong value)
        {
            Console.WriteLine(value);
        }
    }
}