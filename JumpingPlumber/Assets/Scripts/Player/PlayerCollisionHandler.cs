using System;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    
    private PlayerHealth _playerHealth;
    private PlayerController _playerController;

    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (_playerHealth.IsStar())
            {
                enemy.Burn();
            }
            else
            {
                // Gets the first contact
                ContactPoint2D contact = other.contacts[0];

                if (contact.normal.y > 0.5f)
                {
                    enemy.JumpedOn();
                
                    _playerController.JumpOnEnemy();
                }
                else
                {
                    _playerHealth.GetHit();
                }
            }
        } else if (other.gameObject.TryGetComponent<Item>(out Item item))
        {
            switch (item.type)
            {
                case ItemType.GrowMushroom:
                    _playerHealth.Grow();
                    break;

                case ItemType.LifeMushroom:
                    break;
                
                case ItemType.FireFlower:
                    _playerHealth.AddFire();
                    break;

                case ItemType.Star:
                    break;
                
                case ItemType.Coin:
                    break;
            }
            
            Destroy(other.gameObject); // Destroy the item after the player used it
        } else if (other.gameObject.CompareTag("Flag"))
        {
            _playerController.OnFlag();
            
            // Remove pole's collider
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            // Make the flag go down with the player
            other.gameObject.GetComponentInParent<FlagPole>().canGoDown = true;
        }
    }
}
