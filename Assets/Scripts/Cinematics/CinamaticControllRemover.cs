using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic
{
    public class CinamaticControllRemover : MonoBehaviour
    {
        GameObject player;

        private void Awake() {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable() {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        private void OnDisable() {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector a)
        {
            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector a)
        {
            
            Debug.Log("Enable Control");
            player.GetComponent<PlayerController>().enabled = true;
        }

    }
}


