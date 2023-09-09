using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using UnityEngine.SceneManagement;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string currentSaveKey = "currentSaveName";
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] int firstBuildIndex = 1;
        [SerializeField] int menuBuildIndex = 0;

        public void ContinueGame() {
            if (!PlayerPrefs.HasKey(currentSaveKey)) return;
            if (!GetComponent<SavingSystem>().SaveFileExist(GetCurrentSave())) return;
            StartCoroutine(LoadLastScreen());
        }
        public void NewGame(string saveFile)
        {
            if(String.IsNullOrEmpty(saveFile)) return;
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScreen());
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        internal void LoadMenu()
        {
            StartCoroutine(LoadMenuScreen());
        }
        private IEnumerator LoadMenuScreen()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(menuBuildIndex);
            //fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }
        private IEnumerator LoadLastScreen()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            //fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }
        private IEnumerator LoadFirstScreen()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstBuildIndex);
            //fader.FadeOutImmediate();
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
           GetComponent<SavingSystem>().Load(GetCurrentSave());
        }
        public void Save()
        {
            //Call to Saving System Load
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        
    }
}
