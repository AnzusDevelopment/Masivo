using UnityEngine;
using System.Collections;

public class scoreController : MonoBehaviour {

    private int score = 0;
    private static scoreController instance;

    static public bool isActive
    {
        get
        {
            return instance != null;
        }
    }

    public static scoreController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType<scoreController>();
                if (instance == null)
                {
                    GameObject container = new GameObject("_scorecontroller");
                    DontDestroyOnLoad(container);
                    instance = container.AddComponent<scoreController>();
                }
            }
            return instance;
        }
    }

    void Start () {
	
	}
	
	void Update () {
	
	}

    public void setScore( int score )
    {
        this.score = score;
    }

    public int getScore()
    {
        return this.score;
    }
}

