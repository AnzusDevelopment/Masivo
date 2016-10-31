using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

    public GameObject soundBar;
    public GameObject musicBar;

    public int soundVolume = 100;
    public int musicVolume = 100;

    private LayerMask layerMask;
    private GameObject button;
    private RaycastHit2D raycast;

    private float soundBarScale = 0.55f;
    private float musicBarScale = 0.55f;
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            layerMask = 1 << LayerMask.NameToLayer("ButtonSettings");
            raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 50, layerMask);
            if (raycast.transform != null)
            {
                if (raycast.transform.tag.Equals("ButtonSettings"))
                    this.gameObject.SetActive(false);

                if (raycast.transform.tag.Equals("soundUp"))
                {
                    if (soundVolume < 100)
                        soundVolume += 20;
                }
                if (raycast.transform.tag.Equals("soundDown"))
                {
                    if (soundVolume > 0)
                        soundVolume -= 20;
                }
                if (raycast.transform.tag.Equals("musicUp"))
                {
                    if (musicVolume < 100)
                        musicVolume += 20;
                }
                if (raycast.transform.tag.Equals("musicDown"))
                {
                    if (musicVolume > 0)
                        musicVolume -= 20;
                }
                soundBar.transform.localScale = new Vector3(soundBarScale * soundVolume / 100, 1, 1);
                musicBar.transform.localScale = new Vector3(musicBarScale * musicVolume / 100, 1, 1);
            }
        }
    }
}
