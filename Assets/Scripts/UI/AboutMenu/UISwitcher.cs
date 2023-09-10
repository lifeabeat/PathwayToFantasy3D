using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] GameObject entryPoint;
        // Start is called before the first frame update
        void Start()
        {
            //SwtichTo(entryPoint);
        }

        public void SwtichTo(GameObject toDisplay)
        {
            if (AudioManagerUpdateVer1.HasInstance)
            {
                AudioManagerUpdateVer1.Instance.PlaySE(AUDIO.BGM_MENU_SELECT_00);
            }
            if (toDisplay.transform.parent != transform) return;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == toDisplay);
            }
        }

    }
}
