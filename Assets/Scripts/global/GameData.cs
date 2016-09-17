using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {


    public const int LEVELS_PER_WORLD = 9;
    public const int BUTTONS_POS = 2;

    public GameObject[] worlds;
    public GameObject panelLevels;
    public GameObject loadingWindow;

    public GameObject button;
    public Sprite buttonPending;
    public Sprite buttonOk;
    public Sprite buttonOneStar;
    public Sprite buttonTwoStars;
    public Sprite buttonThreeStars;

    private int _actualWorld = 0;
    public int actualworld
    {
        get { return _actualWorld; }
        set { _actualWorld = value; }
    }

    void Awake () {
        panelLevels.SetActive(false);
        loadingWindow.SetActive(false);
        button = panelLevels.transform.GetChild(BUTTONS_POS).gameObject;
	}
}
