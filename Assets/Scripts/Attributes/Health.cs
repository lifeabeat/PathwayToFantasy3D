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
        public UnityEvent onDie;
        
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        float health = -1;
        bool wasDead = false;

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
            return health <= 0;
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " Took Damage: " + damage);
        
            health = Mathf.Max(health - damage, 0);
            
            if (IsDead())
            {
                //UpdateState();
                onDie.Invoke();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
            UpdateState();
        }

        public void Heal(float healthToRestore)
        {
            health = Mathf.Min(health + healthToRestore, GetMaxHealthPoints());
            UpdateState();
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

        private void UpdateState()
        {
            if(!wasDead && IsDead())
            {
                GetComponent<Animator>().SetTrigger("isDead");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            if (wasDead && !IsDead())
            {
                GetComponent<Animator>().Rebind();
            }
            wasDead = IsDead();


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

    
                UpdateState();
            
        }


    }
}

