using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Transform player;
    
    public float damageAmount = 10f;
    public float damageCooldown = 1.0f;
    private float lastDamageTime;

    private bool isLeaping = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        
        // 1. Get the Rigidbody so they can Leap
        rb = GetComponent<Rigidbody>(); 
        
        // 2. WE ADDED THIS BACK: Find the player so they know who to chase!
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // 3. Keep your awesome GameManager speed scaling
        float difficulty = GameManager.Instance.GetDifficultyProgress();
        float scaledSpeed = Mathf.Lerp(GameManager.Instance.startEnemySpeed, GameManager.Instance.endEnemySpeed, difficulty); 
        
        agent.speed = scaledSpeed;
        agent.acceleration = 25f;
    }

    void Update()
    {
        if (player == null || agent == null || isLeaping) return;

        if (IsPlayerDodging())
        {
            agent.isStopped = true; 
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            
            // BONUS: The "Climb/Leap" Logic
            // If the player is above the enemy, and the enemy is close but stuck in a crowd
            if (player.position.y > transform.position.y + 1.5f && Vector3.Distance(transform.position, player.position) < 5f)
            {
                StartCoroutine(LeapUpward());
            }
        }
    }

    bool IsPlayerDodging()
    {
        var controller = player.GetComponent<FirstPersonControllerEdited>();
        return controller != null && controller.isSprinting;
    }

    IEnumerator LeapUpward()
    {
        isLeaping = true;
        agent.enabled = false; // Disable NavMesh so physics can take over
        
        if (rb != null)
        {
            // Give them a physics boost up and forward over the crowd
            Vector3 leapForce = (Vector3.up * 7f) + (transform.forward * 3f);
            rb.AddForce(leapForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1.0f); // Wait for them to land
        
        agent.enabled = true; // Re-enable NavMesh
        isLeaping = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damageAmount);
            lastDamageTime = Time.time;
        }
    }
}