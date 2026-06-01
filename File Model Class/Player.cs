using UnityEngine;

public class Player : Character
{
    [SerializeField] private float damage;

    public float Damage => damage;

    public override void Attack()
    {
        // Implement player attack logic here
    }
    public void movement()
    {
        // Implement player movement logic here
    }
}