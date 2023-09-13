using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorPortal : MonoBehaviour
{
    public GameObject portal;
    public bool isDefeated = false;

    public void SpawnPortal ()
    {
        isDefeated = true;
        if (AudioManagerUpdateVer1.HasInstance)
        {
            AudioManagerUpdateVer1.Instance.PlaySE(AUDIO.BGM_SPELL_00);
        }

        if (isDefeated == true)
        {
            portal.SetActive(true);
        }
    }
    private void Update() {
        if (isDefeated == true)
        {
            portal.SetActive(true);
        }
    }
}
