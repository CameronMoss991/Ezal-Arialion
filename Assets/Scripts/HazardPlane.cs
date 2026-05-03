using UnityEngine;

public class HazardPlane : MonoBehaviour
{
    [Header("Hazard Settings")]
    public float damageAmount = 25f;
    public Transform respawnPoint; // Drag an Empty GameObject here for the target

    private void OnTriggerEnter(Collider other)
    {
        // Ensure your Player object has the "Player" tag in the Inspector
        if (other.CompareTag("Player"))
        {
            // 1. Damage the player using your script's TakeDamage function
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }

            // 2. Teleport the player
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
        {
            // If using a CharacterController, we MUST disable it to teleport
            cc.enabled = false;
            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation;
            cc.enabled = true;
        }
        else
        {
            // Standard transform move for non-CC players
            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation;
        }

        Debug.Log("Player fell into hazard! Teleported to " + respawnPoint.name);
    }
}