using UnityEngine;
using System.Collections;

public class DraggingWorld : MonoBehaviour {

    enum Direction { Up, Down };

    private static readonly Vector3 UPPER_MOVEMENT = new Vector3(0.0f, 3.26f, 1.0f);
    private static readonly Vector3 LOWER_MOVEMENT = new Vector3(0.0f, 2.3f, 1.0f);
    private static readonly Vector3 START_POINT = new Vector3(0.0f, -1.9f, -1.0f);
    private static readonly Vector3 LOWER_POINT = new Vector3(0.0f, -4.2f, -2.0f);
    private static readonly Vector3 UPPER_POINT = new Vector3(0.0f, 1.36f, 0.0f);
    
    private static readonly Color UPPER_COLOR = new Color(122f/255f, 122f/255f, 122f/255f, 1.0f);
    private static readonly Color START_COLOR = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private static readonly Color HIDE_COLOR_DOWN = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    private static readonly Color HIDE_COLOR_UP = new Color(122f / 255f, 122f / 255f, 122f / 255f, 0.0f);
    private const float UPPER_COLOR_COMPONENT = 0.47f;
    private const float UPPER_SCALE_FACTOR = 0.44f;
    private const float START_COLOR_COMPONENT = 1.0f;
    private const float START_SCALE_FACTOR = 1.0f;
    private const float HIDE_ALPHA_COMPONENT = 0.0f;
    private const float HIDE_SCALE_FACTOR = 0.0f;
    private const float LOWER_SCALE_FACTOR = 0.3f;
    private const float ZERO_POINT_Y = 1.9f;
    private const float BOUNCE_UP = -1.2f;
    private const float BOUNCE_DOWN = -2.6f;

    private int activeWorld;
    private int validWorld;

    private Vector3 worldCenter;
    private Vector3 touchPosition;
    private Vector3 offset;
    private Vector3 newWorldCenter;
    private GameData gameData;
    private GameObject[] worlds;
    private GameObject world, worldUp, worldDown, worldUpHidden;
    private float worldCurrentY, offsetGeneralUp, offsetGeneralDown, worldCurrentScale;

    private RaycastHit2D raycast;
    private LayerMask layerMask;
    private bool dragging = false;

    void Start()
    {
        gameData = GameObject.Find("GameController").GetComponent<GameData>();

        worlds = gameData.worlds;
        activeWorld = AdvanceController.instance.activeWorld;
        validWorld = 0;

        for(var i = 0; i < worlds.Length; i++)
        {
            if (AdvanceController.instance.validWorld[i])
                validWorld = i;

            if (i == activeWorld)
                goToCenter(worlds[i]);
            if (i < activeWorld)
                hideLower(worlds[i]);
            if (i - 1 == activeWorld)
                goToUpper(worlds[i]);
            if (i - 1 > activeWorld)
                hideUpper(worlds[i]);
        }  
    }

