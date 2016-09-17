using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GamePatterns {

    List<Tuple<int, float, int>> pattern = new List<Tuple<int, float, int>>();

    private int _numEnemies;
    public int numEnemies
    {
        get { return _numEnemies; }
        set { _numEnemies = value;  }
    }
    private int _threeStarsScore;
    public int threeStarsScore{
        get { return _threeStarsScore; }
        set { _threeStarsScore = value; }
    }

    private string fileName;  
    private StreamWriter writer;
    private StreamReader reader;

    FileInfo file;

    public GamePatterns(int world, int level)
    {
        pattern = new List<Tuple<int, float, int>>();
        createLevel(1, 0, 0);
        createLevel(1, 0, 1);
        //readLevel(world, level);
        readLevel(0, 1);
    }

    void createLevel(int numEnemies, int world, int level)
    {
        pattern = new List<Tuple<int, float, int>>();
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
            {
                if ((i % 3 == 0 && level != 0) && i != 0)
                {
                    if (slot == 1) slot = 2;
                    if (slot == 5) slot = 4;
                    pattern.Add(new Tuple<int, float, int>(slot, time, 2));
                }
                else
                    pattern.Add(new Tuple<int, float, int>(slot, time, 2));
            }
            else
                i--;
        }
        _threeStarsScore = 1400;
        writeLevel("Level_" + (world+1) + "_" + (level+1), pattern, _threeStarsScore);
    }

    void writeLevel(string levelName, List<Tuple<int, float, int>> pattern, int maxScore)
    {
        string levelString = "";

        foreach (GamePatterns.Tuple<int, float, int> move in pattern)
            levelString += ";" + move.ToString();

        levelString = levelString.Substring(1);

        fileName = levelName;
        file = new FileInfo(Application.persistentDataPath + "\\" + fileName + ".msv");

        if (file.Exists)
           file.Delete();

        writer = file.CreateText();
        writer.WriteLine(maxScore + "&" + levelString);
        writer.Close();
    }
    
    void readLevel(int world, int level)
    {
        pattern = new List<Tuple<int, float, int>>();
        string levelName = "Level_" + (world + 1) + "_" + (level + 1);
        string levelString = "";
        string[] levelData;

        file = new FileInfo(Application.persistentDataPath + "\\" + levelName + ".msv");
        if (file.Exists)
        {
            reader = file.OpenText();
            levelString = reader.ReadLine();

            _threeStarsScore = int.Parse( levelString.Split('&')[0] );
            levelData = levelString.Split('&')[1].Split(';');
            numEnemies = levelData.Length;
            foreach(string data in levelData)
            {
                string[] arrayData = data.Split(',');
                int slot = int.Parse(arrayData[0]);
                float time = float.Parse(arrayData[1]);
                int enemy = int.Parse(arrayData[2]);
                pattern.Add( new Tuple<int, float, int>(slot, time, enemy) );
            }
            reader.Close();
        }
        Debug.Log(levelString);
    }

    public List<Tuple<int, float, int>> getPattern()
    {
        return this.pattern;
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

        public override string ToString()
        {
            return Line + "," + Time + "," + Enemy;
        }
    }

}