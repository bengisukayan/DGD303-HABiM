using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;


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

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        projectile.transform.forward = shootDirection;

        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
        if (rigidBody != null)
        {
            rigidBody.velocity = shootDirection * projectileSpeed;
        }
    }
}