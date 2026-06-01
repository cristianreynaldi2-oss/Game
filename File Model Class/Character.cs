public abstract class Character : MonoBehaviour, DamageAble
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;

public float Health
    {
        get => health;
        protected set => health = value;
    }

public float MaxHealth => maxHealth;
public float MoveSpeed => moveSpeed;

    public virtual void takeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Health = 0;
        }
    }

    public abstract void Attack();
}