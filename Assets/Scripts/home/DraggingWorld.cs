using UnityEngine;
using System.Collections;

public class DraggingWorld : MonoBehaviour {

    enum Direction { Up, Down };

    private static readonly Vector3 UPPER_MOVEMENT = new Vector3(0f, 3.26f, -0.2f);
    private static readonly Vector3 LOWER_MOVEMENT = new Vector3(0f, 2.3f, -0.2f);
    private static readonly Vector3 START_POINT = new Vector3(0f, -1.9f, -0.2f);
    private static readonly Vector3 LOWER_POINT = new Vector3(0f, -4.2f, -0.2f);
    private static readonly Vector3 UPPER_POINT = new Vector3(0f, 1.36f, -0.2f);
    private static readonly Color UPPER_COLOR = new Color(122f/255f, 122f/255f, 122f/255f, 1);
    private static readonly Color START_COLOR = new Color(1, 1, 1, 1);
    private static readonly Color HIDE_COLOR_DOWN = new Color(1, 1, 1, 0f);
    private static readonly Color HIDE_COLOR_UP = new Color(122f / 255f, 122f / 255f, 122f / 255f, 0f);
    private const float UPPER_COLOR_COMPONENT = 0.47f;
    private const float UPPER_SCALE_FACTOR = 0.44f;
    private const float START_COLOR_COMPONENT = 1;
    private const float START_SCALE_FACTOR = 1;
    private const float HIDE_ALPHA_COMPONENT = 0;
    private const float HIDE_SCALE_FACTOR = 0;
    private const float LOWER_SCALE_FACTOR = 0.3f;
    private const float ZERO_POINT_Y = 1.9f; 

    public GameObject world, worldUp, worldDown, worldUpHidden;
    public GameObject[] worlds;

    private Vector3 worldCenter, worldInitPosition;
    private Vector3 touchPosition;
    private Vector3 offset;
    private Vector3 newWorldCenter;
    private float worldCurrentY, offsetGeneralUp, offsetGeneralDown, worldCurrentScale;

    private RaycastHit2D raycast;
    private LayerMask layerMask;
    private bool dragging = false;

    void Start()
    {
        worldInitPosition = new Vector3();
    }
	
