using UnityEngine;
using System.Collections;

public class RotateLoading : MonoBehaviour {

    public float speed;

    void Start()
    {
        speed /= 1000;
    }

    void Update () {
        gameObject.GetComponent<Transform>().Rotate(0, 0, speed / Time.fixedDeltaTime);
    }
}
