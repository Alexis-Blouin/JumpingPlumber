using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private float _moveInput;
    private Vector2 _velocity;

    private bool _isGrounded;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _isGrounded = false;
        }
    }

    private void Update()
    {
        float distance = 0.1f;
        float angle = 0.0f;
        float castSize = 0.9f;
        
        RaycastHit2D hit = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            _boxCollider.bounds.size * castSize,
            angle,
            Vector2.down,
            distance,
            groundLayer);

        // Implicit conversion from RaycastHit2D to bool
        _isGrounded = hit;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_moveInput * speed, _rb.linearVelocity.y);
    }
}
