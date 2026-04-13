using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    public float shieldAmount = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Restore shield and destroy the pickup
            other.GetComponent<PlayerHealth>()?.RestoreShield(shieldAmount);
            Destroy(gameObject);
        }
    }
}
