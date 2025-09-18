using UnityEngine;

public class EnemyChaseAttack : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int attackDamage;
    public LayerMask playerLayer;

    private Transform player;
    private Rigidbody2D rb;
    private Collider2D col;

    private Vector2 direction;
    public Animator animator;
    private float lastAttackTime;
    public bool isAttacking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        // GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // if (enemyObj != null)
        // {
        //     Collider2D enemyCol = enemyObj.GetComponent<Collider2D>();
        //     if (enemyCol != null && col != null)
        //     {
        //         Physics2D.IgnoreCollision(enemyCol, col, true);
        //     }
        // }
    }

    void Update()
    {
        if (player == null) return;
        Collider2D[] alliesCanBeHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        // if you're within attacking range & cooldown has elapsed, then attack
        if (alliesCanBeHit.Length != 0 && Time.time >= lastAttackTime + attackCooldown)
        {
            // Within range, try to attack
            direction = Vector2.zero;
            AttackAnimation();
            lastAttackTime = Time.time;
        }
        // if you're within attacking range but cooldown hasn't elapsed, then be idle
        else if (alliesCanBeHit.Length != 0 && Time.time < lastAttackTime + attackCooldown)
        {
            // Remain idle
            direction = Vector2.zero;
            animator.SetFloat("Speed", 0f); // <-- ensure idle animation
        }
        else
        {
            // Do not change direction during attack
            if (isAttacking) return;

            // Chase player
            direction = (player.position - transform.position).normalized;
            if (direction.x < 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = 1;
                transform.localScale = scale;
                // Walk animation
                animator.SetFloat("Speed", moveSpeed);
            }
            else if (direction.x > 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
                // Walk animation
                animator.SetFloat("Speed", moveSpeed);
            }

        }
    }

    void FixedUpdate()
    {
        // Move enemy + stop drifting during attack
        if (!isAttacking)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void AttackAnimation()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        Debug.Log($"Enemy attacks: Dealt {attackDamage} damage!");

        // You can add actual damage logic here, like:
        // player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    }

    // Called at the END of attack animation via Animation Event
    public void EndAttack()
    {
        isAttacking = false;
    }

    // Optional: Visualize attack range in Scene view
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void RestartAttackCooldown()
    {
        lastAttackTime = Time.time;
    }

    public void ApplyDamage()
    {
        // Detect allies in attack range on "hit" frame
        Collider2D[] hitAllies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        // Apply damage to allies on "hit" frame (just player for now)
        foreach (Collider2D ally in hitAllies)
        {
            ally.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }
    }
}