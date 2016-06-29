using UnityEngine;
using System.Collections;

public class EnergyState : MonoBehaviour {

    private readonly string ENERGY_NAME = "energyBar_1";

    public GameObject energyBar;
    public int maxEnergy, startEnergy;
    private int energyLeft;
    private int timeField = 0;
    private bool blink = false;
    private static EnergyState instance;

    static public bool isActive
    {
        get
        {
            return instance != null;
        }
    }

    public static EnergyState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType<EnergyState>();
                if (instance == null)
                {
                    GameObject container = new GameObject("_energystate");
                    DontDestroyOnLoad(container);
                    instance = container.AddComponent<EnergyState>();
                }
            }
            return instance;
        }
    }

    void Start()
    {  
        GameObject.Find( ENERGY_NAME ).GetComponent<Transform>().localScale = new Vector3(1, 1.0f, 1);
        energyLeft = startEnergy;
        GameObject.Find("energyBar").GetComponent<Transform>().localScale = new Vector3(0.97F * energyLeft / maxEnergy, 0.75F, 1);
    }

    void Update()
    {
        if ( blink == true && GameObject.Find("field") != null )
        {
            if (timeField % 5 == 0 && timeField > 0)
                GameObject.Find("field").GetComponent<SpriteRenderer>().color = new Color(167 / 255F, 248 / 255F, 1F, 229 / 255F);
            else
                GameObject.Find("field").GetComponent<SpriteRenderer>().color = new Color(167 / 255F, 248 / 255F, 1F, 120 / 255F);
            timeField++;
            if (timeField == 51)
            {
                blink = false;
                timeField = 0;
            }
        }
    }

    public void damage(int amount)
    {
        if (energyLeft - amount > 0) {
            energyLeft = energyLeft - amount;
            blinkField();
        }
        else
        {
            Destroy(GameObject.Find("field"));
            energyLeft = 0;
        }
        updateBar();
    }

    void updateBar()
    {
        GameObject.Find("energyBar").GetComponent<Transform>().localScale = new Vector3(0.97F * energyLeft / maxEnergy, 0.75F, 1);
    }

    void blinkField()
    {
        blink = true;
    }

    public float getEnergyLeft()
    {
        return this.energyLeft;
    }
}
