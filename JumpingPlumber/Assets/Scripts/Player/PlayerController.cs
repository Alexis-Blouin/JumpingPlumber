using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float jumpForceEnemy = 2.0f;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private float _moveInput;
    private Vector2 _velocity;

    private bool _isGrounded;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<float>();
        _animator.SetBool("IsMoving", _moveInput is > 0 or < 0);
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
    
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            // Gets the first contact
            ContactPoint2D contact = other.contacts[0];

            Debug.Log("Contact normal: " + contact.normal);
            if (contact.normal.y > 0.5f)
            {
                enemy.JumpedOn();
                
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForceEnemy);
            }
            else
            {
                PlayerHealth playerHealth = gameObject.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(enemy.GetDamage());
            }
        }
    }
}
