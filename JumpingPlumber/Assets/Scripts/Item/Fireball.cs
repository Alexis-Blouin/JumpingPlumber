using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isMoving = true;
    [SerializeField] private float bounceForce = 2.0f;
    
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    
    private SpriteRenderer _spriteRenderer;
    
    private short _direction = 1;
    
    public void SetDirection (short direction) { _direction = direction; }
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
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
                HitWall();
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
                HitWall();
            }
        }

        if (isMoving)
        {
            castOrigin = _boxCollider.bounds.center;
            castSize = new Vector2(
                _boxCollider.bounds.size.x * 0.9f, 
                _boxCollider.bounds.size.y * 0.9f);
            
            RaycastHit2D hitDown = Physics2D.BoxCast(
                castOrigin,
                castSize,
                angle,
                Vector2.down,
                distance,
                groundLayer);
            
            if (hitDown)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, bounceForce);
            }
        }
    }

    private void HitWall()
    {
        isMoving = false;
        _animator.SetTrigger("Explode");
    }
    
    public void OnExplosionFinished()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_direction * speed, _rb.linearVelocity.y);
    }
}
