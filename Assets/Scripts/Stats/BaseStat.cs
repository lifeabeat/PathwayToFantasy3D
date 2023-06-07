using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStat : MonoBehaviour
    {
        [Range(1, 99)] 
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;

        [SerializeField] bool shouldUseModifier = false;
    
        
        public event Action onLevelUp;

        int currentLevel = 0;
        Experience experience;

        private void Awake() {
            
            experience = GetComponent<Experience>();    
        }
        private void Start() 
        {    
            currentLevel = CalculateLevel();
        }

        private void OnEnable() {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable() {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }
        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAddictiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        
        private float GetBaseStat(Stat   stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if( currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        private float GetAddictiveModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            foreach (IModiferProvider provider in GetComponents<IModiferProvider>())
            {
                foreach ( float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            foreach (IModiferProvider provider in GetComponents<IModiferProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifer(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }


        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if(experience == null ) return startingLevel;

            float currentXP = experience.GetPoint();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelup = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelup >= currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel +1;
        }
    }
}


