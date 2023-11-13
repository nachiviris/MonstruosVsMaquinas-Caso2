using System.Collections;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Transform[] patrolPoints; // Agrega los puntos de patrulla en el Inspector

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private int currentPatrolIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(Patrol());
    }

    void Update()
    {
        // Check if there's ground beneath the enemy
        bool isGrounded = IsGrounded();

        // Move towards the current patrol point
        Move();

        // Flip enemy based on the current patrol point
        if (currentPatrolIndex == 0)
        {
            FlipToLeft();
        }
        else if (currentPatrolIndex == 1)
        {
            FlipToRight();
        }

        // Update the animator parameter "isMoving" based on the current state
        animator.SetBool("isMoving", isMoving);
    }

    void Move()
    {
        // Move towards the current patrol point
        float direction = Mathf.Sign(patrolPoints[currentPatrolIndex].position.x - transform.position.x);
        Vector2 movement = new Vector2(direction * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        // If the enemy is close to the current patrol point, switch to the next one
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPatrolIndex].position.x) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }

        // Check if the enemy is moving
        isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;
    }

    void FlipToLeft()
    {
        // Rotate to the left
        if (isFacingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = false;
        }
    }

    void FlipToRight()
    {
        // Rotate to the right
        if (!isFacingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = true;
        }
    }

    bool IsGrounded()
    {
        // Check if there's ground beneath the enemy
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask);
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            // Wait for a short duration before changing direction
            yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 3.0f));
        }
    }
}
