using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 5;
    [SerializeField] private float invulnarabilityTime = 2.0f;

    private int _playerLayer;
    private int _enemyLayer;
    private bool _isInvulnerable = false;
    
    void Start()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
        _enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    public void TakeDamage(int damage)
    {
        if (_isInvulnerable) return;
        
        health -= damage;
        Debug.Log("ouch: " + health);
        
        StartCoroutine(BecomeInvulnerable());
    }

    private IEnumerator BecomeInvulnerable()
    {
        _isInvulnerable = true;
        
        // Ignore collisions between player and enemy layer
        Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer, true);
        Debug.Log("Player is invulnerable!");
        
        yield return new WaitForSeconds(invulnarabilityTime);
        
        // Re-enable collisions
        Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer, false);
        _isInvulnerable = false;
        
        Debug.Log("Player is vulnerable again!");
    }
}
