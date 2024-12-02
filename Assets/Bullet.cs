using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void onCollisionEnter2D(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
