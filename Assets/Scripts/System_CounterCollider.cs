using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class System_CounterCollider : MonoBehaviour
{
    [SerializeField]
    private GameEvent onBulletAdd;

    [SerializeField]
    private GameEvent onBulletRemove;

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Bullet"))
        {
            onBulletAdd.Raise(this, otherGameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Bullet"))
        {
            onBulletRemove.Raise(this, otherGameObject);
        }
    }
}
