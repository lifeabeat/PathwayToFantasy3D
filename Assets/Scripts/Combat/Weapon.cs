using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {   
        [SerializeField] UnityEvent onHit;
        [SerializeField] GameObject hitEffect = null;
        public void OnHit()
        {
            // print("Weapon HIT with melee :" + gameObject.name);
            SpawnSpecialEffect();
            onHit.Invoke();
            
        }

        private void SpawnSpecialEffect()
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, transform.rotation);
            }
        }

    
    }
}
