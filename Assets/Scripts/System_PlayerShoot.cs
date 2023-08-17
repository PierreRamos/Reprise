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
    private Transform shootPointNormal;

    [SerializeField]
    private Transform shootPointEmpowered;

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
            enable_PlayerShoot = false;

            //If special meter is full or not, decides which cast time to do
            if (isFull_SpecialMeter)
            {
                onPlayerShootEmpoweredBulletCast.Raise(this, null);
            }
            else
            {
                onPlayerShootBulletCast.Raise(this, null);
            }
        }
    }

    //Instantiates player bullet
    public void PlayerBullet()
    {
        Instantiate(bullet, shootPointNormal.position, bullet.transform.rotation);
        onPlayerShootBullet.Raise(this, null);
    }

    //Instantiates player empowered bullet
    public void PlayerEmpoweredBullet()
    {
        onPlayerShootEmpoweredBullet.Raise(this, null);
        Instantiate(empoweredBullet, shootPointEmpowered.position, bullet.transform.rotation);
    }

    //Spawns player reversal attack projectile (Reversal Animation)
    public void PlayerReversalBullet()
    {
        Instantiate(bullet, shootPointNormal.position, bullet.transform.rotation);
    }

    //Called by OnSpecialMeterIsFull Game event which sets the special meter is full bool
    public void SetSpecialMeterIsFull(Component sender, object data)
    {
        isFull_SpecialMeter = (bool)data;
    }

    private bool isRunning_PlayerShootCooldown;

    //Gets called when player shot bullet event
    public void PlayerShootPenalties()
    {
        StartCoroutine(PlayerShootCooldown(playerShootCooldown));
        StartCoroutine(PlayerShootMovementCooldown(playerShootMovementCooldown));

        //Timer before player can shoot again
        IEnumerator PlayerShootCooldown(float time)
        {
            isRunning_PlayerShootCooldown = true;
            yield return new WaitForSeconds(time);
            enable_PlayerShoot = true;
            isRunning_PlayerShootCooldown = false;
        }

        //Delays player movement when player shoots
        IEnumerator PlayerShootMovementCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            onPlayerShootMovementCooldownFinish.Raise(this, null);
        }
    }

    public void PlayerShootInterrupted()
    {
        if (isRunning_PlayerShootCooldown == false)
        {
            enable_PlayerShoot = true;
        }
    }
}
