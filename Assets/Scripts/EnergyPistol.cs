using UnityEngine;

public class EnergyPistol : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform spawnPoint;
    public float fireRate = 0.2f; // Faster firing
    public float damage = 25f;
    public float range = 100f;
    
    private float nextFireTime;
    private Camera fpsCam;

    void Start()
    {
        fpsCam = GetComponentInParent<Camera>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // 1. VISUAL: Spawn the fast bolt
        Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        // 2. LOGIC: Instant Raycast hit
        RaycastHit hit;
        // Shoot the ray from the center of the screen
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyHealth>()?.TakeDamage(damage);
                Debug.Log("Raycast Hit Enemy!");
            }
        }
    }
}