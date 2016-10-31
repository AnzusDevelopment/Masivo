using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

    public const int STARS_POS = 2;
    public const int BUTTONS_POS = 1;

    public GameObject[] worlds;
    public GameObject panelLevels;
    public GameObject loadingWindow;

    public GameObject button;
    public GameObject stars;
    public Sprite[] buttonPending;
    public Sprite[] buttonOk;
    public Sprite buttonZeroStar;
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
        stars = panelLevels.transform.GetChild(STARS_POS).gameObject;
        button = panelLevels.transform.GetChild(BUTTONS_POS).gameObject;
    }
}
