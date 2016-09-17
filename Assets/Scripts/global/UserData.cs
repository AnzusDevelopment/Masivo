using UnityEngine;
using System.Collections;

public class UserData : MonoBehaviour {

    public int activeWorld;
    public LevelInfo levelInfo;

    private GameData gameData;
    private GameObject[] worlds;
    private bool userRegistered = false;

    void Awake()
    {
        gameData = gameObject.GetComponent<GameData>();
        worlds = gameData.worlds;

        //PlayerPrefs.DeleteAll();
        if (!userRegistered && !PlayerPrefs.HasKey(PlayerPrefsName.PLAYER_LEVEL_INFO))
        {
            activeWorld = 0;
            levelInfo = new LevelInfo(worlds.Length, GameData.LEVELS_PER_WORLD);
        }
        else
        {
            activeWorld = PlayerPrefs.GetInt(PlayerPrefsName.PLAYER_ACTUAL_WORLD);
            levelInfo = new LevelInfo();
            levelInfo.stringToLevelInfo(PlayerPrefs.GetString(PlayerPrefsName.PLAYER_LEVEL_INFO));
            Debug.Log("levelInfo: world" + levelInfo.toStringPrefs());
        }
    }

}

