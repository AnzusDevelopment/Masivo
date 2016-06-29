using UnityEngine;
using System.Collections;

public class RotateEnemy : MonoBehaviour {

    public float speed;
    private float direction;

	// Use this for initialization
	void Start () {
        speed /= 1000;
        direction =  Random.Range(0, 2);
        gameObject.GetComponent<Transform>().Rotate( 0, 0, Random.Range( 0, 360 ) );
        if (direction == 1)
            speed *= -1;
    }
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Transform>().Rotate( 0, 0, speed / Time.fixedDeltaTime );
    }
}
