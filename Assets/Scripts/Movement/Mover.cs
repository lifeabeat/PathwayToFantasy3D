using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;

        Health health;
        NavMeshAgent navMeshAgent;

        private void Awake() {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
         void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        public void SetSpeed(float speed)
        {
            navMeshAgent.speed = speed;
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
            
        }
        public void MoveTo(Vector3 destination)
        {
            
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            // Change velocity from worldspace to localspace because when creating velocity, navmesh velocity store the velocity as variable in UpdateAnimator as global coordinate
            // Animator just know Running forward or not. So use Inverse to tell Animator that Animator is moving forward 
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            // Casting
            SerializableVector3 position = (SerializableVector3)state;
            navMeshAgent.enabled = false;
            transform.position = position.ToVector();
            navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
