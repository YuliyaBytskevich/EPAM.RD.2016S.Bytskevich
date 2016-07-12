using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage
{
    public static class IdentifiersGenerator
    {

        public static IEnumerator<int> GetNewId()
        {

        }








        //private class Enumerator : IEnumerator<int>
        //{
        //    private int[] elements;
        //    int position = -1;

        //    public Enumerator(int[] elems)
        //    {
        //        elements = elems;
        //    }

        //    private IEnumerator<int> GetEnumerator()
        //    {
        //        return (IEnumerator<int>)this;
        //    }

        //    public bool MoveNext()
        //    {
        //        position++;
        //        return (position < elements.Length);
        //    }

        //    public void Reset()
        //    {
        //        position = -1;
        //    }

        //    public void Dispose()
        //    {
        //        // DO NOTHING
        //    }

        //    public int Current
        //    {
        //        get
        //        {
        //            try
        //            {
        //                return elements[position];
        //            }

        //            catch (IndexOutOfRangeException)
        //            {
        //                throw new InvalidOperationException();
        //            }
        //        }
        //    }
        //    object IEnumerator.Current => Current;
        //}


    }
}
