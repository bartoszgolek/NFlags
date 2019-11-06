using System;

namespace NFlags.Utils
{
    internal class ArrayReader<T>    
    {
        private readonly T[] _array;

        private int _current;

        public ArrayReader(T[] array) {
            _array = array;
        }

        public bool HasData() {
            return _current < _array.Length;
        }

        public T Current() {
            return _array[_current];            
        }

        public T Read() {
            return _array[_current++];
        }

        public T[] ReadToEnd()
        {
            var copyLength = _array.Length - _current;
            var destination = new T[copyLength];
            Array.Copy(_array, _current, destination, 0, copyLength);

            return destination;
        }
    }
}