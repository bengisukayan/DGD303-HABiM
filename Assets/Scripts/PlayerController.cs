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
    public float stars = 3f;
    public UnityEngine.UI.Image healthBar;
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

    private void Die() {
        stars--;
        //eksilt yıldız ve checkpoint
        if (stars == 0) {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }

    }
}
