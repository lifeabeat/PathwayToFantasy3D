using System;
using System.Collections;
using System.Collections.Generic;
using GameDev.Inventories;
using RPG.Control;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

public class ItemCollector : MonoBehaviour, IAction
{
    Pickup target;
    public void Cancel()
    {
        target = null;
        GetComponent<Mover>().Cancel();
    }

    void Update()
    {
        if (target == null) return;
        if (!GetIsInRange(target.transform)) // have to add a "!" in front to move when NOT in range
        {
            GetComponent<Mover>().MoveTo(target.transform.position);
        }
        else
        {
            target.PickupItem();
            target = null;
        }
    }
    public void CollectItem(Pickup pickup)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        target = pickup;
    }

    private bool GetIsInRange(Transform targetTransform)
    {
        return Vector3.Distance(transform.position, targetTransform.position) < 3;
    }

}
