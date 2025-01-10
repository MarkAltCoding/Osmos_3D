using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float baseFireRate;
    public Transform playerCamera;

    private float _currentReloadTime;

    void Start()
    {
        // Debug initial setup
        Debug.Log($"EnemyGun Start - BulletPrefab: {(bulletPrefab != null ? "Assigned" : "Missing")}");
        Debug.Log($"EnemyGun Start - BulletSpawnPoint: {(bulletSpawnPoint != null ? "Assigned" : "Missing")}");
        Debug.Log($"EnemyGun bulletSpawnPoint: {bulletSpawnPoint?.name}");

        if (playerCamera == null)
        {
            GameObject cameraObject = GameObject.FindWithTag("MainCamera");
            if (cameraObject != null)
            {
                playerCamera = cameraObject.transform;
                Debug.Log("Found player camera through tag");
            }
            else
            {
                Debug.LogError("Player Camera not found!");
            }
        }

        // Set an initial reload time
        _currentReloadTime = 1f / baseFireRate; // Adjust this value if needed
        Debug.Log($"EnemyGun initial reload time set to: {_currentReloadTime}");
    }

    void FixedUpdate()
    {
        if (playerCamera == null) return;

        Vector3 directionToCamera = playerCamera.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
        
        transform.rotation = Quaternion.Euler(
            targetRotation.eulerAngles.x,
            targetRotation.eulerAngles.y,
            transform.rotation.eulerAngles.z
        );

        _currentReloadTime = Mathf.Max(0f, _currentReloadTime - Time.fixedDeltaTime);
    }

    public void Shoot()
    {
        Debug.Log($"Shoot called - Current reload time: {_currentReloadTime}");
        if (_currentReloadTime <= 0f)
        {
            Fire();
            _currentReloadTime = 1f / baseFireRate;
            Debug.Log($"Reset reload time to: {_currentReloadTime}");
        }
    }

    private void Fire()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("BulletPrefab or BulletSpawnPoint is missing!");
            return;
        }

        // Get the position and forward direction of the bulletSpawnPoint
        Vector3 spawnPosition = bulletSpawnPoint.position;
        Vector3 direction = (playerCamera.position - spawnPosition).normalized;

        // Instantiate the bullet at the spawn position and rotation
        var bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));
        Debug.Log($"Bullet spawned at {spawnPosition} and directed towards {playerCamera.position}");

        // Ensure the bullet has the Bullet script and mark it as an enemy bullet
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.isEnemyBullet = true;
        }

        // Apply velocity if Rigidbody is attached
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = direction * 10f; // Adjust the speed as needed
            Debug.Log($"Bullet velocity set to {bulletRb.linearVelocity}");
        }
    }
}