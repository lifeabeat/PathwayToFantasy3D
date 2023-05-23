using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace  RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 0.5f;
        [SerializeField] float timeWaitForNextWayPoint = 3f;
        int currentWayPointIndex = 0;
        Health health;
        Fighter fighter;
        Mover mover;
        GameObject player;
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArriveAtWayPoint= Mathf.Infinity;

        private void Start() {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
        }
        private void Update() 
        {
            if(health.IsDead()) return;
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                // Suspicious State
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();

            }
            timeSinceLastSawPlayer +=Time.deltaTime;
            timeSinceArriveAtWayPoint  +=Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPostion = guardPosition;
            if (patrolPath != null)
            {
                if(AtWayPoint())
                {
                    //Get Next Waypoint Position from PatrolPath
                    timeSinceArriveAtWayPoint = 0;
                    CycleWayPoint();
                }
                nextPostion = GetCurrentWayPoint();
            }
            if (timeSinceArriveAtWayPoint >  timeWaitForNextWayPoint)
            {
                mover.StartMoveAction(nextPostion);
            }
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWaypointAtIndex(currentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextWaypointIndex(currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return (distanceToWayPoint < waypointTolerance);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,chaseDistance);   
        }
       
    }
}
