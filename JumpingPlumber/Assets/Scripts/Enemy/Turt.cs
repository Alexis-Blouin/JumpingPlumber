using UnityEngine;

public class Turt : Enemy
{
    public override void JumpedOn()
    {
        Debug.Log("Turt transforms in shell");
        Destroy(gameObject);
    }
}