	void Update ()
    {
        if( world != null)
        {
            worldCurrentY = world.transform.position.y;
            if (worldCurrentY > START_POINT.y)
            {
                offsetGeneralDown = 0;
                moveUp();
            }
            if (worldCurrentY < START_POINT.y)
            {
                offsetGeneralUp = 0;
                moveDown();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            layerMask = 1 << LayerMask.NameToLayer("World");
            raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 50, layerMask);
            if(raycast.transform != null)
            {
                if (raycast.transform.tag.Equals("World"))
                {
                    world = raycast.collider.gameObject;
                    worldCenter = world.transform.position;
                    if (worldCenter.y >= START_POINT.y -0.001 && worldCenter.y < START_POINT.y + 0.001)
                    {
                        int worldIndex = System.Array.IndexOf(worlds, world);
                        worldUp = worldIndex + 1 < worlds.Length ? worlds[worldIndex + 1] : null;
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
        }

        if (Input.GetMouseButton(0))
        {
            if (dragging)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newWorldCenter = touchPosition - offset;
                if(newWorldCenter.y > START_POINT.y)
                {
                    if(System.Array.IndexOf(worlds, world) != 0)
                    {
                        if (newWorldCenter.y <= UPPER_POINT.y)
                            world.transform.position = new Vector3(world.transform.position.x, newWorldCenter.y, world.transform.position.z);
                    }else
                    {
                        if (newWorldCenter.y <= BOUNCE_UP)
                            world.transform.position = new Vector3(world.transform.position.x, newWorldCenter.y, world.transform.position.z);
                        if (newWorldCenter.y > BOUNCE_UP)
                            dragging = false;
                    }
                }

                if (newWorldCenter.y < START_POINT.y)
                {
                    if(System.Array.IndexOf(worlds, world) != worlds.Length - 1 && System.Array.IndexOf(worlds, world) < validWorld)
                    {
                        if (newWorldCenter.y >= LOWER_POINT.y)
                            world.transform.position = new Vector3(world.transform.position.x, newWorldCenter.y, world.transform.position.z);
                    }else
                    {
                        if (newWorldCenter.y >= BOUNCE_DOWN)
                            world.transform.position = new Vector3(world.transform.position.x, newWorldCenter.y, world.transform.position.z);
                        if (newWorldCenter.y < BOUNCE_DOWN)
                            dragging = false;
                    }
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            if(newWorldCenter.y > START_POINT.y - 2 * LOWER_MOVEMENT.y / 3 && newWorldCenter.y < START_POINT.y)
                restartDrag(world, worldUp, worldUpHidden, Direction.Down);
            
            if (newWorldCenter.y <= START_POINT.y - 2 * LOWER_MOVEMENT.y / 3 && newWorldCenter.y < START_POINT.y)
                moveDragged(world, worldUp, worldUpHidden, Direction.Down);
            
            if (newWorldCenter.y <= START_POINT.y + 3 * UPPER_MOVEMENT.y / 4 && newWorldCenter.y > START_POINT.y)
                restartDrag(world, worldUp, worldDown, Direction.Up);
            
            if (newWorldCenter.y > START_POINT.y + 3 * UPPER_MOVEMENT.y / 4 && newWorldCenter.y > START_POINT.y)
                moveDragged(world, worldUp, worldDown, Direction.Up);
        }

    }

    void moveUp()
    {
        if (world == null) return;
        float colorValue;

        offsetGeneralUp = worldCurrentY >= 0 ? ZERO_POINT_Y + Mathf.Abs(worldCurrentY) : ZERO_POINT_Y + worldCurrentY;

        float zPos = changeFactor(START_POINT.z, UPPER_MOVEMENT.z, UPPER_MOVEMENT.y - offsetGeneralUp, UPPER_MOVEMENT.y);
        world.transform.position = new Vector3(START_POINT.x, world.transform.position.y, zPos);
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
            zPos = changeFactor(LOWER_POINT.z, UPPER_MOVEMENT.z, UPPER_MOVEMENT.y - offsetGeneralUp, UPPER_MOVEMENT.y);
            worldDown.transform.position = new Vector3(START_POINT.x, LOWER_POINT.y + LOWER_MOVEMENT.y * offsetGeneralUp / UPPER_MOVEMENT.y, zPos);
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
        float zPos = changeFactor(START_POINT.z, LOWER_MOVEMENT.z, LOWER_MOVEMENT.y - offsetGeneralDown, LOWER_MOVEMENT.y);
        world.transform.position = new Vector3(START_POINT.x, world.transform.position.y, zPos);
        worldCurrentScale = changeFactor(START_SCALE_FACTOR, LOWER_SCALE_FACTOR, LOWER_MOVEMENT.y + offsetGeneralDown, LOWER_MOVEMENT.y);
        world.transform.localScale = Vector3.one * worldCurrentScale;
        
        colorValue = changeFactor(HIDE_ALPHA_COMPONENT, START_COLOR_COMPONENT, -1 * offsetGeneralDown, LOWER_MOVEMENT.y);
        changeColorChildren(world, new Color(START_COLOR_COMPONENT, START_COLOR_COMPONENT, START_COLOR_COMPONENT, colorValue));

        if (worldUp != null)
        {
            zPos = changeFactor(UPPER_POINT.z, LOWER_MOVEMENT.z, LOWER_MOVEMENT.y - offsetGeneralDown, LOWER_MOVEMENT.y);
            worldUp.transform.position = new Vector3(UPPER_POINT.x, UPPER_POINT.y + UPPER_MOVEMENT.y * offsetGeneralDown / LOWER_MOVEMENT.y, zPos);
            worldCurrentScale = changeFactor(UPPER_SCALE_FACTOR, START_SCALE_FACTOR - UPPER_SCALE_FACTOR, LOWER_MOVEMENT.y + offsetGeneralDown, LOWER_MOVEMENT.y);
            worldUp.transform.localScale = Vector3.one * worldCurrentScale;
            colorValue = changeFactor(UPPER_COLOR_COMPONENT, START_COLOR_COMPONENT - UPPER_COLOR_COMPONENT, LOWER_MOVEMENT.y + offsetGeneralDown, LOWER_MOVEMENT.y);
            changeColorChildren(worldUp, new Color(colorValue, colorValue, colorValue));
        }

        if (worldUpHidden != null)
        {
            worldUpHidden.SetActive(worldUpHidden.transform.localScale.y > HIDE_SCALE_FACTOR + 0.15);
            worldUpHidden.transform.position = new Vector3(UPPER_POINT.x, worldUpHidden.transform.position.y, UPPER_POINT.z);
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
            }
        }
    }

    void goToCenter(GameObject worldToMove)
    {
        if (worldToMove == null) return;
        worldToMove.SetActive(true);
        worldToMove.transform.localScale = Vector3.one * START_SCALE_FACTOR;
        changeColorChildren(worldToMove, START_COLOR);
        worldToMove.transform.position = START_POINT;
    }

    void goToUpper(GameObject worldToMove)
    {
        if (worldToMove == null) return;
        worldToMove.SetActive(true);
        worldToMove.transform.localScale = Vector3.one * UPPER_SCALE_FACTOR;
        changeColorChildren(worldToMove, UPPER_COLOR);
        worldToMove.transform.position = UPPER_POINT;
    }

    void hideLower(GameObject worldToMove)
    {
        if (worldToMove == null) return;
        worldToMove.SetActive(false);
        worldToMove.transform.localScale = Vector3.one * LOWER_SCALE_FACTOR;
        changeColorChildren(worldToMove, HIDE_COLOR_DOWN);
        worldToMove.transform.position = LOWER_POINT;
    }

    void hideUpper(GameObject worldToMove)
    {
        if (worldToMove == null) return;
        worldToMove.SetActive(false);
        worldToMove.transform.localScale = Vector3.one * HIDE_SCALE_FACTOR;
        changeColorChildren(worldToMove, HIDE_COLOR_UP);
        worldToMove.transform.position = UPPER_POINT;
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

/*
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
        }*/
