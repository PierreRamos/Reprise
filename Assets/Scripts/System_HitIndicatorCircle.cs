using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_HitIndicatorCircle : MonoBehaviour
{
    [SerializeField]
    private GameEvent onCirclePairDestroy;

    private Vector3 targetPosition;

    private Vector3 startingPosition;

    private float elapsedTime;

    private float desiredTimeToDestination = 0.425f;

    private bool canMove;

    private void Start()
    {
        startingPosition = transform.localPosition;
        StartCoroutine(CountdownTimer());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // float percentageComplete = elapsedTime / sampleHitTime;
        float percentageComplete = elapsedTime / desiredTimeToDestination;
        transform.position =
            Vector3.Lerp(startingPosition, targetPosition, percentageComplete);
    }

    IEnumerator CountdownTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (startingPosition.x < targetPosition.x)
        {
            onCirclePairDestroy.Raise(this, null);
        }
    }

    public void CreateCircleIndicator(Component sender, object data)
    {
        targetPosition = (Vector3) data;
    }
}
