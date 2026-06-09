using UnityEngine;
using System.Collections cone; // Dibutuhkan untuk IEnumerator

public class LongMelee : Enemy // Sesuai konvensi PascalCase untuk nama kelas
{
    [SerializeField] private Longmeleedash befoDashPrefab; // Diganti agar jelas ini prefab
    [SerializeField] private float changeMovementInterval = 2f;
    
    private Vector2 patrolDirection;
    private float patrolTimer;
    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
        maxHealth      = 100f;
        health         = maxHealth;
        moveSpeed      = 1.0f;
        damage         = 25f;
        
        // Pengaturan Jarak
        detectionRange = 6f;   // Masuk radius ini -> Otomatis Ikuti/Chase
        attackRange    = 5f;   // Sangat dekat (masuk radius ini) -> Langsung Attack
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
        
        // Hentikan gerakan saat menyerang (sesuaikan ke linearVelocity jika pakai Unity terbaru)
        rb.velocity = Vector2.zero; 
        isAttacking = true;

        // Picu animasi dan damage
        anim?.SetTrigger("AttackTrigger");
        anim?.SetBool("IsWalking", false);

        target.GetComponent<IDamageable>()?.TakeDamage(damage);

        // Beri jeda/cooldown serangan 4 detik
        Invoke(nameof(ResetAttack), 4f);
    }

    public void SpecialAttack()
    {
        if (target == null || isAttacking) return;

        FlipTowardsTarget(target);

        rb.velocity = Vector2.zero;
        isAttacking = true;

        anim?.SetTrigger("SpecialAttackTrigger");
        anim?.SetBool("IsWalking", false);

        // Memulai Coroutine untuk jeda peluncuran dash efek
        StartCoroutine(SpawnDashEffectRoutine("SpecialAttackTrigger"));
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void FlipTowardsDirection(Vector2 direction)
    {
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // Penyatuan Coroutine yang duplikat sebelumnya
    private System.Collections.IEnumerator SpawnDashEffectRoutine(string triggerName)
    {
        yield return null; // Tunggu satu frame
        anim?.ResetTrigger(triggerName);

        // Memastikan prefab dan firePoint (dari parent Enemy) tersedia
        if (befoDashPrefab != null && target != null && firePoint != null)
        {
            // Spawn menggunakan posisi dari firePoint musuh
            Longmeleedash dashObj = Instantiate(befoDashPrefab, firePoint.position, Quaternion.identity);
            Vector3 direction = target.position - firePoint.position;

            dashObj.Setup(direction);
        }

        // Tunggu jeda animasi/visual sebelum musuh bisa bergerak lagi
        yield return new WaitForSeconds(1.5f); 
        ResetAttack();
    }
}