using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_BulletCounterTime : MonoBehaviour
{
    [SerializeField]
    private GameEvent onConfirmPerfectParryHit;

    [SerializeField]
    private GameEvent onConfirmParryHit;

    [SerializeField]
    private float parryWindowStart;

    [SerializeField]
    private float perfectParryWindowStart;

    private float currentTime;

    private bool canParry;

    private bool canPerfectParry;

    public bool parryIsHeld { private get; set; }

    private void Start()
    {
        currentTime = 0;
    }

    private void Update()
    {
        currentTime += 1 * Time.deltaTime;
        if (currentTime > parryWindowStart)
        {
            canParry = true;
            if (currentTime > perfectParryWindowStart)
            {
                canPerfectParry = true;
            }
        }
        else
        {
            canParry = false;
        }
        if (parryIsHeld)
        {
            ConfirmParryHit(this, transform.position);
        }
    }

    public void ConfirmParryHit(Component sender, object data)
    {
        if (canParry && canPerfectParry)
        {
            onConfirmPerfectParryHit.Raise(this, transform.position);
        }
        else if (canParry)
        {
            onConfirmParryHit.Raise(this, transform.position);
        }
    }
}
