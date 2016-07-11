using UnityEngine;
using System.Collections;

public class zoomToLevel : MonoBehaviour {

	void Update ()
    {
        var objectCamera = GameObject.Find("Main Camera");
        var mainCamera = objectCamera.GetComponent<Camera>();
        var positionCamera = objectCamera.transform.position;

        if(mainCamera.orthographicSize > 4.35)
        {
            mainCamera.orthographicSize -= 0.01f;
            objectCamera.transform.position = new Vector3(positionCamera.x, 0.01f + positionCamera.y, positionCamera.z);
        }

        //if (positionCamera.y < 0.73)
        //    objectCamera.transform.position = new Vector3(positionCamera.x, 0.01f + positionCamera.y, positionCamera.z);
    }
}
