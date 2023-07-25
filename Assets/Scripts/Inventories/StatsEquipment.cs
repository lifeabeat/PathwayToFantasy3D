using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using GameDev.Inventories;

namespace RPG.Inventories
{
    public class StatsEquipment : Equipment, IModiferProvider
    {
        IEnumerable<float> IModiferProvider.GetAdditiveModifier(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModiferProvider;
                if (item == null) continue;
                foreach (float modifier in item.GetAdditiveModifier(stat))
                {
                    yield return modifier;
                }
            }
        }

        IEnumerable<float> IModiferProvider.GetPercentageModifer(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModiferProvider;
                if (item == null) continue;
                foreach (float modifier in item.GetPercentageModifer(stat))
                {
                    yield return modifier;
                }
            }
        }
    }
}
