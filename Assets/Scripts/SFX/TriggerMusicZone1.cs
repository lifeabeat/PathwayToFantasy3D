using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusicZone1 : MonoBehaviour
{

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player")
            {
            if (AudioManagerUpdateVer1.HasInstance)
            {
                AudioManagerUpdateVer1.Instance.PlayBGM(AUDIO.BGM_OUTSIDE);
            }
        }
        }
}
