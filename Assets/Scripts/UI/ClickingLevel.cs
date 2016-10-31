using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickingLevel : MonoBehaviour {

    private int buttonNumber;
    private int[] maxLevel;
    private GameData gameData;

    private LayerMask layerMask;
    private GameObject button;
    private RaycastHit2D raycast;

    void Start () {
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
                    maxLevel = AdvanceController.instance.maxLevel;
                    button = raycast.collider.gameObject;
                    buttonNumber = int.Parse( button.name.Substring(button.name.Length - 1) ) - 1;

                    if (buttonNumber <= maxLevel[gameData.actualworld] || maxLevel[gameData.actualworld] == 0)
                        StartCoroutine( loadLevel(buttonNumber, gameData.actualworld) );
                }
            }
        }
    }

    IEnumerator loadLevel(int level, int world)
    {
        gameData.loadingWindow.SetActive(true);
        AdvanceController.instance.activeLevel = level;
        AdvanceController.instance.activeWorld = world;
        PlayerPrefs.SetString(PlayerPrefsName.PLAYER_LEVEL_INFO, AdvanceController.instance.toStringPrefs());
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(AdvanceController.instance.levelScenes[world]);
    }
}
