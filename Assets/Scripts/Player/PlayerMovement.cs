using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    // Miscellaneous variables
    [SerializeField] private float moveSpeed = 5f;
    public Animator anim;
    private Vector2 moveDirection;

    // Collision variables
    private Rigidbody2D rb;
    public float collisionOffset = 0.05f;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public ContactFilter2D movementFilter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ProcessInputs();
        Animate();
    }

    void FixedUpdate()
    {
        Move();

        if (moveDirection != Vector2.zero)
        {
            bool success = TryMove(moveDirection);

            if (!success)
            {
                success = TryMove(new Vector2(moveDirection.x, 0));

                if (!success)
                {
                    success = TryMove(new Vector2(0, moveDirection.y));
                }
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
    
        int count = rb.Cast
        (
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset
        );

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    void Animate()
    {
        anim.SetFloat("AnimMoveX", moveDirection.x);
        anim.SetFloat("AnimMoveY", moveDirection.y);
        anim.SetFloat("AnimMoveMagnitude", moveDirection.magnitude);
    }
}
