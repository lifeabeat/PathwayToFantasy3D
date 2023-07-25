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
        //Constant
        const int ATTEMPTS = 30;
        protected override Vector3 GetDropLocation()
        {
            // Try more than once to get on the NavMesh
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return transform.position;
        }
    }
}
