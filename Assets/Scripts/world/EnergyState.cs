﻿using UnityEngine;
using System.Collections;

public class EnergyState : MonoBehaviour {

    private readonly string ENERGY_NAME = "energyBar_1";

    public GameObject energyBar;
    public int maxEnergy, startEnergy;
    private int energyLeft;
    private int timeField = 0;
    private bool blink = false;
    private Vector3 localPosition;

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
        GameObject.Find("energyBar").GetComponent<Transform>().localScale = new Vector3(0.98F * energyLeft / maxEnergy, 0.75F, 1);
        localPosition = GameObject.Find("energyBar").GetComponent<Transform>().position;
    }

    void Update()
    {
        if ( blink == true && GameObject.Find("field") != null )
        {
            if (timeField % 5 == 0 && timeField > 0)
                GameObject.Find("field").GetComponent<SpriteRenderer>().color = new Color(0 / 255F, 202 / 255F, 1F, 60 / 255F);
            else
                GameObject.Find("field").GetComponent<SpriteRenderer>().color = new Color(0 / 255F, 202 / 255F, 1F, 25 / 255F);
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
        localPosition = new Vector3(localPosition.x, localPosition.y + 0.009f, localPosition.z);
        GameObject.Find("energyBar").GetComponent<Transform>().localScale = new Vector3(0.98f * energyLeft / maxEnergy, 0.75F, 1);
        GameObject.Find("energyBar").GetComponent<Transform>().position = localPosition;
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
