using UnityEngine;
using System.Collections;

public class AdvanceController : MonoBehaviour {

    public static AdvanceController instance;

    public const int NUMBER_OF_WORLDS = 3;
    public const int NUMBER_OF_LEVELS = 9;

    public string[] levelScenes;

    private bool[] _validWorld;
    public bool[] validWorld { get { return _validWorld; } set { _validWorld = value; } }

    private int[] _maxLevel;
    public int[] maxLevel { get { return _maxLevel; } set { _maxLevel = value; } }

    private int[,] _score;
    public int[,] score { get { return _score; } set { _score = value; } }

    private int[,] _stars;
    public int[,] stars { get { return _stars; } set { _stars = value; } }

    private int _activeWorld;
    public int activeWorld { get { return _activeWorld; } set { _activeWorld = value; } }

    private int _activeLevel;
    public int activeLevel { get { return _activeLevel; } set { _activeLevel = value; } }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        levelScenes = new string[] { "LevelsWorld_0", "LevelsWorld_1", "LevelsWorld_2" };

        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey(PlayerPrefsName.PLAYER_LEVEL_INFO))
        {
            instance.activeWorld = 0;
            instance.activeLevel = 0;
            instance.validWorld = new bool[NUMBER_OF_WORLDS];
            instance.maxLevel = new int[NUMBER_OF_WORLDS];
            instance.score = new int[NUMBER_OF_WORLDS, NUMBER_OF_LEVELS];
            instance.stars = new int[NUMBER_OF_WORLDS, NUMBER_OF_LEVELS];

            for (var i = 0; i < NUMBER_OF_WORLDS; i++)
            {
                instance.validWorld[i] = i == 0;
                instance.maxLevel[i] = 0;

                for (var j = 0; j < NUMBER_OF_LEVELS; j++)
                {
                    instance.score[i, j] = 0;
                    instance.stars[i, j] = -1;
                }
            }
        }
        else
        {
            stringToLevelInfo(PlayerPrefs.GetString(PlayerPrefsName.PLAYER_LEVEL_INFO));
        }
    }

    void Start () {
        
    }

    public string toStringPrefs()
    {
        string validWorldInfo = "";
        string maxLevelInfo = "";
        string scoreInfo = "";
        string starsInfo = "";

        for (var i = 0; i < score.GetLength(0); i++)
        {
            validWorldInfo += instance.validWorld[i] ? ",1" : ",0";
            maxLevelInfo += "," + instance.maxLevel[i];
            for (var j = 0; j < instance.score.GetLength(1); j++)
            {
                scoreInfo += instance.score[i, j] + ",";
                starsInfo += instance.stars[i, j] + ",";
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

        return validWorldInfo + "&" + maxLevelInfo + "&" + scoreInfo + "&" + starsInfo + "&" + instance.activeWorld + "&" + instance.activeLevel;
    }

    public void stringToLevelInfo(string stringLevelInfo)
    {
        string[] infoData = stringLevelInfo.Split('&');
        string[] validWorldString = infoData[0].Split(',');
        string[] maxLevelString = infoData[1].Split(',');
        string[] scoreString = infoData[2].Split(';');
        string[] starsString = infoData[3].Split(';');

        instance.validWorld = new bool[NUMBER_OF_WORLDS];
        instance.maxLevel = new int[NUMBER_OF_WORLDS];
        instance.score = new int[NUMBER_OF_WORLDS, NUMBER_OF_LEVELS];
        instance.stars = new int[NUMBER_OF_WORLDS, NUMBER_OF_LEVELS];
        instance.activeWorld = int.Parse(infoData[4]);
        instance.activeLevel = int.Parse(infoData[5]);

        for (var i = 0; i < NUMBER_OF_WORLDS; i++)
        {
            string[] scoreWorld = scoreString[i].Split(',');
            string[] starsWorld = starsString[i].Split(',');
            instance.validWorld[i] = validWorldString[i].Equals("1") ? true : false;
            instance.maxLevel[i] = int.Parse(maxLevelString[i]);
            for (var j = 0; j < NUMBER_OF_LEVELS; j++)
            {
                instance.score[i, j] = int.Parse(scoreWorld[j]);
                instance.stars[i, j] = int.Parse(starsWorld[j]);
            }
        }
    }

}
