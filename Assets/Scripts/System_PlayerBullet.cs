using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerBullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private float maxBulletVelocity;

    [SerializeField]
    private float empoweredBulletStunTime;

    [SerializeField]
    private GameEvent onPlayerBulletEnterEnemy;

    [SerializeField]
    private GameEvent onPlayerEmpoweredBulletEnterEnemy;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rigid.velocity.x < maxBulletVelocity)
        {
            rigid.AddForce(Vector3.right * bulletSpeed, ForceMode.Impulse);
        }
        else
        {
            rigid.velocity =
                new Vector3(maxBulletVelocity,
                    rigid.velocity.y,
                    rigid.velocity.z);
        }
    }

    //When player bullet enters enemy collider; If bullet is empowered, guaranteed hit to enemy
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (gameObject.CompareTag("EmpoweredBullet"))
            {
                onPlayerEmpoweredBulletEnterEnemy
                    .Raise(this, empoweredBulletStunTime);
            }
            else
            {
                onPlayerBulletEnterEnemy.Raise(this, transform);
            }
            Destroy(this.gameObject);
        }
    }

    public void DestroyPlayerBullet()
    {
        Destroy(this.gameObject);
    }
}
