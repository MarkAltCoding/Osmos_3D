using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; // Assign the player GameObject here
    public float stoppingDistance = 1.5f; // Distance to stop chasing the player

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        // Get the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component missing on the enemy!");
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned in EnemyMovement script!");
            return;
        }

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the enemy is farther than the stopping distance, set the destination to the player
        if (distanceToPlayer > stoppingDistance)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            // Stop moving when within the stopping distance
            navMeshAgent.ResetPath();
        }
    }
}