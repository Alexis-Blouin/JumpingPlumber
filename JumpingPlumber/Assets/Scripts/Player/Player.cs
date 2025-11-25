using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float jumpForceEnemy = 2.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask brickLayer;
    
    [SerializeField] private float invulnarabilityTime = 2.0f;

    [SerializeField] private GameObject fireballPrefab;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private float _moveInput;
    private bool _goingRight = true;
    private Vector2 _velocity;

    private bool _isBig = false;
    private bool _isFire = false;
    private float _smallBoxColliderHeight = 1.0f;
    private float _shrinkDownForce = 500.0f;
    private float _bigBoxColliderHeight = 2.0f;
    private bool _isGrounded;

    private int _playerLayer;
    private int _enemyLayer;
    private bool _isInvulnerable = false;
    
    public Tilemap visualTilemap;
    
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
            _animator.SetBool("IsJumping", !_isGrounded);
        }
    }

    public void OnFireball(InputAction.CallbackContext context)
    {
        if (_isFire && context.started)
        {
            short direction = (short)(_spriteRenderer.flipX ? -1 : 1);
            Vector3 position = new Vector3(transform.position.x + direction, transform.position.y, transform.position.z);
            GameObject fireball = Instantiate(fireballPrefab, position, Quaternion.identity);
            fireball.GetComponent<Fireball>().SetDirection(direction);
        }
    }

    private void Update()
    {
        float distance = 0.1f * _boxCollider.bounds.size.y;
        float angle = 0.0f;
        float castSize = 0.9f;
        
        RaycastHit2D hit = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            _boxCollider.bounds.size * castSize,
            angle,
            Vector2.down,
            distance,
            groundLayer|brickLayer);

        // Implicit conversion from RaycastHit2D to bool
        _isGrounded = hit;
        _animator.SetBool("IsJumping", !_isGrounded);

        _CheckHeadHit();
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
                GetHit();
            }
        } else if (other.gameObject.TryGetComponent<Item>(out Item item))
        {
            switch (item.type)
            {
                case ItemType.GrowMushroom:
                    _Grow();
                    break;

                case ItemType.LifeMushroom:
                    break;
                
                case ItemType.FireFlower:
                    _AddFire();
                    break;

                case ItemType.Star:
                    break;
                
                case ItemType.Coin:
                    break;
            }
            
            Destroy(other.gameObject); // Destroy the item after the player used it
        }
    }

    private void _CheckHeadHit()
    {
        float distance = 0.1f * _boxCollider.bounds.size.y;
        float angle = 0.0f;
        float castSize = 0.9f;
        
        RaycastHit2D hit = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            _boxCollider.bounds.size * castSize,
            angle,
            Vector2.up,
            distance,
            brickLayer
        );

        if (hit.collider != null)
        {
            Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
            
            if (tilemap != null)
            {
                Vector3 hitPos = (Vector3)hit.point + Vector3.up * distance;
                Debug.Log("hit pos: " + hitPos);
                Vector3Int cellPos = tilemap.WorldToCell(hitPos);
                
                TileBase tile = tilemap.GetTile(cellPos);
            
                Debug.Log(tile);
                if (tile is BrickTile brick)
                {
                    brick.OnHit(cellPos, tilemap, visualTilemap);
                    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0.0f);
                }
            }
        }
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
            // Die
        }
        
        StartCoroutine(BecomeInvulnerable());
    }

    private void _Grow()
    {
        _isBig = true;
        _animator.SetBool("IsBig", _isBig);
        _boxCollider.size = new Vector2(_boxCollider.size.x, _bigBoxColliderHeight);
    }

    private void _Shrink()
    {
        _isBig = false;
        _animator.SetBool("IsBig", _isBig);
        _boxCollider.size = new Vector2(_boxCollider.size.x, _smallBoxColliderHeight);
        // Push the Player to the ground after making it slimer 
        _rb.AddForce(Vector2.down * _shrinkDownForce);
    }

    private void _AddFire()
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
}
