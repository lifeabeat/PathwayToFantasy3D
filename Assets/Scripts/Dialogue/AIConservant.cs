using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue 
{
    public class AIConservant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] string conservantName;
        public CursorType GetCursorType()
        {
            return CursorType.Conservation;
        }
        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null)
            {
                return false;
            }

            Health health = GetComponent<Health>();
            if (health && health.IsDead()) return false;
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConservant>().StartDialogue(this, dialogue);
            }
            return true;
        }

        public string GetName() 
        {
            return conservantName;
        }

    }
}
