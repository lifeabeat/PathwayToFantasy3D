using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Management
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        private void Start() 
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public IEnumerator FadeOut(float time)
        {
            while(canvasGroup.alpha < 1) //alpha is not 1
            {
                // moving alpha forward 1
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
                
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0) //alpha is not 0
            {
                // moving alpha forward 0
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;

            }
        }
    }
}

