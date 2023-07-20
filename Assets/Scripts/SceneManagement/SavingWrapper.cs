using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 0.2f;

        private void Awake() {
            StartCoroutine(LoadLastScreen());
        }
        private IEnumerator LoadLastScreen()
        {
           yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
           yield return fader.FadeIn(fadeInTime);
        }

       //Loading API
       void Update()
       {
         if(Input.GetKeyDown(KeyCode.L))
         {
            Load();
         }
         if(Input.GetKeyDown(KeyCode.S))
         {
            Save();
         }

        if(Input.GetKeyDown(KeyCode.D))
        {
            Delete();
        }
       }

        public void Load()
        {
           //Call to Saving System Load
           StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
        }
        public void Save()
        {
            //Call to Saving System Load
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
