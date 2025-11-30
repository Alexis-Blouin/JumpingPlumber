using UnityEngine;

public class Star : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHealth>(out var health))
        {
            health.AddStar();
            Destroy(gameObject);
        }
    }
}
