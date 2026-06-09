using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    protected float health;
    protected float maxHealth;
    protected float moveSpeed;
    protected Animator anim;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb   = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} terkena {amount} damage. HP: {health}/{maxHealth}");

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        anim?.SetTrigger("DieTrigger");
        Destroy(gameObject, 1.5f);
    }

    public abstract void Attack();

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}