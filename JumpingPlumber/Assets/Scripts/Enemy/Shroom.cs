using UnityEngine;

public class Shroom : Enemy
{
    private float _deathDelay = 0.5f;
    
    public override void JumpedOn()
    {
        Debug.Log("Shroom dies");
        _animator.SetTrigger("JumpedOn");
        Destroy(gameObject, _deathDelay);
    }
}
