using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelNextController : MonoBehaviour {

    public GameObject btnMenu;
    public GameObject btnNext;

    private LayerMask layerMask;
    private GameObject button;
    private RaycastHit2D raycast;

    void Start () {
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
                    GameObject.Find("backWorld").GetComponent<LevelController>().loadingWindow.SetActive(true);
                    if (raycast.transform.name.Equals("buttonMenu"))
                        StartCoroutine(loadHome());
                    if (raycast.transform.name.Equals("buttonNext"))
                        StartCoroutine(loadLevel());
                }
            }
        }
    }

    IEnumerator loadLevel()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(AdvanceController.instance.levelScenes[AdvanceController.instance.activeWorld]);
    }

    IEnumerator loadHome()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Home");
    }
}
