using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] public Transform target;

    private void Update()
    {

        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }

        UpdateAnimator();

    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if(hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }    
    }   
    
    private void UpdateAnimator()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        // Change velocity from worldspace to localspace because when creating velocity, navmesh velocity store the velocity as variable in UpdateAnimator as global coordinate
        // Animator just know Running forward or not. So use Inverse to tell Animator that Animator is moving forward 
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

}
