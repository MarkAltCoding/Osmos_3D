using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxTravelDistance; // Maximum distance the bullet can travel
    private Vector3 startPosition; // Position where the bullet was spawned
    private Rigidbody rb; // Reference to the Rigidbody component

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position; // Store the spawn position
    }

    void FixedUpdate()
    {
        // Dynamically adjust the speed of the bullet based on the player's movement state
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * (PlayerMovement.isMoving ? 10f : 0.5f);
        }

        // Check if the bullet has traveled the maximum distance  
        if (Vector3.SqrMagnitude(transform.position - startPosition) >= maxTravelDistance * maxTravelDistance)  
        {  
            Destroy(gameObject); // Destroy the bullet  
        }  
    }

    void OnCollisionEnter(Collision collision)
    {
        // Optional: Destroy the object hit (commented out here)
        // Destroy(collision.gameObject);
        Destroy(gameObject); // Destroy the bullet
    }
}