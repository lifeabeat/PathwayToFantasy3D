using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDev.Inventories;


namespace RPG.Control
{
    public class ClickablePickup : Pickup, IRaycastable
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public CursorType GetCursorType()
        {
             return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<ItemCollector>().CollectItem(pickup);
            
            }
            return true;
        }
    }
}

