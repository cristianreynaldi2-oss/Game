using UnityEngine;
using System.Collections;

public class Boss : Enemy
{
    protected float damageSpecial;

    [Header("Settings Cooldown Jurus")]
    [SerializeField] private float specialCooldown = 8f; 
    private float specialTimer    = 0f;
    private bool  isAttacking     = false;

    // Variabel Patroli
    private Vector2 patrolDirection;
    private float patrolTimer;
    [SerializeField] private float changeMovementInterval = 3f; 

    [Header("Settings Lempar Tombak (Mirip Range)")]
    [SerializeField] private GameObject spearPrefab; // Taruh prefab tombak terbang di sini
    [SerializeField] private Transform firePoint;     // Titik tempat tombak muncul (misal di tangan Boss)

    protected override void Start()
    {
        base.Start();
        maxHealth      = 300f;
        health         = maxHealth;
        moveSpeed      = 2f;
        damage         = 30f;       // Damage tusukan jarak dekat
        damageSpecial  = 60f;       // Damage lemparan tombak jarak jauh
        
        detectionRange = 15f; 
        attackRange    = 2f;  // Jarak tusukan dekat

        patrolDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        specialTimer = specialCooldown * 0.5f; 
    }

    protected override void Update()
    {
        base.Update();

        if (currentState == EnemyState.Chase || currentState == EnemyState.Attack)
        {
            if (specialTimer < specialCooldown)
            {
                specialTimer += Time.deltaTime;
            }
        }
    }

    public override void Patrol()
    {
        if (isAttacking) return;

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

    public override void ChasePlayer()
    {
        if (target == null || isAttacking) return;

        // Jika cooldown LEMPAR TOMBAK sudah siap, langsung potong gerakan dan lempar!
        if (specialTimer >= specialCooldown)
        {
            specialTimer = 0f;
            SpecialAttack();
            return;
        }

        FlipTowardsTarget(target);

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

        anim?.ResetTrigger("AttackTrigger");
        anim?.ResetTrigger("SpecialAttackTrigger");
        anim?.SetBool("IsWalking", true);
    }

    public override void Attack()
    {
        if (target == null || isAttacking) return;

        if (specialTimer >= specialCooldown)
        {
            specialTimer = 0f;
            SpecialAttack();
            return;
        }

        FlipTowardsTarget(target);

        rb.linearVelocity = Vector2.zero;
        isAttacking = true;

        anim?.SetBool("IsWalking", false);
        anim?.SetTrigger("AttackTrigger");

        // Serangan dekat biasa (tusuk)
        target.GetComponent<IDamageable>()?.TakeDamage(damage);
        
        StartCoroutine(ResetTriggerNextFrame("AttackTrigger"));
        Invoke(nameof(ResetAttack), 1f); 
    }

    // SERANGAN KHUSUS: Lempar Tombak
    public void SpecialAttack()
    {
        if (isAttacking) return;

        rb.linearVelocity = Vector2.zero;
        isAttacking = true;
        
        anim?.SetBool("IsWalking", false);
        anim?.SetTrigger("SpecialAttackTrigger");

        StartCoroutine(ResetTriggerNextFrame("SpecialAttackTrigger"));
        StartCoroutine(SpearThrowSequence());
    }

    private IEnumerator SpearThrowSequence()
    {
        // JEDA 1: Tunggu gerakan ancang-ancang tangan boss mundur ke belakang (misal 0.5 detik)
        yield return new WaitForSeconds(0.5f);

        // EKSEKUSI MUNCIULKAN TOMBAK (Sama persis dengan sistem Range)
        if (spearPrefab != null && firePoint != null && target != null)
        {
            GameObject spearObj = Instantiate(spearPrefab, firePoint.position, Quaternion.identity);
            Vector3 direction = target.position - firePoint.position;

            SpearProjectile spearScript = spearObj.GetComponent<SpearProjectile>();
            if (spearScript != null)
            {
                spearScript.Setup(direction, damageSpecial);
            }
        }

        // JEDA 2: Tunggu sisa visual animasi melempar selesai sampai tuntas (misal 1 detik)
        yield return new WaitForSeconds(1.0f);

        ResetAttack();
    }

    private IEnumerator ResetTriggerNextFrame(string triggerName)
    {
        yield return null;
        anim?.ResetTrigger(triggerName);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
}