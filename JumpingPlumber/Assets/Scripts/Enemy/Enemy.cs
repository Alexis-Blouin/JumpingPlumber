using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask groundLayer;
    
    protected Rigidbody2D _rb;
    protected BoxCollider2D _boxCollider;
    protected Animator _animator;

    protected bool _isMoving = true;
    protected float _deathDownForce = 500.0f;
    protected float _deathBoxColliderHeight;
    
    private SpriteRenderer _spriteRenderer;

    private short _direction = 1;
    
    public abstract void JumpedOn();

    public int GetDamage()
    {
        return damage;
    }
    
    protected void Initialize()
    {
        _spriteRenderer =  GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 castOrigin = _boxCollider.bounds.center + Vector3.up * 0.1f;
        Vector2 castSize = new Vector2(
            _boxCollider.bounds.size.x * 0.9f, 
            _boxCollider.bounds.size.y * 0.5f);
        float distance = 0.1f;
        float angle = 0.0f;

        if (_direction == 1)
        {
            RaycastHit2D hitRight = Physics2D.BoxCast(
                castOrigin,
                castSize,
                angle,
                Vector2.right,
                distance,
                groundLayer);
            
            if (hitRight)
            {
                ChangeDirection();
            }
        }
        else
        {
            RaycastHit2D hitLeft = Physics2D.BoxCast(
                castOrigin,
                castSize,
                angle,
                Vector2.left,
                distance,
                groundLayer);
            
            if (hitLeft)
            {
                ChangeDirection();
            }
        }
    }

    private void ChangeDirection()
    {
        _direction = (short)-_direction;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            _rb.linearVelocity = new Vector2(_direction * speed, _rb.linearVelocity.y);
        }
    }
    
    
    
    public void Burn()
    {
        _animator.SetTrigger("Die");
        _isMoving = false;
        _rb.linearVelocity = new Vector2(0.0f, _rb.linearVelocity.y);
        _rb.freezeRotation = false;
        _rb.angularVelocity = Random.Range(-600f, 600f);
        // Adjusts the collider size to fit the sprite
        _boxCollider.size = new Vector2(_boxCollider.size.x, _deathBoxColliderHeight);
        _boxCollider.isTrigger = true;
        // Push the Enemy upward a little before going down
        _rb.AddForce(Vector2.up * 10.0f);
        Destroy(gameObject, 5.0f);
    }
}
