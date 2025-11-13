using UnityEngine;

public enum ItemType
{
    GrowMushroom,
    HealthMushroom,
    Star,
    Coin
}

public class Item : MonoBehaviour
{
    public ItemType type;
    
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    
    protected Rigidbody2D _rb;
    protected BoxCollider2D _boxCollider;
    
    protected bool _isMoving = true;
    
    private SpriteRenderer _spriteRenderer;
    
    private short _direction = 1;
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
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
}
