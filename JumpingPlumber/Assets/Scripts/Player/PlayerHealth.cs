using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float jumpForceDead = 12.0f;
    [SerializeField] private float invulnarabilityTime = 2.0f;
    
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    
    private bool _isBig = false;
    private bool _isFire = false;
    private bool _isAlive = true;
    private float _smallBoxColliderHeight = 1.0f;
    private float _shrinkDownForce = 500.0f;
    private float _bigBoxColliderHeight = 2.0f;
    private bool _isInvulnerable = false;
    
    private int _playerLayer;
    private int _enemyLayer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        
        _playerLayer = LayerMask.NameToLayer("Player");
        _enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    public void GetHit()
    {
        if (_isInvulnerable) return;

        if (_isFire)
        {
            _RemoveFire();
        }
        else if (_isBig)
        {
            _Shrink();
        }
        else
        {
            _Die();
        }
        
        StartCoroutine(BecomeInvulnerable());
    }
    
    public void Grow()
    {
        if (!_isBig) {
            _isBig = true;
            _animator.SetBool("IsBig", _isBig);
            _boxCollider.size = new Vector2(_boxCollider.size.x, _bigBoxColliderHeight);
        }
    }

    private void _Shrink()
    {
        _isBig = false;
        _animator.SetBool("IsBig", _isBig);
        _boxCollider.size = new Vector2(_boxCollider.size.x, _smallBoxColliderHeight);
        // Push the Player to the ground after making it slimer 
        _rb.AddForce(Vector2.down * _shrinkDownForce);
    }

    private void _Die()
    {
        _isAlive = false;
        _animator.SetTrigger("Die");
        _boxCollider.enabled = false;
        // Stop any possible horizontal movement
        _rb.linearVelocity = new Vector2(0.0f, jumpForceDead);
        
        // TODO Pause the enemies and other stuff when he dies
    }

    public void AddFire()
    {
        _isFire = true;
        _animator.SetBool("IsFire", _isFire);
    }

    private void _RemoveFire()
    {
        _isFire = false;
        _animator.SetBool("IsFire", _isFire);
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

    public bool IsAlive()
    {
        return _isAlive;
    }

    public bool IsFire()
    {
        return _isFire;
    }
}
