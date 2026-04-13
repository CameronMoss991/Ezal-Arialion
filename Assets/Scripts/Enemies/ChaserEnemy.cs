using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    
    public float damageAmount = 10f;
    public float damageCooldown = 1.0f;
    private float lastDamageTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player != null && agent != null)
        {
            // If player is dodging, stop the agent in its tracks
            if (IsPlayerDodging())
            {
                agent.isStopped = true; 
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }
    }

    bool IsPlayerDodging()
    {
        var controller = player.GetComponent<FirstPersonControllerEdited>();
        // Using the public bool we set up earlier
        return controller != null && controller.isSprinting;
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