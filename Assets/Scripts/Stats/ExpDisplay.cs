using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
    public class ExpDisplay : MonoBehaviour
    {
        Experience exp;
        private void Awake()
        {
            exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update() 
        
        {
            GetComponent<TextMeshProUGUI>().text = exp.GetPoint().ToString();
        }
    }

}