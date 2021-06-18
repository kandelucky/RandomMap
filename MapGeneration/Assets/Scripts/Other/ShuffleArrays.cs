using System.Collections.Generic;
using UnityEngine;


// code by https://github.com/herbou/Unity_ShuffleArraysAndLists
public static class ShuffleArrays
{
    //shuffle arrays:
    public static void Shuffle<T>(this T[] array, int shuffleAccuracy)
    {
        for (int i = 0; i < shuffleAccuracy; i++)
        {
            int randomIndex = Random.Range(1, array.Length);

            T temp = array[randomIndex];
            array[randomIndex] = array[0];
            array[0] = temp;
        }
    }
}
