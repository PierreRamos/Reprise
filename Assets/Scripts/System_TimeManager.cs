using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_TimeManager : MonoBehaviour
{
    [SerializeField]
    private float slowTimeValue;

    private bool gradualTimeToNormal;

    private void Update()
    {
        if (gradualTimeToNormal)
        {
            Time.timeScale += 5f * Time.deltaTime;
            if (Time.timeScale >= 1)
            {
                Time.timeScale = 1f;
                gradualTimeToNormal = false;
            }
        }
    }

    public void SetNormalTime()
    {
        Time.timeScale = 1f;
    }

    //Called by those who want to slow time (ex. shoot player empowered bullet)
    public void SetSlowTime(float value)
    {
        Time.timeScale = slowTimeValue;
        // StartCoroutine(SlowTimeTimer(value));
    }

    public void GradualTimeToNormal(bool value)
    {
        gradualTimeToNormal = value;
        Time.timeScale = slowTimeValue * 2;
    }

    IEnumerator SlowTimeTimer(float value)
    {
        yield return new WaitForSecondsRealtime(value);
        SetNormalTime();
    }
}
