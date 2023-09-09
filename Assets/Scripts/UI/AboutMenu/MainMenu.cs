using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using RPG.SceneManagement;
using RPG.Utils;
using UnityEngine;
using TMPro;

namespace RPG.UI
{
    public class MainMenu : MonoBehaviour
    {
       UtilsValue<SavingWrapper> savingWrapper;

       [SerializeField] TMP_InputField newGameNameField;
        private void Awake()
        {
            savingWrapper = new UtilsValue<SavingWrapper>(GetSavingWrapper);
        }
        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }
        public void NewGame()
        {
            savingWrapper.value.NewGame(newGameNameField.text);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }


}
