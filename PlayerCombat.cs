using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    public float attackRate = 2f; // attacks per second
    float nextAttackTime;
    public Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {

    if (Time.time >= nextAttackTime)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
        }
    }

    void Attack()
    {
        // Play an attack animation
        animator.SetTrigger("Attack");
        // Detect enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Apply damage to enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        // Stop the player from drifting
        rb.linearVelocity = Vector2.zero;
        Debug.Log("rb.linearVelocity: " + rb.linearVelocity);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}