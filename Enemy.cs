using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth;
    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Damaged");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("Dead", true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyChaseAttack>().enabled = false;
        this.enabled = false;
    }
}
