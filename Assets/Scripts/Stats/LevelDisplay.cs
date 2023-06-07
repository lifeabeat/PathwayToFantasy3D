using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStat baseStat;
        private void Awake() {
            baseStat = GameObject.FindWithTag("Player").GetComponent<BaseStat>();
        }

        void Update()
        {   
            
            GetComponent<TextMeshProUGUI>().text = baseStat.GetLevel().ToString();
        }
    }

}