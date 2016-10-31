using UnityEngine;
using System.Collections;

public class RotateEnemy : MonoBehaviour {

    public float speed;
    public float speedDisc;
    public bool newtonDisc = false;
    private float direction;
    private float speedChange = 0;

	void Start () {
        speed /= 1000;
        direction =  Random.Range(0, 2);
        gameObject.GetComponent<Transform>().Rotate( 0, 0, Random.Range( 0, 360 ) );
        
        if (direction == 1)
            speed *= -1;
    }
	
	void Update () {
        gameObject.GetComponent<Transform>().Rotate( 0, 0, speed / Time.fixedDeltaTime );
        if (speedChange == 26)
            speedChange = 0;

        if (newtonDisc)
        {
            if (speedChange < 20)
                gameObject.GetComponent<Transform>().Rotate(0, 0, speedDisc / Time.fixedDeltaTime);
            else
                gameObject.GetComponent<Transform>().Rotate(0, 0, 0 / Time.fixedDeltaTime);
        }
        speedChange += 0.25f;
    }
}
