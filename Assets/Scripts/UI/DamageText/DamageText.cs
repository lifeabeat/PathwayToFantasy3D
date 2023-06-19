using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText 
{
    public class DamageText : MonoBehaviour
    {
      [SerializeField] GameObject targetToDestroy = null;
        [SerializeField] TextMeshProUGUI damageText = null;
        public void DestroyTarget()
        {
            Destroy(targetToDestroy);
        }

        public void SetText(float amount)
        {
            damageText.text = amount.ToString();
        }
    }
}
