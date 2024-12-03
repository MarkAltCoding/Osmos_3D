using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            if (PlayerMovement.isMoving)
            {
                bullet.GetComponent<Rigidbody>().linearVelocity = bullet.transform.forward * 10f;
            }
            else
            {
                bullet.GetComponent<Rigidbody>().linearVelocity = bullet.transform.forward * 0.5f;
            }
        }
    }
}
