using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;
        

        public void SpawnText(float damageAmount)
        {
            DamageText instance = Instantiate<DamageText>(damageTextPrefab,transform);
            instance.SetText(damageAmount);
        }

    }
}
