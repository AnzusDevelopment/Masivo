using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    public Sprite buttonOk;
    public Sprite buttonOneStar;
    public Sprite buttonTwoStars;
    public Sprite buttonThreeStars;

    public GameObject panelNextLevel;
    public GameObject panelPaused;
    public GameObject panelFailed;
    public GameObject loadingWindow;

    LevelInfo levelInfo;

    public static int actualLevel;
    public static int actualWorld;
    private int numberOfWorlds;

    void Awake()
    {
        levelInfo = new LevelInfo();
        levelInfo.stringToLevelInfo(PlayerPrefs.GetString(PlayerPrefsName.PLAYER_LEVEL_INFO));
        actualLevel = PlayerPrefs.GetInt(PlayerPrefsName.PLAYER_ACTUAL_LEVEL);
        actualWorld = PlayerPrefs.GetInt(PlayerPrefsName.PLAYER_ACTUAL_WORLD);
    }

	void Start () {
	
	}
	
	void Update () {
	
	}

    public void openPanelNextLevel(int threeStarsScore)
    {
        panelNextLevel.SetActive(true);
        levelInfo.maxLevel[actualWorld] = levelInfo.maxLevel[actualWorld] < 8? levelInfo.maxLevel[actualWorld] + 1 : 8;
        levelInfo.score[actualWorld, actualLevel] = ScoreController.Instance.getScore();

        PlayerPrefs.SetInt(PlayerPrefsName.PLAYER_ACTUAL_LEVEL, actualLevel <= 8 ? actualLevel + 1 : 0 );

        Debug.Log("actual level: " + PlayerPrefs.GetInt(PlayerPrefsName.PLAYER_ACTUAL_LEVEL));
        Debug.Log("actual world: " + PlayerPrefs.GetInt(PlayerPrefsName.PLAYER_ACTUAL_WORLD));

        if (levelInfo.maxLevel[actualWorld] == 8)
        {
            numberOfWorlds = levelInfo.validWorld.Length;
            if (actualWorld < numberOfWorlds - 1) {
                levelInfo.validWorld[actualWorld + 1] = true;
                PlayerPrefs.SetInt(PlayerPrefsName.PLAYER_ACTUAL_WORLD, actualWorld + 1);
            }
        }
        
        if(levelInfo.score[actualWorld, actualLevel] >= threeStarsScore * 0.95)
        {
            levelInfo.stars[actualWorld, actualLevel] = 3;
            panelNextLevel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonThreeStars;
        }
        else if (levelInfo.score[actualWorld, actualLevel] >= threeStarsScore * 0.90)
        {
            levelInfo.stars[actualWorld, actualLevel] = 2;
            panelNextLevel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonTwoStars;
        }
        else if (levelInfo.score[actualWorld, actualLevel] >= threeStarsScore * 0.85)
        {
            levelInfo.stars[actualWorld, actualLevel] = 1;
            panelNextLevel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonOneStar;
        }
        else
        {
            levelInfo.stars[actualWorld, actualLevel] = 0;
            panelNextLevel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonOk;
        }
        PlayerPrefs.SetString(PlayerPrefsName.PLAYER_LEVEL_INFO, levelInfo.toStringPrefs());
        Debug.Log("levelInfo level: " + levelInfo.toStringPrefs());
    }
}
