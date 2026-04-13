using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float shield = 100f;
    public bool isOneShot = false;

    //public Image shieldBarImage;
    public TextMeshProUGUI shieldDisplay; // Drag your ShieldText here

    void Update()
    {
        // Fill Amount is a value between 0 and 1
        //shieldBarImage.fillAmount = shield / 100f;
        // Keep the UI updated every frame
        if (shieldDisplay != null)
        {
            shieldDisplay.text = "SHIELD: " + Mathf.RoundToInt(shield) + "%";
            
            // Visual feedback: Turn text red when One-Shot
            shieldDisplay.color = (shield <= 0) ? Color.red : Color.cyan;
        }
    }
    public void TakeDamage(float amount)
    {
        if (shield > 0)
        {
            shield -= amount;
            if (shield <= 0)
            {
                shield = 0;
                isOneShot = true;
                Debug.Log("Shield Down! You are now One-Shot.");
            }
        }
        else if (isOneShot)
        {
            GameManager.Instance.EndGame();
        }
    }

    public void RestoreShield(float amount)
    {
        shield += amount;
        if (shield > 0) isOneShot = false;
        Debug.Log("Shield Restored! Current Shield: " + shield);
    }
}