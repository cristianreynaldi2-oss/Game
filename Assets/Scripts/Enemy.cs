using UnityEngine;

public abstract class Enemy : Character
{
    protected float damage;
    protected float detectionRange;
    protected float attackRange; // Pindahkan ke base class agar seragam
    protected Transform target;

    // Status Enemy
    protected enum EnemyState { Patrol, Chase, Attack }
    protected EnemyState currentState = EnemyState.Patrol;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            currentState = EnemyState.Patrol;
            return;
        }

        // Hitung jarak ke player
        float dist = Vector2.Distance(transform.position, target.position);

        // ATUR STATE BERDASARKAN JARAK
        if (dist <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (dist <= detectionRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    protected virtual void FixedUpdate()
    {
        // Jalankan aksi sesuai State saat ini di FixedUpdate (aman untuk Fisika/Rigidbody)
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    protected void FlipTowardsTarget(Transform t)
    {
        if (t == null) return;
        Vector2 dir = t.position - transform.position;
        if (dir.x > 0.1f) transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
    }

    protected void FlipTowardsDirection(Vector2 dir)
    {
        if (dir.x > 0.1f) transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
    }

    // Fungsi-fungsi yang wajib diisi oleh anak kelas (Melee, Ranged, dll)
    public abstract void Patrol();
    public abstract void ChasePlayer();
    public abstract override void Attack();

    // Gambar visualisasi radius di Unity Editor
    private void OnDrawGizmosSelected()
    {
        // Radius Deteksi/Kejar (Kuning)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Radius Serang (Merah)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}