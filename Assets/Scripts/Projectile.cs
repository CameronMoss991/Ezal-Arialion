using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public float damage;
    [HideInInspector] public float lifetime;
    [HideInInspector] public bool hasPiercing;
    [HideInInspector] public bool hasExplosiveRounds;

    public float explosionRadius = 4f; // How big the explosion is
    public GameObject explosionParticles; // Optional: Drag a particle effect here!

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ApplyDamage(other.gameObject);

            // If we DO NOT have piercing, destroy the bullet
            if (!hasPiercing) 
            {
                Destroy(gameObject);
            }
        }
        
        // Always destroy bullet if it hits a wall/floor, even if it has piercing
        else if (other.CompareTag("Untagged") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // If we hit a wall with explosive rounds, it should still explode!
            if (hasExplosiveRounds) ApplyDamage(gameObject); 
            
            Destroy(gameObject);
        }
    }

    void ApplyDamage(GameObject directHit)
    {
        if (hasExplosiveRounds)
        {
            // 1. VISUAL: Spawn the sphere
            if (explosionParticles != null)
            {
                // Store the spawned sphere in a temporary variable 'exp'
                GameObject exp = Instantiate(explosionParticles, transform.position, Quaternion.identity);
                
                // TELL UNITY TO DESTROY IT: The second number is the delay in seconds
                Destroy(exp, 0.25f); 
            }

            // 2. LOGIC: Sphere overlap check
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Enemy"))
                {
                    hit.GetComponent<EnemyHealth>()?.TakeDamage(damage);
                    
                    Rigidbody enemyRb = hit.GetComponent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        Vector3 pushDirection = (hit.transform.position - transform.position).normalized;
                        enemyRb.AddForce(pushDirection * 15f, ForceMode.Impulse);
                    }
                }
            }
        }
        else
        {
            // Standard single-target damage if NOT explosive
            if (directHit.CompareTag("Enemy"))
            {
                directHit.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            }
        }
    }
}