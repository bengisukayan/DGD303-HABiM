using UnityEngine;

public class BulletSpecial : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shooter"))
        {
            other.GetComponent<ShooterAI>().TakeDamage(3);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Puncher"))
        {
            other.GetComponent<PuncherAI>().TakeDamage(3);
            Destroy(gameObject);
        }
    }
}