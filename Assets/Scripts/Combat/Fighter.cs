using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction,  ISaveable
    {
        
        [SerializeField] float  timeBetweenAttaack = 1.2f;
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;
        // End Atk Longtime ago, If let 0 it first need to run one time
        float timeSinceLastAttack = Mathf.Infinity; 
        Weapon currentWeapon = null;

        private void Awake() 
        {
            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
                
            }
            

        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttaack)
            {
                // This will trigger a Hit event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().SetTrigger("isAttack");
            GetComponent<Animator>().ResetTrigger("stopAttack");
        }

        // Animation Events
        void Hit()
        {
            if (target == null) return;
            float damage = GetComponent<BaseStat>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectiles())
            {
                currentWeapon.LaunchProjectiels(rightHandTransform,leftHandTransform,target, gameObject, damage);
            }
            else 
            {
                // target.TakeDamage(gameObject, currentWeapon.WeaponDamage());
               
                target.TakeDamage(gameObject, damage);
            }
           
        }

        void Shoot()
        {
            Hit();
        }


        private bool GetIsInRange()
        {
            return (Vector3.Distance(transform.position, target.transform.position) < currentWeapon.WeaponRange());
        }

        public bool CanAttack(GameObject combatTarget)
        {   
            if(combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
            
        }
        public void Attack(GameObject combatTarget)
        {
            
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("isAttack");
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        // Save weaponName in Current scene
        public object CaptureState()
        {
            return currentWeapon.name;
        }

        // Load a string weaponName from resources and Equip Weapon Founded
        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
 }
