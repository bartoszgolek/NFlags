using System;

namespace NFlags
{
    /// <summary>
    /// Represents ability to write output.
    /// </summary>
    public interface IOutput
    {
        /// <summary>Writes the text representation of the specified Boolean value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(bool value);

        /// <summary>Writes the specified Unicode character value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(char value);

        /// <summary>Writes the specified array of Unicode characters to the standard output stream.</summary>
        /// <param name="buffer">A Unicode character array.</param>
        void Write(char[] buffer);

        /// <summary>Writes the specified subarray of Unicode characters to the standard output stream.</summary>
        /// <param name="buffer">An array of Unicode characters.</param>
        /// <param name="index">The starting position in buffer.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index">index</paramref> or <paramref name="count">count</paramref> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="index">index</paramref> plus <paramref name="count">count</paramref> specify a position that is not within <paramref name="buffer">buffer</paramref>.</exception>
        void Write(char[] buffer, int index, int count);

        /// <summary>Writes the text representation of the specified <see cref="T:System.Decimal"></see> value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(Decimal value);

        /// <summary>Writes the text representation of the specified double-precision floating-point value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(double value);

        /// <summary>Writes the text representation of the specified 32-bit signed integer value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(int value);

        /// <summary>Writes the text representation of the specified 64-bit signed integer value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(long value);

        /// <summary>Writes the text representation of the specified object to the standard output stream.</summary>
        /// <param name="value">The value to write, or null.</param>
        void Write(object value);

        /// <summary>Writes the text representation of the specified single-precision floating-point value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(float value);

        /// <summary>Writes the specified string value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(string value);

        /// <summary>Writes the text representation of the specified array of objects to the standard output stream using the specified format information.</summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using format.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> or <paramref name="arg">arg</paramref> is null.</exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
        void Write(string format, params object[] arg);

        /// <summary>Writes the text representation of the specified 32-bit unsigned integer value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(uint value);

        /// <summary>Writes the text representation of the specified 64-bit unsigned integer value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void Write(ulong value);

        /// <summary>Writes the current line terminator to the standard output stream.</summary>
        void WriteLine();

        /// <summary>Writes the text representation of the specified Boolean value, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(bool value);

        /// <summary>Writes the specified Unicode character, followed by the current line terminator, value to the standard output stream.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(char value);

        /// <summary>Writes the specified array of Unicode characters, followed by the current line terminator.</summary>
        /// <param name="buffer">A Unicode character array.</param>
        void WriteLine(char[] buffer);

        /// <summary>Writes the specified subarray of Unicode characters, followed by the current line terminator.</summary>
        /// <param name="buffer">An array of Unicode characters.</param>
        /// <param name="index">The starting position in buffer.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer">buffer</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index">index</paramref> or <paramref name="count">count</paramref> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="index">index</paramref> plus <paramref name="count">count</paramref> specify a position that is not within <paramref name="buffer">buffer</paramref>.</exception>
        void WriteLine(char[] buffer, int index, int count);

        /// <summary>Writes the text representation of the specified <see cref="T:System.Decimal"></see> value, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(Decimal value);

        /// <summary>Writes the text representation of the specified double-precision floating-point value, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(double value);

        /// <summary>Writes the text representation of the specified 32-bit signed integer value, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(int value);

        /// <summary>Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(long value);

        /// <summary>Writes the text representation of the specified object, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(object value);

        /// <summary>Writes the text representation of the specified single-precision floating-point value, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(float value);

        /// <summary>Writes the specified string value, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(string value);

        /// <summary>Writes the text representation of the specified array of objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
        /// <summary>Writes the text representation of the specified array of objects, followed by the current line terminator, to the standard output stream using the specified format information.</summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using format.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="format">format</paramref> or <paramref name="arg">arg</paramref> is null.</exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format">format</paramref> is invalid.</exception>
        void WriteLine(string format, params object[] arg);

        /// <summary>Writes the text representation of the specified 32-bit unsigned integer value, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(uint value);

        /// <summary>Writes the text representation of the specified 64-bit unsigned integer value, followed by the current line terminator.</summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(ulong value);
    }
}