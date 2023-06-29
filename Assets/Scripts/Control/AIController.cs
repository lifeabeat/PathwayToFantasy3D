using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Attributes;

namespace  RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroTime = 3f;
        [SerializeField] float aggroArea = 2f;
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
        float timeSinceAggro = Mathf.Infinity;

        private void Awake() {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
        }
        private void Start() {
            guardPosition = transform.position;
        }
        private void Update() 
        {
            if(health.IsDead()) return;
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
                mover.SetSpeed(4.5f);
            }
            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                // Suspicious State
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
                mover.SetSpeed(2.5f);

            }
            timeSinceLastSawPlayer +=Time.deltaTime;
            timeSinceArriveAtWayPoint  +=Time.deltaTime;
            timeSinceAggro += Time.deltaTime;
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
            AggroNearbyEnemy();
        }

        private void AggroNearbyEnemy()
        {
           RaycastHit[] hits = Physics.SphereCastAll(transform.position, aggroArea, Vector3.up, 0);
           foreach( RaycastHit hit in hits)
           {
                AIController targetArgo = hit.transform.GetComponent<AIController>();
                if (targetArgo == null) continue;
                if (targetArgo.timeSinceAggro < targetArgo.aggroTime)
                {
                    return;
                }
                targetArgo.AggroBehaviour();
           }
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            
            return distanceToPlayer < chaseDistance || timeSinceAggro < aggroTime;
        }

        public void AggroBehaviour()
        {
            timeSinceAggro = 0;
        }

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,chaseDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroArea);
        }
       
    }
}
