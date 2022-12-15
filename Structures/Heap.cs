using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardMemorizer.Structures
{

    public class MaxHeap<T> where T: IComparable
    {
        T[] values;

        public MaxHeap(int size)
        {
            values = new T[size];
        }

        public T GetValue(int index)
        {
            return values[index];
        }

        public int FindIndex(T value)
        {
            for (int i = 0; i < values.Length; ++i)
            {
                if (values[i].Equals(value))
                    return i;
            }
            return -1;
        }

        public void RotateLeft(int index)
        {
            T parent = GetValue(index);
            T child1 = GetValue((2 * index) + 1);
            T child2 = GetValue((2 * index) + 2);
            values[index] = child2;
            values[(2 * index) + 1] = parent;
            values[(2*((2 * index) + 1))+1] = child1;
        }

        public void Insert(T value)
        {

        }
    }
}
