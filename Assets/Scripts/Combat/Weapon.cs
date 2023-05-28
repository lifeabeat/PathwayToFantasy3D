using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG PathwayToFantasy/Weapon", order = 0)]
        public class Weapon : ScriptableObject
        {
            [SerializeField] GameObject equippedPrefab = null;
            [SerializeField] AnimatorOverrideController animatorOverride = null;
            [SerializeField]  float weaponRange = 2f;
            [SerializeField]  float weaponDamage = 5f;
            [SerializeField] bool isRightHanded = true;
            [SerializeField] Projectiles projectiles = null;

            const string weaponName = "Weapon";

            public void Spawn(Transform rightHandTransform,Transform leftHandTransform, Animator animator)
            {
                DestroyOldWWeapon(rightHandTransform,leftHandTransform);
                if (equippedPrefab != null)
                {
                    Transform handTransform = GetHandsTransform(rightHandTransform, leftHandTransform);
                    GameObject weapon = Instantiate(equippedPrefab, handTransform);
                    weapon.name = weaponName;
                }
                if (animatorOverride != null)
               {
                    animator.runtimeAnimatorController = animatorOverride;
               }
            }

        private void DestroyOldWWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHandTransform.Find(weaponName);
            } 
            if(oldWeapon == null) return;
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

            public void LaunchProjectiels(Transform rightHand, Transform leftHand, Health target)
            {
                Projectiles projectilesInstance = Instantiate(projectiles, GetHandsTransform(rightHand,leftHand).position,Quaternion.identity);
                projectilesInstance.SetTarget(target, weaponDamage);
            }
            public float WeaponDamage()
            {
                 return weaponDamage;
            }

            public float WeaponRange()
            {
                return weaponRange;
            }
            
    
        }
}