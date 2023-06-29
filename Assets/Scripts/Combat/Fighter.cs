using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Attributes;
using RPG.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction,  ISaveable, IModiferProvider
    {
        
        [SerializeField] float  timeBetweenAttaack = 1.2f;
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        Health target;
        // End Atk Longtime ago, If let 0 it first need to run one time
        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeapon = null;
        UtilsValue<Weapon> typeOfCurrentWeapon;

        private void Awake() 
        {
            // if(currentWeapon == null)
            // {
            //     EquipWeapon(defaultWeapon);
            // }
            currentWeapon = defaultWeapon;
            typeOfCurrentWeapon = new UtilsValue<Weapon>(SetupDefaultWeapon);

        }
        public Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start() {

            // May overridden the default weapon when we do restore
            typeOfCurrentWeapon.ForceInitialize();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetIsInRange(target.transform))
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
            
            if (typeOfCurrentWeapon.value != null)
            {
                typeOfCurrentWeapon.value.OnHit();
            }


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


        private bool GetIsInRange(Transform targetTransform)
        {
            return (Vector3.Distance(transform.position, targetTransform.position) < currentWeapon.WeaponRange());
        }

        public bool CanAttack(GameObject combatTarget)
        {   
            if(combatTarget == null) return false;
            if(!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && 
                !GetIsInRange(combatTarget.transform)) 
            {
                return false;
            }
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

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.WeaponDamage();
            }   
        }

        public IEnumerable<float> GetPercentageModifer(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBounus();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeapon = weapon;
            typeOfCurrentWeapon.value = AttachWeapon(weapon);

        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
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
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        
    }
 }