	void Update ()
    {
        if( world != null)
        {
            worldCurrentY = world.transform.position.y;
            if (worldCurrentY > START_POINT.y /*&& System.Array.IndexOf(worlds, world) > 0*/)
            {
                offsetGeneralDown = 0;
                moveUp();
            }
            if (worldCurrentY < START_POINT.y && System.Array.IndexOf(worlds, world) < worlds.Length - 1)
            {
                offsetGeneralUp = 0;
                moveDown();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            layerMask = 1 << LayerMask.NameToLayer("World");
            raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 50, layerMask);
            if (raycast.transform.tag.Equals("World"))
            {
                world = raycast.collider.gameObject;
                worldCenter = world.transform.position;
                if (worldCenter.y == START_POINT.y)
                {
                    int worldIndex = System.Array.IndexOf(worlds, world);
                    worldUp = worldIndex + 1 < worlds.Length ? worlds[worldIndex +1] : null;
                    worldUpHidden = worldIndex + 2 < worlds.Length ? worlds[worldIndex + 2] : null;
                    worldDown = worldIndex - 1 >= 0 ? worlds[worldIndex - 1] : null;
                    touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    offset = touchPosition - worldCenter;
                    dragging = true;
                }
                else
                    world = null;

            }
        }
        if (Input.GetMouseButton(0))
        {
            if (dragging)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newWorldCenter = touchPosition - offset;
                if (newWorldCenter.y <= UPPER_POINT.y && newWorldCenter.y >= LOWER_POINT.y)
                    world.transform.position = new Vector3(world.transform.position.x, newWorldCenter.y, world.transform.position.z);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            if(newWorldCenter.y > START_POINT.y - 2 * LOWER_MOVEMENT.y / 3 && newWorldCenter.y < START_POINT.y)
            {
                restartDrag(world, worldUp, worldUpHidden, Direction.Down);
            }
            if (newWorldCenter.y <= START_POINT.y + 3 * UPPER_MOVEMENT.y / 4 && newWorldCenter.y > START_POINT.y)
            {
                restartDrag(world, worldUp, worldDown, Direction.Up);
            }
        }
            

        foreach ( Touch touch in Input.touches)
        {
            switch ( touch.phase )
            {
                case TouchPhase.Began:
                    layerMask = 1 << LayerMask.NameToLayer("World");
                    raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 50, layerMask);
                    if (raycast.transform.tag.Equals("World"))
                    {
                        world = raycast.collider.gameObject;
                        worldCenter = world.transform.position;
                        touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        offset = touchPosition - worldCenter;
                        dragging = true;
                    }
                    break;

                case TouchPhase.Moved:
                    if (dragging)
                    {
                        touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        newWorldCenter = touchPosition - offset;
                        if( newWorldCenter.y < 2.2 && newWorldCenter.y > -4.06)
                            world.transform.position = new Vector3(world.transform.position.x, newWorldCenter.y, world.transform.position.z);
                    }
                    break;
                case TouchPhase.Ended:
                    dragging = false;
                    break;
            }
        }
    }

    void moveUp()
    {
        if (world == null) return;
        float colorValue;

        offsetGeneralUp = worldCurrentY >= 0 ? ZERO_POINT_Y + Mathf.Abs(worldCurrentY) : ZERO_POINT_Y + worldCurrentY;

        worldCurrentScale = changeFactor(UPPER_SCALE_FACTOR, START_SCALE_FACTOR - UPPER_SCALE_FACTOR, offsetGeneralUp, UPPER_MOVEMENT.y);
        world.transform.localScale = Vector3.one * worldCurrentScale;
        colorValue = changeFactor(UPPER_COLOR_COMPONENT, START_COLOR_COMPONENT - UPPER_COLOR_COMPONENT, offsetGeneralUp, UPPER_MOVEMENT.y);
        changeColorChildren(world, new Color(colorValue, colorValue, colorValue));
        
        if(worldUp != null)
        {
            worldCurrentScale = changeFactor(HIDE_SCALE_FACTOR, UPPER_SCALE_FACTOR, offsetGeneralUp, UPPER_MOVEMENT.y);
            worldUp.transform.localScale = Vector3.one * worldCurrentScale;
            colorValue = changeFactor(HIDE_ALPHA_COMPONENT, START_COLOR_COMPONENT, offsetGeneralUp, UPPER_MOVEMENT.y);
            changeColorChildren(worldUp, new Color(UPPER_COLOR_COMPONENT, UPPER_COLOR_COMPONENT, UPPER_COLOR_COMPONENT, colorValue));
        }

        if (worldDown != null)
        {
            worldDown.SetActive(worldDown.transform.position.y > LOWER_POINT.y + 0.15);
            worldDown.transform.position = new Vector3(START_POINT.x, LOWER_POINT.y + LOWER_MOVEMENT.y * offsetGeneralUp / UPPER_MOVEMENT.y, START_POINT.z);
            worldCurrentScale = changeFactor(START_SCALE_FACTOR, LOWER_SCALE_FACTOR, offsetGeneralUp, UPPER_MOVEMENT.y);
            worldDown.transform.localScale = Vector3.one * worldCurrentScale;
            colorValue = (float)(START_COLOR_COMPONENT - (UPPER_MOVEMENT.y - offsetGeneralUp) / UPPER_MOVEMENT.y);

            changeColorChildren(worldDown, new Color(1f, 1f, 1f, colorValue));
        }
            
    }

    void moveDown()
    {
        if (world == null) return;
        float colorValue;

        world.SetActive(world.transform.position.y > LOWER_POINT.y + 0.15);
        offsetGeneralDown = ZERO_POINT_Y + worldCurrentY;
        worldCurrentScale = changeFactor(START_SCALE_FACTOR, LOWER_SCALE_FACTOR, LOWER_MOVEMENT.y + offsetGeneralDown, LOWER_MOVEMENT.y);
        world.transform.localScale = Vector3.one * worldCurrentScale;
        
        colorValue = changeFactor(HIDE_ALPHA_COMPONENT, START_COLOR_COMPONENT, -1 * offsetGeneralDown, LOWER_MOVEMENT.y);
        Debug.Log(colorValue);
        changeColorChildren(world, new Color(START_COLOR_COMPONENT, START_COLOR_COMPONENT, START_COLOR_COMPONENT, colorValue));

        if (worldUp != null)
        {
            worldUp.transform.position = new Vector3(UPPER_POINT.x, UPPER_POINT.y + UPPER_MOVEMENT.y * offsetGeneralDown / LOWER_MOVEMENT.y, UPPER_POINT.z);
            worldCurrentScale = changeFactor(UPPER_SCALE_FACTOR, START_SCALE_FACTOR - UPPER_SCALE_FACTOR, LOWER_MOVEMENT.y + offsetGeneralDown, LOWER_MOVEMENT.y);
            worldUp.transform.localScale = Vector3.one * worldCurrentScale;
            colorValue = changeFactor(UPPER_COLOR_COMPONENT, START_COLOR_COMPONENT - UPPER_COLOR_COMPONENT, LOWER_MOVEMENT.y + offsetGeneralDown, LOWER_MOVEMENT.y);
            changeColorChildren(worldUp, new Color(colorValue, colorValue, colorValue));
        }

        if (worldUpHidden != null)
        {
            worldUpHidden.SetActive(worldUpHidden.transform.localScale.y > HIDE_SCALE_FACTOR + 0.15);
            worldCurrentScale = changeFactor(HIDE_SCALE_FACTOR, UPPER_SCALE_FACTOR, LOWER_MOVEMENT.y + offsetGeneralDown, LOWER_MOVEMENT.y);
            worldUpHidden.transform.localScale = Vector3.one * worldCurrentScale;
            colorValue = changeFactor(HIDE_ALPHA_COMPONENT, START_COLOR_COMPONENT, LOWER_MOVEMENT.y + offsetGeneralDown, LOWER_MOVEMENT.y);
            changeColorChildren(worldUpHidden, new Color(UPPER_COLOR_COMPONENT, UPPER_COLOR_COMPONENT, UPPER_COLOR_COMPONENT, colorValue));
        }

    }

    float changeFactor(float baseScale, float startScale, float offset, float reference)
    {
        return (float) (baseScale + (reference - offset) / reference * startScale);
    }

    void changeColorChildren(GameObject objectToPaint, Color color)
    {
        GameObject terrain;
        for (var i = 0; i < objectToPaint.transform.childCount; i++)
        {
            terrain = objectToPaint.transform.GetChild(i).gameObject;
            for (var j = 0; j < terrain.transform.childCount; j++)
            {
                GameObject item = terrain.transform.GetChild(j).gameObject;
                if (item.GetComponent<SpriteRenderer>() != null)
                    item.GetComponent<SpriteRenderer>().color = color;
                if(item.transform.childCount > 0)
                {
                    if (item.transform.GetChild(0) != null)
                    {
                        item.transform.GetChild(0).gameObject.GetComponent<Renderer>().sortingOrder = 0;
                    }
                }
            }
        }
    }

    void goToCenter(GameObject worldToMove)
    {
        if (worldToMove == null) return;
        worldToMove.transform.position = START_POINT;
        worldToMove.transform.localScale = new Vector3(START_SCALE_FACTOR, START_SCALE_FACTOR, START_SCALE_FACTOR);
        changeColorChildren(worldToMove, START_COLOR);
    }

    void goToUpper(GameObject worldToMove)
    {
        if (worldToMove == null) return;
        worldToMove.transform.position = UPPER_POINT;
        worldToMove.transform.localScale = new Vector3(UPPER_SCALE_FACTOR, UPPER_SCALE_FACTOR, UPPER_SCALE_FACTOR);
        changeColorChildren(worldToMove, UPPER_COLOR);
    }

    void hideLower(GameObject worldToMove)
    {
        if (worldToMove == null) return;
        worldToMove.transform.position = LOWER_POINT;
        worldToMove.transform.localScale = new Vector3(LOWER_SCALE_FACTOR, LOWER_SCALE_FACTOR, LOWER_SCALE_FACTOR);
        changeColorChildren(worldToMove, HIDE_COLOR_DOWN);
        worldToMove.SetActive(false);
    }

    void hideUpper(GameObject worldToMove)
    {
        if (worldToMove == null) return;
        worldToMove.transform.position = UPPER_MOVEMENT;
        worldToMove.transform.localScale = Vector3.one * HIDE_SCALE_FACTOR;
        changeColorChildren(worldToMove, HIDE_COLOR_UP);
    }

    void restartDrag(GameObject worldCenter, GameObject worldUp, GameObject worldHidden, Direction direction)
    {
        goToCenter(worldCenter);
        goToUpper(worldUp);
        if(direction == Direction.Up)
            hideLower(worldHidden);
        else
            hideUpper(worldHidden);
    }

    void moveDragged(GameObject worldCenter, GameObject worldUp, GameObject worldHidden, Direction direction)
    {
        if(direction == Direction.Up)
        {
            hideUpper(worldUp);
            goToUpper(worldCenter);
            goToCenter(worldHidden);
        }else
        {
            hideLower(worldCenter);
            goToCenter(worldUp);
            goToUpper(worldHidden);
        }
    }

}
