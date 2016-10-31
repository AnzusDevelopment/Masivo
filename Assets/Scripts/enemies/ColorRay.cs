using UnityEngine;
using System.Collections;

public class ColorRay : MonoBehaviour {

    public GameObject ray;
    public GameObject newBlast;
    public InstantiateEnemy instantiateEnemy;

    void Start()
    {
        instantiateEnemy = GameObject.Find("backWorld").GetComponent<InstantiateEnemy>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.transform.tag.Equals("Enemy") && col.gameObject.name.StartsWith("asteroid5"))
        {
            Vector3 position = Vector3.up * 0.25f + gameObject.transform.position;
            GameObject clone = Instantiate(ray, position, Quaternion.identity) as GameObject;
            clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -0.18f) / Time.fixedDeltaTime, ForceMode2D.Impulse);
            instantiateEnemy.enemiesLeft = instantiateEnemy.enemiesLeft + 1;
            Destroy(gameObject);
        }
        if (col.gameObject.transform.tag.Equals("Enemy") && col.gameObject.name.StartsWith("asteroid8"))
        {
            Vector3 position = Vector3.up * -0.2f + col.transform.position;
            Quaternion rotation = col.transform.rotation;
            GameObject clone = Instantiate(ray, position, rotation) as GameObject;
            clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, col.GetComponent<Rigidbody2D>().velocity.y * 0.75f), ForceMode2D.Impulse);
            col.gameObject.GetComponent<DestroyEnemy>().blast = newBlast;
        }
    }

}
