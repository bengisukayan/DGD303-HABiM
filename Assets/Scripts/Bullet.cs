using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Shooter"))
        {
            other.GetComponent<ShooterAI>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Puncher"))
        {
            other.GetComponent<PuncherAI>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}