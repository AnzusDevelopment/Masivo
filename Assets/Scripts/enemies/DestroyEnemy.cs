using UnityEngine;
using System.Collections;

public class DestroyEnemy : MonoBehaviour {

    private static readonly Vector4 LEVEL_MAP_BOUNDS = new Vector4(8.1f, 3.63f, -6.7f, -2.22f);

    public float duration;
    public int score = 10;
    public int maxScore = 10;
    public bool duplicate;
    public GameObject blast;
    public GameObject enemyChild;
    public InstantiateEnemy instantiateEnemy;
	
	void Start () {
        instantiateEnemy = GameObject.Find("backWorld").GetComponent<InstantiateEnemy>();
        StartCoroutine(destroyEnemy());
	}

    void Update()
    {
        Vector3 objectPos = gameObject.transform.position;
        if ( objectPos.y > LEVEL_MAP_BOUNDS.x || objectPos.x > LEVEL_MAP_BOUNDS.y || objectPos.y < LEVEL_MAP_BOUNDS.z || objectPos.x < LEVEL_MAP_BOUNDS.z)
        {
            instantiateEnemy.enemiesLeft = instantiateEnemy.enemiesLeft - 1;
            Destroy(gameObject);
        }
    }

    IEnumerator destroyEnemy()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

}
