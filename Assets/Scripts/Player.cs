using UnityEngine;

public class Player : Character
{
    protected float damage;
    private bool isAttacking = false;

    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.down;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        maxHealth = 100f;
        health    = maxHealth;
        moveSpeed = 5f;
        damage    = 25f;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        UpdateAnimator();

        if (Input.GetKeyDown(KeyCode.K) && !isAttacking)
            Attack();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    private void UpdateAnimator()
    {
        float speed = moveInput.magnitude;
        anim?.SetBool("IsWalking", speed > 0.1f);

        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDir = moveInput;

            // ✅ Flip sprite berdasarkan arah horizontal
            if (moveInput.x > 0.1f)
                transform.localScale = new Vector3(1, 1, 1);   // hadap kanan (normal)
            else if (moveInput.x < -0.1f)
                transform.localScale = new Vector3(-1, 1, 1);  // hadap kiri (flip)
        }
    }

    public override void Attack()
    {
        isAttacking = true;
        anim?.SetTrigger("AttackTrigger");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        bool kenaMusuh = false;

        foreach (var hit in hits)
        {
            if (hit.gameObject == this.gameObject) continue;

            IDamageable target = hit.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
                kenaMusuh = true;
                Debug.Log($"Player menyerang {hit.gameObject.name}, damage: {damage}");
            }
        }

        if (!kenaMusuh)
            Debug.Log("Player menyerang tapi tidak ada musuh dalam jangkauan.");

        Invoke(nameof(ResetAttack), 0.8f);
    }

    private void ResetAttack() => isAttacking = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

    public void RestoreFullHealth()
    {
        // Karena variabel 'health' dan 'maxHealth' di class Character bersifat 'protected',
        // kelas turunannya (Player) bisa langsung mengakses dan mengubah nilainya.
        health = maxHealth; 
        Debug.Log($"HP Player dipulihkan sepenuhnya! HP: {health}/{maxHealth}");
    }
}