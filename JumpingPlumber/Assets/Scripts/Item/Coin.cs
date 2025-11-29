using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerStats>(out var stats))
        {
            stats.AddGold(1);
            Destroy(gameObject);
        }
    }
    
    public void OnAnimationFinished()
    {
        Destroy(gameObject);
    }
}
