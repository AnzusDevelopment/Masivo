using UnityEngine;
using System.Collections;

public class EnemyCrash : MonoBehaviour {

    private const int LAYER_FIELD = 9;
    private const int LAYER_CITY = 10;

    public GameObject fieldBlast;
    public GameObject cityBlast;
    public int damage;

    void Start () {

    }

    void Update(){

    }

    void OnCollisionEnter2D(Collision2D collision){
        if ( collision.gameObject.layer == LAYER_FIELD ){
            GameObject.Find("backWorld").GetComponent<EnergyState>().damage(damage);
            Instantiate( fieldBlast, gameObject.GetComponent<Transform>().position + new Vector3( 0, -0.5f, 0 ), Quaternion.identity );
            Destroy(gameObject);
        }else if ( collision.gameObject.layer == LAYER_CITY )
        {
            Instantiate(cityBlast, gameObject.GetComponent<Transform>().position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
