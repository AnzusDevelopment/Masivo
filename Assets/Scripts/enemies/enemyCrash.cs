using UnityEngine;
using System.Collections;

public class EnemyCrash : MonoBehaviour {

    private const int LAYER_FIELD = 9;
    private const int LAYER_CITY = 10;

    public GameObject fieldBlast;
    public GameObject cityBlast;

    public bool cityHarmless = false; 
    public int damage;

    public InstantiateEnemy instantiateEnemy;

    void Start () {
        instantiateEnemy = GameObject.Find("backWorld").GetComponent<InstantiateEnemy>();
    }

    void Update(){

    }

    void OnCollisionEnter2D(Collision2D collision){
        if ( collision.gameObject.layer == LAYER_FIELD ){
            GameObject.Find("backWorld").GetComponent<EnergyState>().damage(damage);
            Instantiate( fieldBlast, gameObject.GetComponent<Transform>().position + new Vector3( 0, -0.5f, 0 ), Quaternion.identity );
            if(!cityHarmless)
                instantiateEnemy.enemiesLeft = instantiateEnemy.enemiesLeft - 1;
            Destroy(gameObject);
        }
        else if ( collision.gameObject.layer == LAYER_CITY )
        {
            if (!cityHarmless)
            {
                Instantiate(cityBlast, gameObject.GetComponent<Transform>().position, Quaternion.identity);
                instantiateEnemy.enemiesLeft = instantiateEnemy.enemiesLeft - 1;
            }
            Destroy(gameObject);
        }
    }

}
