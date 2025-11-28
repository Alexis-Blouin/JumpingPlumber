using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    
    public void Shoot(short direction)
    {
        Vector3 position = new Vector3(transform.position.x + direction, transform.position.y, transform.position.z);
        GameObject fireball = Instantiate(fireballPrefab, position, Quaternion.identity);
        fireball.GetComponent<Fireball>().SetDirection(direction);
    }
}
