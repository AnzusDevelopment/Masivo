using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class InstantiateEnemy : MonoBehaviour {

    public static readonly float[] INITIAL_POSITIONS = { -2.18f, -1.09f, 0.02f, 1.11f, 2.19f };
    private static readonly float[] Z_RANGE = { -2.18f, -1.82f };
    private const float Y_START_POINT = 8.0f;

    public GameObject[] enemies;
    public int difficulty;
    public string playFabTitleId = "63E3";

    private float xPos, yPos;
    private Vector3 position;
    private float numEnemies;
    private float[] speeds;
    private float time;
    private List< GamePatterns.Tuple<int, float, int> > pattern;

    void Awake()
    {
        PlayFabSettings.TitleId = playFabTitleId;
        AuthenticateWithPlayFab();
        //PlayFabSettings.DeveloperSecretKey = "TBKPYIKJ8K4CAJJAWWHDZN3JHOPXEEETQ4UM48W6TOAKHWX3W5";
    }

    void AuthenticateWithPlayFab()
    {
        Debug.Log("Logging into PlayFab...");
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest() { TitleId = this.playFabTitleId, CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = false };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginCallback, OnApiCallError, null);
    }

    void OnLoginCallback(LoginResult result)
    {
        Debug.Log(string.Format("Login Successful. Welcome Player:{0}!", result.PlayFabId));
        Debug.Log(string.Format("Your session ticket is: {0}", result.SessionTicket));
        //GetInventory();
        //UnlockUI();
    }

    void OnApiCallError(PlayFabError err)
    {
        string http = string.Format("HTTP:{0}", err.HttpCode);
        string message = string.Format("ERROR:{0} -- {1}", err.Error, err.ErrorMessage);
        string details = string.Empty;

        if (err.ErrorDetails != null)
        {
            foreach (var detail in err.ErrorDetails)
            {
                details += string.Format("{0} \n", detail.ToString());
            }
        }

        Debug.LogError(string.Format("{0}\n {1}\n {2}\n", http, message, details));
    }

    void Start () {
        GamePatterns gamePatterns = new GamePatterns();
        pattern = gamePatterns.getPattern();
        time = 0;
        speeds = new float[enemies.Length];
        for (var i = 0; i < enemies.Length; i++)
            speeds[i] = (float)difficulty / 30;
	}

    void Update()
    {
        foreach(GamePatterns.Tuple<int, float, int> move in pattern)
        {
            if ( time < move.Time ) break;

            if ( time == move.Time )
            {
                xPos = INITIAL_POSITIONS[move.Line-1];
                position = new Vector3( xPos, Y_START_POINT, Random.Range(Z_RANGE[0], Z_RANGE[1]) );
                GameObject clone = Instantiate(enemies[move.Enemy], position, enemies[move.Enemy].transform.rotation) as GameObject;
                clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -speeds[move.Enemy]) / Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
        }
        time += 0.5f;
        if(time > pattern[pattern.Count-1].Time + 200)
        {
            Application.Quit();
        }
    }

}
