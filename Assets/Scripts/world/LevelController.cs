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

    public static int actualLevel;
    public static int actualWorld;
    private int numberOfWorlds;

    AdvanceController advanceIntance;

    void Awake()
    {
        advanceIntance = AdvanceController.instance;
        actualLevel = advanceIntance.activeLevel;
        actualWorld = advanceIntance.activeWorld;
    }

	void Start () {
	
	}
	
	void Update () {
	
	}

    public void openPanelNextLevel(int threeStarsScore)
    {
        panelNextLevel.SetActive(true);
        advanceIntance.maxLevel[actualWorld] = advanceIntance.maxLevel[actualWorld] < 8? advanceIntance.maxLevel[actualWorld] + 1 : 8;
        advanceIntance.score[actualWorld, actualLevel] = ScoreController.Instance.getScore();

        advanceIntance.activeLevel = actualLevel < 8 ? actualLevel + 1 : 0;

        numberOfWorlds = AdvanceController.NUMBER_OF_WORLDS;

        if (actualLevel == 8)
        {
            if (actualWorld < numberOfWorlds - 1) {
                advanceIntance.validWorld[actualWorld + 1] = true;
                advanceIntance.activeWorld = actualWorld + 1;
            }
        }
        
        if (advanceIntance.score[actualWorld, actualLevel] >= threeStarsScore * 0.95)
        {
            advanceIntance.stars[actualWorld, actualLevel] = 3;
            panelNextLevel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonThreeStars;
        }
        else if (advanceIntance.score[actualWorld, actualLevel] >= threeStarsScore * 0.90)
        {
            advanceIntance.stars[actualWorld, actualLevel] = 2;
            panelNextLevel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonTwoStars;
        }
        else if (advanceIntance.score[actualWorld, actualLevel] >= threeStarsScore * 0.85)
        {
            advanceIntance.stars[actualWorld, actualLevel] = 1;
            panelNextLevel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonOneStar;
        }
        else
        {
            advanceIntance.stars[actualWorld, actualLevel] = 0;
            panelNextLevel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = buttonOk;
        }
        PlayerPrefs.SetString(PlayerPrefsName.PLAYER_LEVEL_INFO, advanceIntance.toStringPrefs());
    }
}