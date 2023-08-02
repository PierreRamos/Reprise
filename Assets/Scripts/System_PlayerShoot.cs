using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private float playerShootCastTime;

    [SerializeField]
    private float playerShootCooldown;

    [SerializeField]
    private float playerShootMovementCooldown;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject empoweredBullet;

    [SerializeField]
    private Transform shootPoint;

    [SerializeField]
    private GameEvent onPlayerShootBullet;

    [SerializeField]
    private GameEvent onPlayerShootBulletCast;

    [SerializeField]
    private GameEvent onPlayerShootEmpoweredBullet;

    [SerializeField]
    private GameEvent onPlayerShootEmpoweredBulletCast;

    [SerializeField]
    private GameEvent onPlayerShootMovementCooldownFinish;

    private bool enable_PlayerShoot = true;

    private bool isFull_SpecialMeter;

    private bool isRunning_PlayerShootPenaltyTimer;

    private Coroutine current_PlayerShootPenaltyTimer;

    //Gets called to make player shoot
    public void PlayerShoot(Component sender, object data)
    {
        if (enable_PlayerShoot)
        {
            StartCoroutine(PlayerShootCast());
        }

        //Spawn player bullet after cast time and checks if special meter is full
        IEnumerator PlayerShootCast()
        {
            enable_PlayerShoot = false;

            //If special meter is full or not, decides which cast time to do
            if (isFull_SpecialMeter)
            {
                onPlayerShootEmpoweredBulletCast.Raise(this, null);
                yield return new WaitForSecondsRealtime(1.25f);
            }
            else
            {
                onPlayerShootBulletCast.Raise(this, null);
                yield return new WaitForSeconds(playerShootCastTime);
            }

            //Check if special meter is full and decides which bullet to spawn
            if (isFull_SpecialMeter)
            {
                onPlayerShootEmpoweredBullet.Raise(this, null);
                Instantiate(empoweredBullet, shootPoint.position, bullet.transform.rotation);
            }
            else
            {
                Instantiate(bullet, shootPoint.position, bullet.transform.rotation);
                onPlayerShootBullet.Raise(this, null);
            }
        }
    }

    //Called by OnSpecialMeterIsFull Game event which sets the special meter is full bool
    public void SetSpecialMeterIsFull(Component sender, object data)
    {
        isFull_SpecialMeter = (bool)data;
    }

    //Gets called when player shot bullet event
    public void PlayerShootPenalties()
    {
        StartCoroutine(PlayerShootCooldown(playerShootCooldown));
        StartCoroutine(PlayerShootMovementCooldown(playerShootMovementCooldown));

        //Timer before player can shoot again
        IEnumerator PlayerShootCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            enable_PlayerShoot = true;
        }

        //Delays player movement when player shoots
        IEnumerator PlayerShootMovementCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            onPlayerShootMovementCooldownFinish.Raise(this, null);
        }
    }
}
