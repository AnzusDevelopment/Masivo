using UnityEngine;
using System.Collections;

public class DestroyEnemy : MonoBehaviour {

	public int duration;
    public int score = 10;
    public bool duplicate;
    public GameObject blast;
    public GameObject enemyChild;
	
	void Start () {
        StartCoroutine(destroyEnemy());
	}

    IEnumerator destroyEnemy()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
