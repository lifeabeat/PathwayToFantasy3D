using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regeneratePercentage = 90;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;
        
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        float health = -1;
        bool isDead = false;

       private void Start() 
       {
            // If health < 0 then the Restore State havent run
            if(health < 0 )
            {
                health = GetComponent<BaseStat>().GetStat(Stat.Health);
            }
       }
       private void OnEnable() {
            GetComponent<BaseStat>().onLevelUp += RegenerateHealth;
       }
       private void OnDisable() {
            GetComponent<BaseStat>().onLevelUp -= RegenerateHealth;
       }

        

        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " Took Damage: " + damage);
        
            health = Mathf.Max(health - damage, 0);
            
            if (health == 0)
            {
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public float GetHealthPoints()
        {
            return health;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStat>().GetStat(Stat.Health);
        }

       

        public float GetPercentage()
        {
            return 100 * GetHealthValueFraction();
        }

        public float GetHealthValueFraction()
        {
            return (health / GetComponent<BaseStat>().GetStat(Stat.Health));
        }

        private void Die()
        {
            if(isDead) return;
            isDead = true;
            onDie.Invoke();
            GetComponent<Animator>().SetTrigger("isDead");
            GetComponent<ActionScheduler>().CancelCurrentAction();  
            
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStat>().GetStat(Stat.ExperienceReward));
            
        }

        private void RegenerateHealth()
        {
            float regenerateHealthPoints = GetComponent<BaseStat>().GetStat(Stat.Health) * (regeneratePercentage / 100);
            health = Mathf.Max(health,regenerateHealthPoints);
        }



        public object CaptureState()
        {
            return health;
        }


        public void RestoreState(object state)
        {
            // Casting 
            health = (float)state;

            if (health == 0)
            {
                Die();
            }
        }


    }
}

