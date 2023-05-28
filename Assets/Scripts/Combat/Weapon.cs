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

            public void Spawn(Transform rightHandTransform,Transform leftHandTransform, Animator animator)
            {
                if (equippedPrefab != null)
            {
                Transform handTransform;
                handTransform = GetHandsTransform(rightHandTransform, leftHandTransform);
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride != null)
               {
                    animator.runtimeAnimatorController = animatorOverride;
               }
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