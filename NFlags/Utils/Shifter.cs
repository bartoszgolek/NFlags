namespace NFlags.Utils
{
    internal class Shifter<T> {
        private readonly T[] _array;

        private int _current;

        public Shifter(T[] array) {
            this._array = array;
        }

        public T Shift() {
            return this._array[this._current++];
        }

        public bool HasData() {
            return this._current < this._array.Length;
        }
    }
}