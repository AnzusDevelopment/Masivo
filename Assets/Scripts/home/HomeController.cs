using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomeController : MonoBehaviour
{

    public static readonly float[] POS_STARS_X = { -2.808f, -2.761f, -2.418f, -2.087f, -2.047f, -1.955f, -1.901f, -1.664f, -1.58f, -1.203f, -0.87f,
                                                   -0.342f, -0.059f, 0.175f, 1.0863f, 1.282f, 1.362f, 1.338f, 1.3945f, 1.935f };
    public static readonly float[] POS_STARS_Y = { 4.85f, 4.844f, 4.965f, 4.9f, 4.368f, 4.072f, 4.601f, 4.989f, 4.048f, 4.617f, 4.082f, 4.669f, 4.96f,
                                                   4.068f, 4.948f, 4.653f, 4.896f, 4.34f, 4.074f, 3.895f };
    public GameObject star;
    public GameObject bird;

    private int timeStar = 0;
    private int sizeStars = POS_STARS_X.Length;
    private float timeBird = 0;
    private int numberOfStars, numberOfBirds, starSelected;
    private List<GameObject> clonedStars = new List<GameObject>();
    private List<GameObject> clonedBirds = new List<GameObject>();

    private Util utilities;

    void Start()
    {
        numberOfBirds = (int)Random.Range(3, 5);
    }

    void Update()
    {
        if (timeStar % 6 == 0 || timeStar == 0)
        {
            numberOfStars = (int)Random.Range(1, 3);
            for(var i = 0; i < numberOfStars; i++)
            {
                starSelected = (int)Random.Range(0, sizeStars);
                GameObject clonedStar = Instantiate(star, new Vector3(POS_STARS_X[starSelected], POS_STARS_Y[starSelected], -0.5f), Quaternion.identity) as GameObject;
                float starScale = Random.Range(0.75f, 1.5f);
                clonedStar.transform.localScale = Vector3.one * starScale;
                clonedStars.Add(clonedStar);
            }
        }
             
        if(timeStar == 59)
        {
            foreach(GameObject clonedStar in clonedStars)
                Destroy(clonedStar);
            timeStar = 0;
            clonedStars.Clear();
        }
        if (timeBird % 100 == 0 && timeBird <= numberOfBirds * 100)
        {
            float yPos = Random.Range(0.23f, -1.78f);
            float scaleBird = yPos >= 0 ? 0.12f + ( (0.23f - yPos)/0.23f ) * 0.12f : 0.24f + (Mathf.Abs(yPos) / 1.78f) * 0.95f;
            float zPos = -2.2f - scaleBird / 1.19f * 2f;
            float animatorSpeed = Random.Range(1f, 1.3f);
            GameObject clonedBird = Instantiate(bird, new Vector3(0, yPos, zPos), Quaternion.identity) as GameObject;
            Vector3 scaleBirdLocal = clonedBird.transform.GetChild(0).transform.GetChild(0).transform.localScale;
            clonedBird.transform.GetChild(0).transform.GetChild(0).transform.localScale = scaleBirdLocal  * scaleBird;
            Animator animatorMovement = clonedBird.transform.GetChild(0).GetComponent<Animator>();
            Animator animatorBird = clonedBird.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();

            animatorMovement.speed = animatorSpeed;
            animatorBird.speed = animatorSpeed;
            clonedBirds.Add(clonedBird);
        }
        if (timeBird > 200)
        {
            foreach (GameObject bc in clonedBirds)
            {
                if(bc.transform.GetChild(0).transform.position.x < -3.29f)
                {
                    clonedBirds.Remove(bc);
                    Destroy(bc);
                }
            }
        }
        if (timeBird > 800)
        {
            numberOfBirds = (int)Random.Range(3, 5);
            timeBird = 0;
        }
            
        timeStar++;
        timeBird += 0.25f;
    }

}
