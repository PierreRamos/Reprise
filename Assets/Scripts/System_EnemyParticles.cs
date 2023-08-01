using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_EnemyParticles : MonoBehaviour
{
    [SerializeField]
    private GameObject parrySparksParticle;

    [SerializeField]
    private GameObject perfectParrySparksParticle;

    public void SpawnParrySparks(Component sender, object data)
    {
        Instantiate(parrySparksParticle,
        transform.position + (transform.right + new Vector3(0.5f, 0, 0)),
        Quaternion.LookRotation(-((Transform) data).right));
    }

    public void SpawnPerfectParrySparks(Component sender, object data)
    {
        Instantiate(perfectParrySparksParticle,
        (Vector3) data,
        perfectParrySparksParticle.transform.rotation);
    }
}
