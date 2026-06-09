using UnityEngine;
using System.Collections;

public class Range : Enemy
{
    private bool isAttacking = false;

    // Variabel Patroli
    private Vector2 patrolDirection;
    private float patrolTimer;
    [SerializeField] private float changeMovementInterval = 2.5f;

    [Header("Settings Jarak Jauh (Range)")]
    [SerializeField] private GameObject arrowPrefab; 
    [SerializeField] private Transform firePoint;     
    
    // KUNCI UTAMA: Beri waktu jeda antar serangan (sesuaikan dengan durasi animasi menyerangmu)
    [SerializeField] private float attackCooldown = 2f; 

    protected override void Start()
    {
        base.Start();
        maxHealth      = 50f;
        health         = maxHealth;
        moveSpeed      = 2f;
        damage         = 15f; 
        detectionRange = 8f;
        attackRange    = 6f;

        patrolDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public override void Patrol()
    {
        if (isAttacking) return; // Jika sedang menyerang, jangan patroli

        patrolTimer += Time.fixedDeltaTime;
        if (patrolTimer >= changeMovementInterval)
        {
            patrolDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            patrolTimer = 0;
        }

        rb.MovePosition(rb.position + patrolDirection * (moveSpeed * 0.5f) * Time.fixedDeltaTime);
        FlipTowardsDirection(patrolDirection);

        // Hanya nyalakan bool jalan
        anim?.SetBool("IsWalking", true);
    }

    public override void ChasePlayer()
    {
        if (target == null || isAttacking) return; 

        FlipTowardsTarget(target);

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

        // PENGAMAN TAMBAHAN:
        // Saat mengejar, pastikan trigger attack dibersihkan agar Animator tidak dipaksa menyerang lagi
        anim?.ResetTrigger("AttackTrigger");

        // Nyalakan animasi jalan
        anim?.SetBool("IsWalking", true);
    }

    // STATUS 3: Menembak dengan Pengunci Animasi
    public override void Attack()
    {
        if (target == null || isAttacking) return; 

        FlipTowardsTarget(target);

        rb.linearVelocity = Vector2.zero;
        isAttacking = true;

        // Matikan bool jalan agar Animator tahu musuh sedang diam/menyerang
        anim?.SetBool("IsWalking", false);
    
        anim?.SetTrigger("AttackTrigger");

        StartCoroutine(ResetTriggerNextFrame());
        StartCoroutine(AttackSequence());
    }

    // Coroutine khusus untuk membersihkan sampah parameter Animator
    private IEnumerator ResetTriggerNextFrame()
    {
        yield return null; 
        anim?.ResetTrigger("AttackTrigger");
    }

    private IEnumerator AttackSequence()
    {
        // JEDA 1: Tunggu gerakan ancang-ancang animasi (misal 0.4 detik)
        yield return new WaitForSeconds(0.4f);

        // Panah muncul
        ShootArrow();

        // JEDA 2: Tunggu sisa durasi animasi menyerang/cooldown
        float remainingTime = Mathf.Max(0.1f, attackCooldown - 0.4f);
        yield return new WaitForSeconds(remainingTime);

        // Selesai cooldown, musuh boleh bergerak/menyerang lagi
        ResetAttack();
    }

    private void ShootArrow()
    {
        if (arrowPrefab != null && firePoint != null && target != null)
        {
            GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            Vector3 direction = target.position - firePoint.position;

            Arrow arrowScript = arrowObj.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.Setup(direction, damage);
            }
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
}