using UnityEngine;

public class DestructorPower : MonoBehaviour {

    public InstantiateEnemy instantiateEnemy;

    void Start()
    {
        instantiateEnemy = GameObject.Find("backWorld").GetComponent<InstantiateEnemy>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.transform.tag.Equals("Enemy"))
        {
            Debug.Log("enemy destroyed");
            if (!col.gameObject.GetComponent<EnemyCrash>().cityHarmless)
            {
                GameObject blast = col.gameObject.GetComponent<DestroyEnemy>().blast;
                Vector3 position = col.gameObject.transform.position;
                Instantiate(blast, position, Quaternion.identity);
                instantiateEnemy.enemiesLeft = instantiateEnemy.enemiesLeft - 1;
            }
            Destroy(col.gameObject);
        }
    }
}
