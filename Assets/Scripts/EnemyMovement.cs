using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 2;
    public Transform target;

    private float attackRange = 1;

    private EnemyState state;

    public float directionX;
    public float directionY;

    public Animator anim;
    private int facingDirection = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    void State(EnemyState newState)
    {
        state = newState;
    }

    public bool Chase()
    {
        anim.SetBool("attack", false);
        if (Vector2.Distance(transform.position, target.transform.position) <= attackRange)
        {
            state = EnemyState.Attack;
            return true;
        }
        if (
            (rb.velocity.x > 0 && facingDirection < 0) || (rb.velocity.x < 0 && facingDirection > 0)
        )
        {
            Flip();
        }

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;

        anim.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("vertical", Mathf.Abs(rb.velocity.y));

        return false;
    }

    void Idle()
    {
        rb.velocity = Vector2.zero;
        anim.SetBool("attack", false);
    }

    void Attack()
    {
        if (target.GetComponent<PlayerHealt>().GetHealth() <= 0)
        {
            state = EnemyState.Idle;
            return;
        }
        rb.velocity = Vector2.zero;
        anim.SetBool("attack", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = EnemyState.Chase;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = EnemyState.Idle;
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
}
