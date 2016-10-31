using UnityEngine;

public class ClickingWorld : MonoBehaviour {

    public GameObject panelSettings;

    private static readonly Vector3 START_POINT = new Vector3(0.0f, -1.9f, -1.0f);
    private static readonly Vector4 OUTSIDE_PANEL = new Vector4(-2.55f, 1.54f, 2.55f, -4.05f);

    private GameObject world;

    private bool dragging = false;
    private bool panelOpen = false;
    private int actualworld;
    private Sprite buttonSprite;
    private Sprite starsSprite;
    private Vector3 worldCenter;
    private Vector3 touchPosition;
    private Vector3 offset;
    private Vector3 newWorldCenter;
    private GameData gameData;

    private GameObject panel;
    private GameObject[] worlds;

    private LayerMask layerMask;
    private RaycastHit2D raycast;

    void Start () {
        gameData = GameObject.Find("GameController").GetComponent<GameData>();
        worlds = gameData.worlds;
    }
	
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            layerMask = 1 << LayerMask.NameToLayer("World");
            raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 50, layerMask);
            if (raycast.transform != null)
            {
                if (raycast.transform.tag.Equals("World"))
                {
                    world = raycast.collider.gameObject;
                    worldCenter = world.transform.position;
                    if (worldCenter.y >= START_POINT.y - 0.001 && worldCenter.y < START_POINT.y + 0.001)
                    {
                        touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        offset = touchPosition - worldCenter;
                        dragging = true;
                    }
                }
            }
            if (panelOpen)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if(touchPosition.x < OUTSIDE_PANEL.x || touchPosition.x > OUTSIDE_PANEL.z || touchPosition.y > OUTSIDE_PANEL.y || touchPosition.y < OUTSIDE_PANEL.w)
                {
                    gameData.panelLevels.SetActive(false);
                    panelOpen = false;
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (dragging)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newWorldCenter = touchPosition - offset;
            }
        }

        if (Input.GetMouseButtonUp(0) && !panelSettings.activeSelf)
        {
            if (newWorldCenter.y >= START_POINT.y - 0.1f && newWorldCenter.y <= START_POINT.y + 0.1f  && dragging)
            {
                actualworld = System.Array.IndexOf(worlds, world);
                if(AdvanceController.instance.validWorld[actualworld])
                {
                    for (var i = 0; i < AdvanceController.NUMBER_OF_LEVELS; i++)
                    {
                        buttonSprite = gameData.buttonOk[actualworld];
                        switch ( AdvanceController.instance.stars[actualworld,i] )
                        {
                            case -1:
                                buttonSprite = gameData.buttonPending[actualworld];
                                starsSprite = gameData.buttonZeroStar;
                                break;
                            case 0:
                                starsSprite = gameData.buttonZeroStar;
                                break;
                            case 1:
                                starsSprite = gameData.buttonOneStar;
                                break;
                            case 2:
                                starsSprite = gameData.buttonTwoStars;
                                break;
                            case 3:
                                starsSprite = gameData.buttonThreeStars;
                                break;
                        }
                        gameData.button.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = buttonSprite;
                        gameData.stars.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = starsSprite;
                    }
                    gameData.actualworld = actualworld;
                    gameData.panelLevels.SetActive(true);
                    panelOpen = true;
                }
            }
            dragging = false;
        }

    }
}
