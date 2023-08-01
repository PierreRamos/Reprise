using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_HitIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject circle;

    [SerializeField]
    private Transform left;

    [SerializeField]
    private Transform right;

    [SerializeField]
    private GameEvent onRequestCreateCircleIndicator;

    private GameObject[] circlePair;

    private List<GameObject[]> listOfCirclePairs = new List<GameObject[]>();

    public void RequestCreateCircleIndicator(Component sender, object data)
    {
        GameObject leftCircle =
            Instantiate(circle, left.position, transform.rotation);
        GameObject rightCircle =
            Instantiate(circle, right.position, transform.rotation);
        onRequestCreateCircleIndicator.Raise(this, transform.position);

        circlePair = new GameObject[2];

        circlePair[0] = leftCircle;
        circlePair[1] = rightCircle;

        listOfCirclePairs.Add (circlePair);

        // listOfCirclePairs.Add (leftCircle);
        // listOfCirclePairs.Add (rightCircle);
    }

    public void PopListOfCirclePairs(Component sender, object data)
    {
        GameObject temp;

        for (int i = 0; i < 2; i++)
        {
            temp = listOfCirclePairs[0][i];
            Destroy (temp);
        }
        listOfCirclePairs.RemoveAt(0);
    }
}
