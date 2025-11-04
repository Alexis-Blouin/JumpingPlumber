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
        _animator.SetTrigger("Die");
        _isMoving = false;
        // Adjusts the collider size to fit the sprite
        _boxCollider.size = new Vector2(_boxCollider.size.x, _deathBoxColliderHeight);
        // Push the Shroom to the ground after making it slimer 
        _rb.AddForce(Vector2.down * _deathDownForce);
    }
}
