using UnityEngine;
using System.Collections;

public class ScoreLabel : MonoBehaviour {

    public Font myFont;

    private GUIStyle guiStyle = new GUIStyle();

    void OnGUI()
    {
        guiStyle.font = myFont;
        guiStyle.normal.textColor = Color.white;
        guiStyle.fontSize = 20;
        guiStyle.alignment = TextAnchor.MiddleLeft;
        GUI.Label(new Rect(Screen.width - 150, 20, 70, 40), "Puntaje: ", guiStyle);
        guiStyle.alignment = TextAnchor.MiddleRight;
        GUI.Label(new Rect(Screen.width - 78, 20, 58, 40), ScoreController.Instance.getScore().ToString(), guiStyle);
    }
}