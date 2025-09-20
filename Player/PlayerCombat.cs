using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public int maxHealth;
    private int currentHealth;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    public float attackRate = 2f; // attacks per second
    float nextAttackTime;
    public Rigidbody2D rb;
    public PlayerMovement playerMovement;
    public HitStopController hitStopController;
    private bool isDead = false;
    public GameObject deathPanel;   // Link your UI panel here
    public HealthBar healthBar;

    void Start()
    {
        // set health and health bar values
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        // other settings
        deathPanel.SetActive(false);
        Time.timeScale = 1f; // Make sure time is normal at start
    }

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

        // Only apply hit stop if enemies are hit
        if (hitEnemies.Length != 0)
            hitStopController.DoHitStop(hitEnemies);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // ignore hits after death

        // set health
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("Damaged");

        // enemyChaseAttack.RestartAttackCooldown();
        // enemyChaseAttack.isAttacking = false;

        // Stop the player from drifting
        playerMovement.canMove = false;
        playerMovement.movement = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("Dead", true);

        // stop movement/combat scripts
        GetComponent<PlayerMovement>().enabled = false;
        this.enabled = false;

        ShowDeathScreen();
    }

    void ShowDeathScreen()
    {
        // Slow motion effect
        Time.timeScale = 0.3f;
        deathPanel.SetActive(true);

        // Optional: Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}