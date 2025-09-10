using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Vector2 movement;
    public bool canMove;

    void Awake()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (movement.x == -1)
            {
                Vector3 scale = transform.localScale;
                scale.x = 1;
                transform.localScale = scale;
            }
            else if (movement.x == 1)
            {
                Vector3 scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
            }
        }
    }

    void FixedUpdate()
    {
        if (canMove)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        else
            rb.MovePosition(rb.position); // lock in place
    }

    public void AllowMove()
    {
        canMove = true;
    }
}