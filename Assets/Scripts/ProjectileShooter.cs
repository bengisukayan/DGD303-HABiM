using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        Vector3 mousePosition = Input.mousePosition;

        float normalizedX = mousePosition.x / Screen.width * 2f - 1f;
        float normalizedZ = mousePosition.y / Screen.height * 2f - 1f;

        Vector3 shootDirection = new Vector3(normalizedX, 0f, normalizedZ).normalized;
        Vector3 spawnPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);

        FindObjectOfType<AudioManager>().PlayShooting();
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.forward = shootDirection;

        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
        if (rigidBody != null)
        {
            rigidBody.velocity = shootDirection * projectileSpeed;
        }
    }
}
