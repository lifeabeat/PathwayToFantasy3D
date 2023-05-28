using System;
using UnityEngine;
using Random = UnityEngine.Random;
using RPG.Core;

namespace RPG.Combat
{
    public class Projectiles : MonoBehaviour
    {
        [SerializeField] float speed = 3f;
        [SerializeField] bool isHoming;
         Health target = null;
         float damage = 0;

        Vector3 targetOffset;

        // Update is called once per frame
        private void Start() {
            GenerateRandomTargetOffset();
            transform.LookAt(GetAimTarget());
        }
        void Update()
        {
             if(target == null) return;
             if (isHoming && !target.IsDead())
             {
                transform.LookAt(GetAimTarget());
             }
             transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimTarget()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return  target.transform.position;
            }
            return target.transform.position + targetOffset;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target) return; 
            if(target.IsDead()) return;
                target.TakeDamage(damage);
                Destroy(this.gameObject);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }
        private void GenerateRandomTargetOffset()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            float randomX = Random.Range(-targetCapsule.radius, targetCapsule.radius);
            float randomZ = Random.Range(-targetCapsule.radius, targetCapsule.radius);
            float randomY = Random.Range(0f, targetCapsule.height);
            targetOffset = new Vector3(randomX, randomY, randomZ);
        }
    }
}

