using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePatterns : MonoBehaviour {

    List<Tuple<int, float, int>> pattern_1 = new List<Tuple<int, float, int>>();
    int numEnemies;

    public GamePatterns()
    {
        numEnemies = 200;
        var time = 0f;
        int[] fibonacciNumbers = fibonacci();

        for (var i = 0; i < numEnemies; i++)
        {
            if (System.Array.IndexOf(fibonacciNumbers, i + 1) != -1)
                time += 65f;
            else
                time += 18f;

            var slot = (int)Random.Range(0, 6);
            if (slot != 0)
                pattern_1.Add(new Tuple<int, float, int>(slot, time, 0));
            else
                i--;
        }
    }

    public List<Tuple<int, float, int>> getPattern()
    {
        return this.pattern_1;
    }

    public int[] fibonacci()
    {
        var f1 = 1;
        var f2 = 1;
        int[] fibo = new int[30];
        fibo[0] = f1;
        fibo[1] = f1;
        for (var i = 2; i < fibo.Length; i++)
        {
            fibo[i] = f1 + f2;
            f1 = f2;
            f2 = fibo[i];
        }
        return fibo;
    }

    public class Tuple<T1, T2, T3>
    {
        public T1 Line { get; private set; }
        public T2 Time { get; private set; }
        public T3 Enemy { get; private set; }
        internal Tuple(T1 line, T2 time, T3 enemy)
        {
            Line = line;
            Time = time;
            Enemy = enemy;
        }
    }

}
