using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regeneratePercentage = 90;
        float health = -1;

        bool isDead = false;

       private void Start() 
       {
            GetComponent<BaseStat>().onLevelUp += RegenerateHealth;
            // Iff health < 0 then the Restore State havent run
            if(health < 0 )
            {
                health = GetComponent<BaseStat>().GetStat(Stat.Health);
            }


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
            return 100*(health / GetComponent<BaseStat>().GetStat(Stat.Health));

        }

        private void Die()
        {
            if(isDead) return;
            isDead = true;
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

