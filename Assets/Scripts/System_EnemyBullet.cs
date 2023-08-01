using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private GameEvent onBulletDestroy;

    [SerializeField]
    private GameEvent onBulletHitPlayer;

    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private float beforeHitTime;

    private float elapsedTime;

    private float damageValue;

    private bool canHitPlayer = true;

    private Vector3 startingPosition;

    private Vector3 playerPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (playerPosition != null)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / beforeHitTime;
            transform.position =
                Vector3
                    .Lerp(startingPosition,
                    playerPosition,
                    curve.Evaluate(percentageComplete));
            if (transform.position == playerPosition)
            {
                ConfirmHitPlayer();
            }
        }
    }

    //Sets the damage value of the bullet
    public void SetBulletDamage(Component sender, object data)
    {
        if (sender is System_Enemy)
        {
            damageValue = (float) data;
        }
    }

    private void ConfirmHitPlayer()
    {
        if (canHitPlayer)
        {
            // print (elapsedTime); //Debug for time to reach bullet to player
            onBulletHitPlayer.Raise(this, damageValue);
            onBulletDestroy.Raise(this, this.gameObject);
            Destroy (gameObject);
        }
    }

    public void DestroyBullet(Component sender, object data)
    {
        canHitPlayer = false;
        onBulletDestroy.Raise(this, this.gameObject);
        Destroy (gameObject);
    }

    public void UpdatePlayerPosition(Component sender, object data)
    {
        playerPosition = ((Vector3) data) + new Vector3(0.2f, 0f, 0f);
    }
}
