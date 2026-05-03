using UnityEngine;

public class EnergyPistol : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform spawnPoint;
    
    [Header("Upgradable Stats")]
    public float fireRate = 0.2f; 
    public float damage = 25f;
    public float range = 100f; // How far the bullet flies
    public float projectileSpeed = 40f;

    [Header("One-Time Upgrades")]
    public bool isAutoFire = false;
    public bool hasExplosiveRounds = false;
    public bool hasPiercing = false;
    
    private float nextFireTime;

    void Update()
    {
        // MAGIC TRICK: If auto-fire is true, use GetButton (hold). If false, use GetButtonDown (click).
        bool isTryingToShoot = isAutoFire ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

        if (isTryingToShoot && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // 1. Spawn the bullet
        GameObject bullet = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        
        // 2. Pass the Pistol's stats into this specific bullet!
        Projectile projScript = bullet.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.damage = this.damage;
            projScript.speed = this.projectileSpeed;
            
            // Calculate how long the bullet should live based on Range and Speed
            projScript.lifetime = this.range / this.projectileSpeed; 
            
            projScript.hasPiercing = this.hasPiercing;
            projScript.hasExplosiveRounds = this.hasExplosiveRounds;
        }
    }
}