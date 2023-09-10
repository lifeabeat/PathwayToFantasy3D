using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.UI
{
    public class PauseMenu : MonoBehaviour
    {
        PlayerController playerController;
        private void Awake()
        {
           playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
       private void OnEnable() 
       {
            if (playerController == null) return;
            Time.timeScale = 0;
            playerController.enabled = false;       
        }

       private void OnDisable() {
            if (playerController == null) return;
            Time.timeScale = 1;
            playerController.enabled = true;

        }

        public void SaveAndQuit ()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();   
            savingWrapper.LoadMenu();
        }
    }
}
