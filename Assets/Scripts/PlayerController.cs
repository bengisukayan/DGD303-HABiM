using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 8;
    public float groundDistance = 0;

    public LayerMask floor;
    public Rigidbody rigidBody;
    public SpriteRenderer spriteRenderer;
    private bool _walking;
    public Animator animator;

    public float maxHealth = 20f;
    public float health = 20f;
    public UnityEngine.UI.Image healthBar;

    public float stars = 3f;
    public GameObject starsUI;

    public Transform checkPoint;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, floor))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDistance;
                transform.position = movePos;
            }
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 moveDir = new(moveHorizontal, 0, moveVertical);
        rigidBody.velocity  = moveDir * speed;
        
        _walking = moveHorizontal != 0 || moveVertical != 0;
        animator.SetBool("walking", _walking);

        if (moveHorizontal != 0 && moveHorizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveHorizontal != 0 && moveHorizontal > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        healthBar.fillAmount = health / maxHealth;
        if (health <= 0) Die();
    }

    public void Heal() {
        health = maxHealth;
        healthBar.fillAmount = maxHealth;
    }

    public void Checkpoint(Transform transform) {
        FindObjectOfType<AudioManager>().PlayCheckpoint();
        checkPoint = transform;
    }

    private void Die() {
        stars--;
        switch(stars)
        {
            case 2:
                starsUI.transform.GetChild(2).gameObject.SetActive(false);
                Respawn();
                break;
            case 1:
                starsUI.transform.GetChild(2).gameObject.SetActive(false);
                starsUI.transform.GetChild(1).gameObject.SetActive(false);
                Respawn();
                break;
            default:
                break;
        }
        if (stars == 0) {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }

    private void Respawn()
    {
        FindObjectOfType<AudioManager>().PlayRespawn();
        Heal();
        if (checkPoint != null)
        {
            transform.position = checkPoint.position + new Vector3(2, 0, 0);
        }
        else
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }
}
