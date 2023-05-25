using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic
{
    public class CinamticTrigger : MonoBehaviour
    {
        bool firstTimeTrigger = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !firstTimeTrigger)
            {
                firstTimeTrigger = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }

}
