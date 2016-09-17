using UnityEngine;
using System.Collections;

public class LevelInfo {

    private bool[] _validWorld;
    public bool[] validWorld
    {
        get { return _validWorld; }
        set { _validWorld = value; }
    }

    private int[] _maxLevel;
    public int[] maxLevel
    {
        get { return _maxLevel; }
        set { _maxLevel = value; }
    }

    private int[,] _score;
    public int[,] score
    {
        get { return _score; }
        set { _score = value; }
    }

    private int[,] _stars;
    public int[,] stars
    {
        get { return _stars; }
        set { _stars = value; }
    }

    public LevelInfo() { }

    public LevelInfo(int numberOfWorlds, int levelsNumber)
    {
        validWorld = new bool[numberOfWorlds];
        maxLevel = new int[numberOfWorlds];
        score = new int[numberOfWorlds, levelsNumber];
        stars = new int[numberOfWorlds, levelsNumber];

        for (var i = 0; i < numberOfWorlds; i++)
        {
            validWorld[i] = i == 0;
            maxLevel[i] = 0;

            for (var j = 0; j < levelsNumber; j++)
            {
                score[i, j] = 0;
                stars[i, j] = -1;
            }
        }
    }

    public LevelInfo(bool[] actualLevel, int[,] score, int[,] stars)
    {
        _validWorld = actualLevel;
        _score = score;
        _stars = stars;
    }

    public string toStringPrefs()
    {
        string validWorldInfo = "";
        string maxLevelInfo = "";
        string scoreInfo = "";
        string starsInfo = "";
        
        for(var i = 0; i < score.GetLength(0); i++)
        {
            validWorldInfo += validWorld[i] ? ",1" : ",0";
            maxLevelInfo += "," + maxLevel[i]; 
            for (var j = 0; j < score.GetLength(1); j++)
            {
                scoreInfo += score[i,j] + ",";
                starsInfo += stars[i,j] + ",";
            }
            scoreInfo = scoreInfo.Substring(0, scoreInfo.Length - 1);
            starsInfo = starsInfo.Substring(0, starsInfo.Length - 1);
            scoreInfo += ";";
            starsInfo += ";";
        }
        validWorldInfo = validWorldInfo.Substring(1);
        maxLevelInfo = maxLevelInfo.Substring(1);
        scoreInfo = scoreInfo.Substring(0, scoreInfo.Length - 1);
        starsInfo = starsInfo.Substring(0, starsInfo.Length - 1);

        return validWorldInfo + "&" + maxLevelInfo + "&" + scoreInfo + "&" + starsInfo;
    }

    public void stringToLevelInfo(string stringLevelInfo)
    {
        string[] infoData = stringLevelInfo.Split('&');
        string[] validWorldString = infoData[0].Split(',');
        string[] maxLevelString = infoData[1].Split(',');
        string[] scoreString = infoData[2].Split(';');
        string[] starsString = infoData[3].Split(';');

        int numberOfWorlds = validWorldString.Length;
        int numberOfLevels = scoreString[0].Split(',').Length;

        validWorld = new bool[numberOfWorlds];
        maxLevel = new int[numberOfWorlds];
        score = new int[numberOfWorlds, numberOfLevels];
        stars = new int[numberOfWorlds, numberOfLevels];

        for (var i = 0; i < numberOfWorlds; i++)
        {
            string[] scoreWorld = scoreString[i].Split(',');
            string[] starsWorld = starsString[i].Split(',');
            validWorld[i] = validWorldString[i].Equals("1") ? true : false;
            maxLevel[i] = int.Parse(maxLevelString[i]);
            for (var j = 0; j < numberOfLevels; j++)
            {
                score[i, j] = int.Parse(scoreWorld[j]);
                stars[i, j] = int.Parse(starsWorld[j]);
            }
        }
    }

}
