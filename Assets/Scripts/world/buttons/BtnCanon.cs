using UnityEngine;
using System.Collections;

public class BtnCanon : MonoBehaviour {

    public GameObject[] points;
    public GameObject bullet; 

    void OnMouseDown()
    {
        if( EnergyState.Instance.getEnergyLeft() <= 0)
        {
            GameObject clone = Instantiate(bullet, points[0].GetComponent<Transform>().position, points[0].GetComponent<Transform>().rotation) as GameObject;
            clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 0.05F) / Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }
}
