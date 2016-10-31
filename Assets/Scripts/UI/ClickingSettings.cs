using UnityEngine;
using System.Collections;

public class ClickingSettings : MonoBehaviour {

    public GameObject panelSettings;
    public GameObject panelLevels;

    private LayerMask layerMask;
    private GameObject button;
    private RaycastHit2D raycast;

    void Start () {
	
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            layerMask = 1 << LayerMask.NameToLayer("ButtonSettings");
            raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 50, layerMask);
            if (raycast.transform != null)
            {
                if (raycast.transform.tag.Equals("ButtonSettings"))
                {
                    if(!panelSettings.activeSelf && !panelLevels.activeSelf)
                        panelSettings.SetActive(true);
                    else if (panelSettings.activeSelf)
                        panelSettings.SetActive(false);
                }
            }
        }
    }
}
