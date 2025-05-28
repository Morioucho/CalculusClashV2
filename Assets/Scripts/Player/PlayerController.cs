using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;

    public Animator animator;

    public bool canMove = true;

    private ContactFilter2D movementFilter;
    private Vector2 movementInput;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    void Start() {
        this.rb = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        if (canMove) {
            if (movementInput != Vector2.zero) {
                bool success = TryMove(movementInput);

                if (!success) {
                    success = TryMove(new Vector2(movementInput.x, 0));

                    if (!success) {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }
            }
        }

        Animate();
    }

    private bool TryMove(Vector2 direction) {
        if (direction != Vector2.zero) {
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0) {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;

            } else {
                return false;
            }
        } else {
            return false;
        }
    }
    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    void Animate() {
        animator.SetFloat("AnimMoveX", movementInput.x);
        animator.SetFloat("AnimMoveY", movementInput.y);

        animator.SetFloat("AnimMoveMagnitude", movementInput.magnitude);
    }
}