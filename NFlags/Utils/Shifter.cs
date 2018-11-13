using System;

namespace NFlags.Utils
{
    internal class Shifter<T> {
        private readonly T[] _array;

        private int _current;

        public Shifter(T[] array) {
            _array = array;
        }

        public T Shift() {
            return _array[_current++];
        }

        public T ShiftBack() {
            return _array[--_current];
        }

        public bool HasData() {
            return _current < _array.Length;
        }

        public T[] ToArray()
        {
            var copyLength = _array.Length - _current;
            var destination = new T[copyLength];
            Array.Copy(_array, _current, destination, 0, copyLength);

            return destination;
        }
    }
}