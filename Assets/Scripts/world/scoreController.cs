using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

    private int score = 0;
    private static ScoreController instance;

    static public bool isActive
    {
        get
        {
            return instance != null;
        }
    }

    public static ScoreController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType<ScoreController>();
                if (instance == null)
                {
                    GameObject container = new GameObject("_scorecontroller");
                    DontDestroyOnLoad(container);
                    instance = container.AddComponent<ScoreController>();
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

