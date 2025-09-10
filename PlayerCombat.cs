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
    public PlayerMovement playerMovement;
    public HitStopController hitStopController;

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AttackAnimation();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void AttackAnimation()
    {
        // Play an attack animation
        animator.SetTrigger("Attack");

        // Stop the player from drifting
        playerMovement.canMove = false;
        playerMovement.movement = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // call this method using animation event on "hit" frame
    public void ApplyDamage()
    {
        // Detect enemies in attack range on "hit" frame
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        // Apply damage to enemies on "hit" frame
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        // might need to add if statement so hitstop only activates if hitEnemies.length != 0
        hitStopController.DoHitStop(hitEnemies);
    }
}