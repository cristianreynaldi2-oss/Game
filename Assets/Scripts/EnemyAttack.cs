using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 1;
    private Rigidbody2D rb;
    public GameObject player;
    private EnemyMovement self;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        self = GetComponent<EnemyMovement>();
    }

    public void Attack()
    {
        if (self.Chase())
        {
            if (isAttacking)
            {
                AttackCooldown();
                return;
            }
            isAttacking = true;
            Debug.Log("attack " + player.GetComponent<PlayerHealt>().GetHealth());
            player.GetComponent<PlayerHealt>().ChangeHealth(-damage);
            player.GetComponent<PlayerMovement>().Knockback(transform, 10, .2f);
        }
    }

    void AttackCooldown()
    {
        isAttacking = false;
    }
}
