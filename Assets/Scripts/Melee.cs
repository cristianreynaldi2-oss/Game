using UnityEngine;

public class Melee : Enemy
{
    private bool isAttacking = false;

    // Variabel Patroli
    private Vector2 patrolDirection;
    private float patrolTimer;
    [SerializeField] private float changeMovementInterval = 2f;

    protected override void Start()
    {
        base.Start();
        maxHealth      = 60f;
        health         = maxHealth;
        moveSpeed      = 2f;
        damage         = 10f;
        
        // Pengaturan Jarak
        detectionRange = 4f;   // Masuk radius ini -> Otomatis Ikuti/Chase
        attackRange    = 1.2f; // Sangat dekat (masuk radius ini) -> Langsung Attack

        patrolDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    // STATUS 1: Patroli saat player jauh
    public override void Patrol()
    {
        patrolTimer += Time.fixedDeltaTime;
        if (patrolTimer >= changeMovementInterval)
        {
            patrolDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            patrolTimer = 0;
        }

        rb.MovePosition(rb.position + patrolDirection * (moveSpeed * 0.5f) * Time.fixedDeltaTime);
        FlipTowardsDirection(patrolDirection);

        anim?.SetBool("IsWalking", true);
    }

    // STATUS 2: Otomatis Mengikuti/Mengejar Player saat mendekat
    public override void ChasePlayer()
    {
        if (target == null || isAttacking) return;

        FlipTowardsTarget(target);

        // Gerak menuju posisi player
        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

        anim?.SetBool("IsWalking", true);
    }

    // STATUS 3: Sudah sangat dekat, langsung serang player
    public override void Attack()
    {
        if (target == null || isAttacking) return;

        FlipTowardsTarget(target);
        
        // Hentikan gerakan saat menyerang
        rb.linearVelocity = Vector2.zero; 
        isAttacking = true;

        // Picu animasi dan damage
        anim?.SetTrigger("AttackTrigger");
        anim?.SetBool("IsWalking", false);

        target.GetComponent<IDamageable>()?.TakeDamage(damage);

        // Beri jeda/cooldown serangan (misal 1 detik sebelum bisa ngejar/serang lagi)
        Invoke(nameof(ResetAttack), 1f);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
}