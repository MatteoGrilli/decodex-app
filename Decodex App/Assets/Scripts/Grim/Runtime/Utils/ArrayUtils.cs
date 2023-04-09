using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Decodex.Utils
{
    public static class ArrayUtils
    {
        public static void Shuffle<T>(T[] array)
        {
        
            int n = array.Length;
            while (n > 1)
            {
                int k = Random.Range(0, n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
