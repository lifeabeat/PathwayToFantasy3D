using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Saving;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D
        }
        [SerializeField] int sceneToLoad = 1;
        [SerializeField] Transform spawwnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float timeToFadeOut = 1f;
        [SerializeField] float timeToFadeIn = 2f;
        [SerializeField] float timeToWait = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.Log("Scene to load not set.");
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            //Save State of Current Scene
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(timeToFadeOut);
            savingWrapper.Save();

            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            // Load Sate From Before New Scene
            savingWrapper.Load();
          
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();
            
            // yield return new WaitForSeconds(timeToWait);
            fader.FadeIn(timeToFadeIn);

            newPlayerController.enabled = true;
            Destroy(gameObject);
            Debug.Log(" done");
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            // Turn off NavMeshAgent wait for position to update or Its sometimes confuse between the position from Load() last position in the old world and new position. 
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawwnPoint.position;
            player.transform.rotation = otherPortal.spawwnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
            
        }

        private Portal GetOtherPortal()
        {

            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }
            return null;
        }
    }
}
