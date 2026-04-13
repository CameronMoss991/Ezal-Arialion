using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 40f;
    public float damage = 25f;
    public float lifetime = 3.0f; // Destroy if it misses and flies into space

    void Start()
    {
        // Automatically destroy after a few seconds to save memory
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move forward every frame
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Did we hit an enemy?
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            Destroy(gameObject); // Remove bullet on hit
        }
        
        // Destroy bullet if it hits a wall/floor
        if (other.CompareTag("Untagged") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}