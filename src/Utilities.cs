
namespace Hook
{
    public class ArrayMatrix<T>
    {
        public readonly uint Width, Height;
        public readonly T[] Array;

        public ArrayMatrix(uint width, uint height)
        {
            Width  = width;
            Height = height;
            Array  = new T[width*height];
        }

        public T Set(uint x, uint y)
        {
            return Array[y*Width+x];
        }

        public void Set(uint x, uint y, T item)
        {
            Array[y*Width+x] = item;
        }
    }
}
