using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClickingLevel : MonoBehaviour {

    private int buttonNumber;
    private int[] maxLevel;
    private UserData userData;
    private GameData gameData;

    private LayerMask layerMask;
    private GameObject button;
    private RaycastHit2D raycast;

    void Start () {
        userData = GameObject.Find("GameController").GetComponent<UserData>();
        gameData = GameObject.Find("GameController").GetComponent<GameData>();
    }
	
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            layerMask = 1 << LayerMask.NameToLayer("ButtonLevel");
            raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 50, layerMask);
            if (raycast.transform != null)
            {
                if (raycast.transform.tag.Equals("ButtonLevel"))
                {
                    maxLevel = userData.levelInfo.maxLevel;
                    button = raycast.collider.gameObject;
                    buttonNumber = int.Parse( button.name.Substring(button.name.Length - 1) ) - 1;

                    if (buttonNumber < maxLevel[gameData.actualworld] || maxLevel[gameData.actualworld] == 0)
                        loadLevel(buttonNumber, gameData.actualworld);
                }
            }
        }

    }

    void loadLevel(int level, int world)
    {
        PlayerPrefs.SetInt(PlayerPrefsName.PLAYER_ACTUAL_WORLD, world);
        PlayerPrefs.SetInt(PlayerPrefsName.PLAYER_ACTUAL_LEVEL, level);
        PlayerPrefs.SetString(PlayerPrefsName.PLAYER_LEVEL_INFO, userData.levelInfo.toStringPrefs());
        gameData.loadingWindow.SetActive(true);
        SceneManager.LoadScene("LevelsWorld_0");
    }
}
