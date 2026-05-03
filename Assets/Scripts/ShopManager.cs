using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Core References")]
    public GameObject shopPanel;
    public FirstPersonControllerEdited playerMovement;
    public EnergyPistol weapon;
    public PlayerHealth playerHealth;
    
    [Header("Upgrade Amounts")]
    public float speedBoost = 2f;
    public float dodgeBoost = 3f;
    public float jumpBoost = 1f;
    public float damageBoost = 10f; // The missing damage boost!
    
    [Header("Weapon Tuning Amounts")]
    public float fireRateBoost = 0.05f; 
    public float rangeBoost = 20f;      

    [Header("Costs")]
    public int agilityCost = 25;       
    public int jumpCost = 50;
    public int damageCost = 75;        // Dedicated cost for damage
    public int tuningCost = 60;        
    public int healCost = 25;

    [Header("One-Time Costs")]
    public int autoFireCost = 150;
    public int explosiveCost = 300;
    public int piercingCost = 250;

    [Header("One-Time Buttons")]
    public Button autoFireBtn;
    public Button explosiveBtn;
    public Button piercingBtn;

    // --- REPEATABLE UPGRADES ---

    public void BuyDamage()
    {
        if (GameManager.Instance.SpendScore(damageCost))
        {
            weapon.damage += damageBoost;
            Debug.Log("Damage Upgraded! Current Damage: " + weapon.damage);
        }
    }

    public void BuyWeaponFireRateRange()
    {
        // Lower fireRate means faster firing (less delay)
        if (weapon.fireRate > 0.05f && GameManager.Instance.SpendScore(tuningCost))
        {
            weapon.fireRate -= fireRateBoost; 
            weapon.range += rangeBoost;       
            Debug.Log($"Tuning Complete! Rate: {weapon.fireRate} | Range: {weapon.range}");
        }
    }

    public void BuyJump()
    {
        if (GameManager.Instance.SpendScore(jumpCost))
        {
            playerMovement.jumpPower += jumpBoost;
            Debug.Log("Jump Power Upgraded: " + playerMovement.jumpPower);
        }
    }

    public void BuySpeed()
    {
        if (GameManager.Instance.SpendScore(agilityCost))
        {
            playerMovement.walkSpeed += speedBoost; 
            playerMovement.sprintSpeed += dodgeBoost;
            Debug.Log("Agility Upgraded!");
        }
    }

    public void BuyRepair()
    {
        if (playerHealth.shield < 100f && GameManager.Instance.SpendScore(healCost))
        {
            playerHealth.RestoreShield(25f);
        }
    }

    // --- ONE-TIME UPGRADES ---

    public void BuyAutoFire()
    {
        if (!weapon.isAutoFire && GameManager.Instance.SpendScore(autoFireCost))
        {
            weapon.isAutoFire = true;
            if (autoFireBtn != null) autoFireBtn.interactable = false;
        }
    }

    public void BuyExplosiveRounds()
    {
        if (!weapon.hasExplosiveRounds && GameManager.Instance.SpendScore(explosiveCost))
        {
            weapon.hasExplosiveRounds = true;
            if (explosiveBtn != null) explosiveBtn.interactable = false;
        }
    }

    public void BuyPiercing()
    {
        if (!weapon.hasPiercing && GameManager.Instance.SpendScore(piercingCost))
        {
            weapon.hasPiercing = true;
            if (piercingBtn != null) piercingBtn.interactable = false;
        }
    }

    // --- UI TOGGLE ---
    void Update()
    {
        if (GameManager.Instance.isGameOver) return; 

        if (Input.GetKeyDown(KeyCode.Tab)) OpenShop();
        if (Input.GetKeyUp(KeyCode.Tab)) CloseShop();
    }

    void OpenShop()
    {
        shopPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.2f; 
    }

    void CloseShop()
    {
        shopPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}