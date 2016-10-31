using UnityEngine;
using System.Collections;

public class HitEnemy : MonoBehaviour {

    private int enemyCount;
    private int sametime;

    public InstantiateEnemy instantiateEnemy;

    void Start()
    {
        instantiateEnemy = GameObject.Find("backWorld").GetComponent<InstantiateEnemy>();
        enemyCount = 0;
        sametime = 0;
    }

    void Update()
    {
        Touch[] userTouches = Input.touches;
        foreach (Touch touch in userTouches)
        {
            if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Ended)
            {
                LayerMask layerMask = 1 << LayerMask.NameToLayer("Enemy");
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero, 50, layerMask);
                if(hit.transform != null)
                {
                    if (hit.transform.tag.Equals("Enemy"))
                    {
                        GameObject enemy = hit.transform.gameObject;
                        GameObject blast = enemy.GetComponent<DestroyEnemy>().blast;
                        Instantiate(blast, enemy.GetComponent<Transform>().position, Quaternion.identity);
                        bool duplicate = enemy.GetComponent<DestroyEnemy>().duplicate;
                        bool leftTrash = enemy.GetComponent<DestroyEnemy>().leftTrash;
                        int destroyScore = enemy.GetComponent<DestroyEnemy>().score;
                        if (duplicate)
                        {
                            GameObject enemyChild = enemy.GetComponent<DestroyEnemy>().enemyChild;
                            Vector3 enemyPosition = enemy.GetComponent<Transform>().position;
                            StartCoroutine(createChildEnemy(enemyPosition, enemyChild));
                            instantiateEnemy.enemiesLeft += 2;
                        }
                        if (leftTrash)
                        {
                            GameObject enemyChild = enemy.GetComponent<DestroyEnemy>().enemyChild;
                            Vector3 enemyPosition = enemy.GetComponent<Transform>().position;
                            Instantiate(enemyChild, enemyPosition, Quaternion.identity);
                        }
                        ScoreController.Instance.setScore(ScoreController.Instance.getScore() + destroyScore);
                        Destroy(enemy);
                        instantiateEnemy.enemiesLeft -= 1;
                        enemyCount++;
                    }
                }
            }
        }
        if(sametime == 8)
        {
            if (enemyCount > 2)
                ScoreController.Instance.setScore(ScoreController.Instance.getScore() + 10 * (enemyCount - 1));
            sametime = 0;
            enemyCount = 0;
        }
        sametime++;
    }

    IEnumerator createChildEnemy(Vector3 enemyPosition, GameObject enemyChild)
    {
        yield return new WaitForSeconds(0.1f);
        GameObject clone;
        int slot = System.Array.IndexOf(InstantiateEnemy.INITIAL_POSITIONS,enemyPosition.x);
        if (slot == -1) yield return 0;

        float xPosOriginLeft = slot > 0 ? InstantiateEnemy.INITIAL_POSITIONS[slot - 1] : 0;
        float xPosOriginRight = slot < 4 ? InstantiateEnemy.INITIAL_POSITIONS[slot + 1] : 0;

        if (xPosOriginLeft != 0)
        {
            clone = Instantiate(enemyChild, new Vector3(xPosOriginLeft, enemyPosition.y + 0.48f, 0), Quaternion.identity) as GameObject;
            clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -0.048f) / Time.fixedDeltaTime, ForceMode2D.Impulse);
        }

        if (xPosOriginRight != 0)
        {
            clone = Instantiate(enemyChild, new Vector3(xPosOriginRight, enemyPosition.y + 0.48f, 0), Quaternion.identity) as GameObject;
            clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -0.048f) / Time.fixedDeltaTime, ForceMode2D.Impulse);
        } 
        
    }

}