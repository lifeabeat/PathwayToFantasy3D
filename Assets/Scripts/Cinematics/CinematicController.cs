using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic
{
    public class CinematicController : MonoBehaviour
    {
        bool firstTimeTrigger = false;

        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Player" )
            {
                firstTimeTrigger = true;
                if (!firstTimeTrigger)
                {
                    GetComponent<PlayableDirector>().Play();
                }
                
            }
        }
    }
}


