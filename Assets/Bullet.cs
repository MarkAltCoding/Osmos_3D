using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxTravelDistance;
    public bool isEnemyBullet;
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * Time.fixedDeltaTime;

        if (Vector3.Distance(transform.position, _startPosition) >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isEnemyBullet && collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.OnPlayerDeath(); // This should trigger the death sequence
            }
            Destroy(gameObject);
        }
        else if (!isEnemyBullet)
        {
            var enemy = collision.gameObject.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }
}