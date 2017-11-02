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

        public bool HasData() {
            return _current < _array.Length;
        }
    }
}