using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;

    private short _direction = 1;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
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
            
            DrawBoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size * castSize, angle, Vector2.right, distance, hitRight);

            if (hitRight)
            {
                _direction = -1;
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
            
            DrawBoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size * castSize, angle, Vector2.left, distance, hitLeft);

            if (hitLeft)
            {
                _direction = 1;
            }
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_direction * speed, _rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("J'ai hit un mur, ayoye!");
        // Change direction when hitting something
        //_direction *= -1;
    }
    
    private void DrawBoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, RaycastHit2D hit)
    {
        Color color = hit ? Color.green : Color.red;

        // The end position of the cast
        Vector2 castEnd = origin + direction * distance;

        // Draw the cast line
        Debug.DrawLine(origin, castEnd, color);

        // Draw the starting box (blue)
        DrawBox(origin, size, angle, Color.cyan);

        // Draw the end box (green/red)
        DrawBox(castEnd, size, angle, color);
    }

    private void DrawBox(Vector2 center, Vector2 size, float angle, Color color)
    {
        // Convert angle to radians
        float rad = angle * Mathf.Deg2Rad;
        Vector2 right = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * size.x / 2;
        Vector2 up = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad)) * size.y / 2;

        // Box corners
        Vector2 topLeft = center + up - right;
        Vector2 topRight = center + up + right;
        Vector2 bottomLeft = center - up - right;
        Vector2 bottomRight = center - up + right;

        // Draw edges
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
    }
}
