using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon pickupWeapon = null;
    [SerializeField] float respawnTime = 5f;
    GameObject pickupObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {   
            other.GetComponent<Fighter>().EquipWeapon(pickupWeapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }
    }
    private IEnumerator HideForSeconds(float seconds)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(respawnTime);
        ShowPickup(true);
    }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<SphereCollider>().enabled = shouldShow;
            // transform.GetChild(0).gameObject.SetActive(shouldShow);
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}
