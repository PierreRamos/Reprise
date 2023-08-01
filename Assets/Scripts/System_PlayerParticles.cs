using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_PlayerParticles : MonoBehaviour
{
    [SerializeField]
    private float sparkDistanceFromPlayer;

    [SerializeField]
    private GameObject parrySparksParticle;

    [SerializeField]
    private GameObject perfectParrySparksParticle;

    public void SpawnParrySparks(Component sender, object data)
    {
        Vector3 playerPosition = transform.position;
        Quaternion playerRight = Quaternion.LookRotation(transform.right);
        Instantiate(parrySparksParticle,
        playerPosition + new Vector3(sparkDistanceFromPlayer, 0f, 0f),
        playerRight);
        // Instantiate(parrySparksParticle,
        // playerPosition + new Vector3(sparkDistanceFromPlayer, 0f, 0f),
        // transform.rotation);
    }

    public void SpawnPerfectParrySparks(Component sender, object data)
    {
        Vector3 playerPosition = transform.position;
        Instantiate(perfectParrySparksParticle,
        playerPosition + new Vector3(sparkDistanceFromPlayer, 0f, 0f),
        Quaternion.LookRotation(transform.right));
    }
}
