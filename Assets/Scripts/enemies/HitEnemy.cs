using UnityEngine;
using System.Collections;

public class HitEnemy : MonoBehaviour {

    public GameObject blast;
    public bool duplicate = false;
    public GameObject subEnemy;

    void OnMouseDown()
    {
        if (this.gameObject.transform.position.y < 4.1) { 
            Instantiate(blast, gameObject.GetComponent<Transform>().position, Quaternion.identity);
            if (duplicate)
            {
                GameObject clone;
                clone = Instantiate(subEnemy, gameObject.GetComponent<Transform>().position + new Vector3((float)0.15, 0, 0), Quaternion.identity) as GameObject;
                clone.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)0.01, (float)-0.05) / Time.fixedDeltaTime, ForceMode2D.Impulse);
                clone = Instantiate(subEnemy, gameObject.GetComponent<Transform>().position + new Vector3((float)-0.15, 0, 0), Quaternion.identity) as GameObject;
                clone.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)-0.01, (float)-0.05) / Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            scoreController.Instance.setScore(scoreController.Instance.getScore() + 10);
            Destroy(this.gameObject);
        }
    }
}
