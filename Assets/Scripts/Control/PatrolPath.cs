using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control{

    public class PatrolPath : MonoBehaviour
    {   
        const float WAYPOINT_GIZMO_RADIUS = 0.25f;
        private void OnDrawGizmos()
        {
            DrawPatrolPathGizmos(Color.white);
        }

        private void OnDrawGizmosSelected()
        {
            DrawPatrolPathGizmos(Color.green);
        }

        public Vector3 GetWaypointAtIndex(int index)
        {
            return transform.GetChild(index).position;
        }

        public int GetNextWaypointIndex(int index)
        {
            // Example using the modulo operator which returns the Integer remainder, wrapping to 0 when needed:
            // IE: 0 + 1 % 4 = 1 and 3 + 1 % 4 = 0
            return (index + 1) % transform.childCount;
        }

        private void DrawPatrolPathGizmos(Color color)
        {
            Gizmos.color = color;

            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypointAtIndex(i), WAYPOINT_GIZMO_RADIUS);
                Gizmos.DrawLine(GetWaypointAtIndex(i), GetWaypointAtIndex(GetNextWaypointIndex(i)));
            }
        }
    }
}
