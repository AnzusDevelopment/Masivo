using UnityEngine;
using System.Collections;

public class InstantiateEnemy : MonoBehaviour {

    private const float RANGE_XMAX = 2.4f;
	private const float RANGE_XMIN = -2.4f;
	private const float RANGE_YMAX = 11.0f;
	private const float RANGE_YMIN = 8.0f;
	
	public GameObject[] enemies;
    public float[] speedRanges;
    public float[] frecuency;
    public float startWait;    

    private float xPos, yPos, zPos;
    private Vector3 position;
    private float[] speed;
    private float velocityValue;
    private float numEnemies;
    private int dificulty;

    void Start () {
        speed = new float[enemies.Length];
        for ( int i = 0; i < enemies.Length; i++)
        {
            velocityValue = speedRanges[2 * i] + ( speedRanges[2 * i + 1] - speedRanges[2 * i] ) / 10;
            speed[i] = velocityValue / 100;
        }	
		zPos = -2;
        for (int i = 0; i < enemies.Length; i++)
            StartCoroutine(generateEnemies(i));
	}
	
    IEnumerator generateEnemies(int enemy)
    {
        dificulty = enemies[enemy].GetComponent<enemyCrash>().dificulty;
        if (dificulty < 2)
            yield return new WaitForSeconds(startWait);
        else
            yield return new WaitForSeconds(startWait + frecuency[enemy]);
        while (true)
        {
            if (dificulty < 2)
                numEnemies = Random.Range(1,3);
            else
                numEnemies = 1;
            for (int j = 0; j < numEnemies; j++)
            {
                xPos = Random.Range(RANGE_XMIN, RANGE_XMAX);
                yPos = Random.Range(RANGE_YMIN, RANGE_YMAX);
                position = new Vector3(xPos, yPos, zPos);
                GameObject clone = Instantiate(enemies[enemy], position, enemies[enemy].transform.rotation) as GameObject;
                clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -speed[enemy]) / Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(frecuency[enemy]);
        }
    }
}
