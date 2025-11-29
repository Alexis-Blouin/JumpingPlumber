using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float flagPoleSpeed = 4.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float jumpForceEnemy = 8.0f;
    // [SerializeField] private float jumpForceDead = 4.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask brickLayer;
    
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private float _moveInput;
    private bool _goingRight = true;
    private Vector2 _velocity;

    private bool _isOnFlag = false;
    private bool _isGrounded;
    
    private PlayerCombat _playerCombat;
    private PlayerHealth _playerHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        
        _playerCombat = GetComponent<PlayerCombat>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<float>();
        _animator.SetBool("IsMoving", _moveInput is > 0 or < 0);
        if (_moveInput != 0.0f && _playerHealth.IsAlive() && !_isOnFlag)
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
        if (_isGrounded && _playerHealth.IsAlive())
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _isGrounded = false;
            _animator.SetBool("IsJumping", !_isGrounded);
        }
    }

    public void OnFireball(InputAction.CallbackContext context)
    {
        if (_playerHealth.IsFire() && context.started)
        {
            short direction = (short)(_spriteRenderer.flipX ? -1 : 1);
            _playerCombat.Shoot(direction);
        }
    }

    // Update is called once per frame
    void Update()
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

        if (_isOnFlag)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - flagPoleSpeed * Time.deltaTime, transform.position.z);
            if (_isGrounded)
            {
                _isOnFlag = false;
                _rb.gravityScale = 3;
                _animator.SetBool("IsOnFlagPole", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_playerHealth.IsAlive() && !_isOnFlag)
        {
            _rb.linearVelocity = new Vector2(_moveInput * speed, _rb.linearVelocity.y);
        }
    }

    public void OnFlag()
    {
        _isOnFlag = true;
        _rb.gravityScale = 0;
        _rb.linearVelocity = Vector2.zero;
        _animator.SetBool("IsOnFlagPole", true);
    }

    public void JumpOnEnemy()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForceEnemy);
    }
}
