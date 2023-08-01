using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_SpecialMeter : MonoBehaviour
{
    [SerializeField]
    private float maxSpecial;

    [SerializeField]
    private float specialMeterGraceDuration;

    [SerializeField]
    private GameEvent onSpecialBarUpdateUI;

    [SerializeField]
    private GameEvent onSpecialMeterIsFull;

    private float specialValue;

    private bool specialMeterIsFull;

    private bool specialMeterHasGrace;

    private bool isRunning_SpecialMeterGracePeriod;

    private Coroutine current_SpecialMeterGracePeriod;

    private void Start()
    {
        specialValue = 0;
    }

    private void Update()
    {
        //Updates every frame and checks if special meter is full
        onSpecialMeterIsFull.Raise(this, specialMeterIsFull);

        //Updates special meter every frame
        onSpecialBarUpdateUI.Raise(this, specialValue);
    }

    public void UpdateSpecialValue(int value)
    {
        //Special value system
        if (!specialMeterHasGrace)
        {
            if (specialValue + value < 0)
            {
                specialValue = 0;
            }
            else if (specialValue + value > maxSpecial)
            {
                specialValue = maxSpecial;
            }
            else
            {
                specialValue += value;
            }
        }

        //Checks if special value is full
        if (specialValue == maxSpecial)
        {
            specialMeterIsFull = true;
            if (!isRunning_SpecialMeterGracePeriod)
            {
                current_SpecialMeterGracePeriod =
                    StartCoroutine(SpecialMeterGracePeriod());
            }
        }
        else
        {
            specialMeterIsFull = false;
        }
    }

    //Gets called by OnPlayerShootEmpoweredBullet which resets special meter to 0
    public void ShotPlayerEmpoweredBullet()
    {
        if (isRunning_SpecialMeterGracePeriod)
        {
            StopCoroutine (current_SpecialMeterGracePeriod);
            isRunning_SpecialMeterGracePeriod = false;
            specialMeterHasGrace = false;
        }
        specialValue = 0f;
        specialMeterIsFull = false;
    }

    //Timer before the special meter to be decreased again after being full
    IEnumerator SpecialMeterGracePeriod()
    {
        isRunning_SpecialMeterGracePeriod = true;
        specialMeterHasGrace = true;
        yield return new WaitForSeconds(specialMeterGraceDuration);
        specialValue = 0f;
        specialMeterHasGrace = false;
        isRunning_SpecialMeterGracePeriod = false;
    }
}
