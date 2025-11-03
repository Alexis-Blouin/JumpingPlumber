using UnityEngine;

public class Shroom : Enemy
{
    public override void JumpedOn()
    {
        Debug.Log("Shroom dies");
        Destroy(gameObject);
    }
}
