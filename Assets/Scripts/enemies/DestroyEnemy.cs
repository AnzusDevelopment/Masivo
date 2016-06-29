using UnityEngine;
using System.Collections;

public class DestroyEnemy : MonoBehaviour {

	public int duration;
	
	void Start () {
        StartCoroutine(destroyEnemy());
	}

    IEnumerator destroyEnemy()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
