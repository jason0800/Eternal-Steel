using UnityEngine;

public class EnemyChaseAttack : MonoBehaviour
{
    public float moveSpeed = 2f;
    // public float attackRange = 1.5f;
    // public float attackCooldown = 1f;
    // public int attackDamage = 1;

    private Transform player;
    public Rigidbody2D rb;

    private Vector2 movement;
    public Animator animator;
    // private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure your player object is tagged 'Player'.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // if (distance <= attackRange)
        // {
        //     // Within range, try to attack
        //     movement = Vector2.zero;

        //     if (Time.time >= lastAttackTime + attackCooldown)
        //     {
        //         Attack();
        //         lastAttackTime = Time.time;
        //     }
        // }
        // else
        // {

        // Chase player
        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction;

        // Walk animation
        animator.SetFloat("Speed", moveSpeed);

        // }
    }

    void FixedUpdate()
    {
        // Move enemy
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // void Attack()
    // {
    //     Debug.Log("Enemy attacks!");

    //     // You can add actual damage logic here, like:
    //     // player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    // }

    // Optional: Visualize attack range in Scene view
    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, attackRange);
    // }
}