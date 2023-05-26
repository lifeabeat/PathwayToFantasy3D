using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Management
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
            yield return fader.FadeOut(timeToFadeOut);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            // Done Load
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(timeToWait);
            yield return fader.FadeIn(timeToFadeIn);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = otherPortal.spawwnPoint.position;
            player.transform.rotation = otherPortal.spawwnPoint.rotation;
            
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
