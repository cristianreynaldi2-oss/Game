using UnityEngine;

public class Range : Enemy
{
    [SerializeField] private float attackRange;

    public float AttackRange => attackRange;

    public override void Attack()
    {
        // Implement ranged attack logic here
    }

    public override void Patrol()
    {
        // Implement ranged patrol logic here
    }
}
