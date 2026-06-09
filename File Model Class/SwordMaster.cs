using UnityEngine;

public class SwordMaster : Melee
{
    [SerializeField] private float throwSwordDamage;

    public float ThrowSwordDamage
    {
        get => throwSwordDamage;
        set => throwSwordDamage = value;
    }

    public override void Attack()
    {
        Debug.Log("SwordMaster throws a sword!");
    }
}