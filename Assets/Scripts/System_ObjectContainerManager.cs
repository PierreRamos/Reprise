using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class System_ObjectContainerManager : MonoBehaviour
{
    [SerializeField]
    private GameEvent onBulletCounterEnable;

    [SerializeField]
    private GameEvent onBulletCounterDisable;

    private List<GameObject> bulletContainer = new List<GameObject>();

    //Bullet list container
    public void AddToBulletContainer(Component sender, object data)
    {
        if (!bulletContainer.Contains((GameObject) data))
        {
            bulletContainer.Add((GameObject) data);
            onBulletCounterEnable.Raise(this, null);
        }
    }

    public void RemoveFromBulletContainer(Component sender, object data)
    {
        if (bulletContainer.Contains((GameObject) data))
        {
            bulletContainer.Remove((GameObject) data);
        }
        if (bulletContainer.Any() == false)
        {
            onBulletCounterDisable.Raise(this, null);
        }
    }
}
