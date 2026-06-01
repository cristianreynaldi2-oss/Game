using UnityEngine;

public class Melee : Enemy
{
    [SerializeField] private float attackRange;

    public float AttackRange => attackRange;
    public override void Attack()
    {
        // Implement melee attack logic here
    }

    public override void Patrol()
    {
        // Implement melee patrol logic here
    }
}
