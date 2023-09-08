using System.Collections;
using System.Collections.Generic;
using GameDev.Inventories;
using RPG.Attributes;
using UnityEngine;

[CreateAssetMenu(menuName = ("RPG PathwayToFantasy/Potion"))]
public class HealthPotion : ActionItem
{
    [SerializeField] float healAmount = 20f;

    public override void Use(GameObject user)
    {
        Health health = user.GetComponent<Health>();
        if (!health) return;
        health.Heal(healAmount);
        
    }
}