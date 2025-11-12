using UnityEngine;

public class Turt : Enemy
{
    private void Start()
    {
        Initialize();
        _deathBoxColliderHeight = 0.85f;
    }
    
    public override void JumpedOn()
    {
        // If turt is not dead yet, we kill it
        if (!_animator.GetBool("Die"))
        {
            _animator.SetTrigger("Die");
            // Adjusts the collider size to fit the sprite
            _boxCollider.size = new Vector2(_boxCollider.size.x, _deathBoxColliderHeight);
            // Push the Shroom to the ground after making it slimer 
            _rb.AddForce(Vector2.down * _deathDownForce);
        }
        // Each time we jump on it, it starts of stops moving
        _isMoving = !_isMoving;
    }
}
