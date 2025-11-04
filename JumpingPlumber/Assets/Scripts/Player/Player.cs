using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float jumpForceEnemy = 2.0f;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private int health = 5;
    [SerializeField] private float invulnarabilityTime = 2.0f;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private float _moveInput;
    private bool _goingRight = true;
    private Vector2 _velocity;

    private bool _isGrounded;

    private int _playerLayer;
    private int _enemyLayer;
    private bool _isInvulnerable = false;
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        
        _playerLayer = LayerMask.NameToLayer("Player");
        _enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<float>();
        _animator.SetBool("IsMoving", _moveInput is > 0 or < 0);
        if (_moveInput != 0.0f)
        {
            if (_goingRight && _moveInput < 0.0f)
            {
                _spriteRenderer.flipX = true;
                _goingRight = false;
            }
            else if (!_goingRight && _moveInput > 0.0f)
            {
                _spriteRenderer.flipX = false;
                _goingRight = true;
            }
        }
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

            if (contact.normal.y > 0.5f)
            {
                enemy.JumpedOn();
                
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForceEnemy);
            }
            else
            {
                TakeDamage(enemy.GetDamage());
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (_isInvulnerable) return;
        
        health -= damage;
        Debug.Log("ouch: " + health);
        
        StartCoroutine(BecomeInvulnerable());
    }

    private IEnumerator BecomeInvulnerable()
    {
        _isInvulnerable = true;
        // Ignore collisions between player and enemy layer
        Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer, true);
        
        yield return new WaitForSeconds(invulnarabilityTime);
        
        // Re-enable collisions
        Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer, false);
        _isInvulnerable = false;
    }
}
