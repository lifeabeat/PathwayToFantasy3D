using System;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using GameDev.Inventories;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG PathwayToFantasy/Weapon", order = 0)]
        public class WeaponConfig : EquipableItem
        {
            [SerializeField] Weapon equippedPrefab = null;
            [SerializeField] AnimatorOverrideController animatorOverride = null;
            [SerializeField]  float weaponRange = 2f;
            [SerializeField]  float weaponDamage = 5f;
            [SerializeField]  float percentageBounus = 0f;
            [SerializeField] bool isRightHanded = true;
            [SerializeField] Projectiles projectiles = null;

            const string weaponName = "Weapon";

            public Weapon Spawn(Transform rightHandTransform,Transform leftHandTransform, Animator animator)
            {
                DestroyOldWWeapon(rightHandTransform,leftHandTransform);

                Weapon weapon = null;

                if (equippedPrefab != null)
                {
                    Transform handTransform = GetHandsTransform(rightHandTransform, leftHandTransform);
                    weapon = Instantiate(equippedPrefab, handTransform);
                    weapon.gameObject.name = weaponName;
                }
                // Nesting To Fix error not override in case forgot to add/ misaken animator override / Logic error
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if (animatorOverride != null)
               {
                    animator.runtimeAnimatorController = animatorOverride;
               } else if (overrideController != null)
               {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
               }

               return weapon;
            }

        private void DestroyOldWWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHandTransform.Find(weaponName);
            } 
            if(oldWeapon == null) return;
            //Change name to fix error confusew which is old/new weapon to delete
            oldWeapon.name = "Destroying";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHandsTransform(Transform rightHandTransform, Transform leftHandTransform)
            {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHandTransform;
            }
            else
            {
                handTransform = leftHandTransform;
            }

            return handTransform;
            }

            public bool HasProjectiles()
            {
                return projectiles != null;
            }

            public void LaunchProjectiels(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculateDamage)
            {
                Projectiles projectilesInstance = Instantiate(projectiles, GetHandsTransform(rightHand,leftHand).position,Quaternion.identity);
                projectilesInstance.SetTarget(target, instigator, calculateDamage);
            }
            public float WeaponDamage()
            {
                 return weaponDamage;
            }

            public float GetPercentageBounus()
            {
                return percentageBounus;
            }
            public float WeaponRange()
            {
                return weaponRange;
            }
            
    
        }
}