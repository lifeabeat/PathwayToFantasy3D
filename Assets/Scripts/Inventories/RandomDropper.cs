using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;
using UnityEngine.AI;
using GameDev.Inventories;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        //Config data
        [Tooltip("How far can the pickups spawn from the dropper")]
        [SerializeField] float scatterDistance = 1f;
        [SerializeField] DropLibrary dropLibrary;
        // [SerializeField] int numberOfDrop = 2;
        //Constant
        const int ATTEMPTS = 30;

        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseStat>();
            
            // var item = dropLibrary[Random.Range(0, dropLibrary.Length)];
            var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
            foreach (var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }
        protected override Vector3 GetDropLocation()
        {
            // Try more than once to get on the NavMesh
            for ( int i = 0;  i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position; 
        }
    }
}
