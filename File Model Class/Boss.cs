using UnityEngine;

public class Boss : Enemy
{

    [SerializeField] private float attackRange;
    [SerializeField] private float damageSpecial;

    public float AttackRange => attackRange;
    public float SpecialAttackDamage => damageSpecial;

    public override void Attack()
    {
        // Implement boss attack logic here
    }

    public override void Patrol()
    {
        // Implement boss patrol logic here
    }

    public void SpecialAttack()
    {
        // Implement boss special attack logic here
    }   
}
