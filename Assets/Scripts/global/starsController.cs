using UnityEngine;
using System.Collections.Generic;

public class starsController : MonoBehaviour {

    public static readonly float[] POS_STARS_X = { -3.03f, -3.086f, -2.888f, -2.9244f, -2.735f, -1.8661f, -1.687f, -1.916f, -1.643f, -1.881f, -0.8333f, 0.0685f, 0.0819f, 0.346f, 2.157f, 2.292f, 1.684f, 1.766f };
    public static readonly float[] POS_STARS_Y = { 1.767f, 0.463f, -1.326f, -2.3345f, -2.9662f, -3.2846f, -2.883f, -2.1562f, -1.453f, 0.559f, -2.3775f, -3.1297f, -1.9909f, 1.552f, 1.563f, -0.195f, -1.16f, -2.434f };
    public GameObject star;

    private int timeStar = 0;
    private int sizeStars = POS_STARS_X.Length;
    private int numberOfStars, numberOfBirds, starSelected;
    private List<GameObject> clonedStars = new List<GameObject>();

    private Util utilities;

    void Update()
    {
        if (timeStar % 6 == 0 || timeStar == 0)
        {
            numberOfStars = (int)Random.Range(1, 3);
            for (var i = 0; i < numberOfStars; i++)
            {
                starSelected = (int)Random.Range(0, sizeStars);
                GameObject clonedStar = Instantiate(star, new Vector3(POS_STARS_X[starSelected], POS_STARS_Y[starSelected], -0.5f), Quaternion.identity) as GameObject;
                float starScale = Random.Range(0.75f, 1.5f);
                clonedStar.transform.localScale = Vector3.one * starScale;
                clonedStars.Add(clonedStar);
            }
        }

        if (timeStar == 59)
        {
            foreach (GameObject clonedStar in clonedStars)
                Destroy(clonedStar);
            timeStar = 0;
            clonedStars.Clear();
        }
        timeStar++;
    }
}
