using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public Transform bulletSpawnPoint; // Where bullets fire from
    public GameObject bulletPrefab;    // Bullet prefab
    public float baseFireRate;    // Fire rate (shots per second)

    private float _currentReloadTime;

    void Start()
    {
        Debug.Log($"PlayerGun bulletSpawnPoint: {bulletSpawnPoint?.name}");
        
        // Set initial reload time to prevent immediate shooting
        _currentReloadTime = 1f / baseFireRate;
        Debug.Log($"PlayerGun initial reload time set to: {_currentReloadTime}");
    }
    
    void FixedUpdate()
    {
        // Reduce the reload timer
        _currentReloadTime = Mathf.Max(0f, _currentReloadTime - Time.fixedDeltaTime);
    }

    public void Shoot()
    {
        if (_currentReloadTime <= 0f)
        {
            Fire();
            _currentReloadTime = 1f / baseFireRate; // Reset reload time
        }
    }

    private void Fire()
    {
        if (bulletSpawnPoint == null) return;

        // Spawn and fire the bullet straight ahead
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.linearVelocity = bulletSpawnPoint.forward * 10f; // Forward direction
    }
}