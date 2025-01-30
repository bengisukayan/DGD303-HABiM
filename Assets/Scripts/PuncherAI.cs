using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PuncherAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask LayerGround, LayerPlayer;
    public Animator animator;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float cooldown;
    bool alreadyAttacked;
    public float attackDamage = 5f;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public float health = 10f;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, LayerPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, LayerPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) Chase();
        if (playerInAttackRange && playerInSightRange) Attack();
    }

    private void Patrol() {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;

        animator.SetBool("walking", agent.velocity.magnitude > 0.1f);
    }

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, LayerGround)) walkPointSet = true;
    }
    
    private void Chase() {
        agent.SetDestination(player.position);
        animator.SetBool("walking", agent.velocity.magnitude > 0.1f);
    }

    private void Attack() {
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        animator.SetBool("walking", false);

        if (!alreadyAttacked) {
            animator.SetTrigger("attack");
            Invoke(nameof(DealDamage), 0.5f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), cooldown);
        }
    }

    private void DealDamage() {
        if (player != null && playerInAttackRange) {
            PlayerController playerHealth = player.GetComponent<PlayerController>();
            playerHealth.TakeDamage(1);
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die() {
        Destroy(gameObject);
    }
}
