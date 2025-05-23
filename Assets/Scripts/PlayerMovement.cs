using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    public void Start() {
        this.rb = GetComponent<Rigidbody2D>();
    }

    public void Update() {
        rb.linearVelocity = (moveInput).normalized * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context) {
        this.moveInput = context.ReadValue<Vector2>();
    }
}
