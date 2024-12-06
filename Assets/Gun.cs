using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float movingFireRate;
    public float stationaryFireRate;

    private float _lastFireTime = 0;

    void Update()
    {
        // Determine the fire rate based on the player's movement
        float currentFireRate = PlayerMovement.isMoving ? movingFireRate : stationaryFireRate;
        float fireInterval = 1f / currentFireRate;

        // Check if enough time has passed to fire again
        if (Input.GetButton("Fire1") && Time.time >= _lastFireTime + fireInterval)
        {
            Fire();
            _lastFireTime = Time.time;
        }
    }

    void Fire()
    {
        // Instantiate and set bullet velocity
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (PlayerMovement.isMoving)
        {
            bulletRb.linearVelocity = bullet.transform.forward * 10f; // Higher speed when moving
        }
        else
        {
            bulletRb.linearVelocity = bullet.transform.forward * 0.5f; // Lower speed when stationary
        }
    }
}