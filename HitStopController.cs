using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopController : MonoBehaviour
{
    // hit stop duration
    public float hitStopDuration = 0.3f;

    // player variables
    public Rigidbody2D playerRb;
    public Animator playerAnimator;
    public PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public void DoHitStop(Collider2D[] hitEnemies)
    {
        StartCoroutine(HitStopCoroutine(hitEnemies));
    }

    private IEnumerator HitStopCoroutine(Collider2D[] hitEnemies)
    {
        // 1. Freeze player
        playerAnimator.speed = 0f;

        // 2. Freeze enemies
        List<Animator> enemyAnimators = new List<Animator>();
        List<Rigidbody2D> enemyRigidbodies = new List<Rigidbody2D>();
        List<EnemyChaseAttack> enemyScripts = new List<EnemyChaseAttack>();

        foreach (Collider2D enemy in hitEnemies)
        {
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            Animator anim = enemy.GetComponent<Animator>();
            EnemyChaseAttack script = enemy.GetComponent<EnemyChaseAttack>();

            if (rb != null) {
                rb.linearVelocity = Vector2.zero;
                enemyRigidbodies.Add(rb);
            }

            // if (anim != null) {
            //     anim.speed = 1f;
            //     enemyAnimators.Add(anim);
            // }

            if (script != null) {
                script.enabled = false;
                enemyScripts.Add(script);
            }
        }

        // 3. Wait
        yield return new WaitForSeconds(hitStopDuration);

        // 4. Resume player
        playerMovement.enabled = true;
        playerAnimator.speed = 1f;

        // 5. Resume enemies
        foreach (Rigidbody2D rb in enemyRigidbodies) rb.linearVelocity = Vector2.zero;
        // foreach (Animator anim in enemyAnimators) anim.speed = 1f;
        foreach (EnemyChaseAttack script in enemyScripts)
        {
            Enemy enemy = script.GetComponent<Enemy>();
            // only re-enable EnemyChaseAttack script if enemy is alive...
            if (enemy.currentHealth > 0)
            {
                script.enabled = true;
            }
        }

    }
}
