using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDev.Inventories;
using RPG.Stats;

namespace RPG.Inventories
{
       [CreateAssetMenu(menuName = ("RPG PathwayToFantasy/Inventory/Equipable Item"))]
       public class StatsEquipableItem : EquipableItem, IModiferProvider
       {
          [SerializeField]
          Modifier[] additiveModifier;
          [SerializeField]
          Modifier[] percentageModifiers;

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            foreach (var modifier in additiveModifier)
            {
               if ( modifier.stat == stat)
               {
                    yield return modifier.value;
               }
            }
        }

        public IEnumerable<float> GetPercentageModifer(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        [System.Serializable]
          struct Modifier
          {
               public Stat stat;
               public float value;
          }
          

       }
}
