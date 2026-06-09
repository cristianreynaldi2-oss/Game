using UnityEngine;
using System.Collections;

/// <summary>
/// LegendaryBoss — Subclass dari Boss.
/// Boss fase-ganda: makin rendah HP, makin kuat serangannya.
/// </summary>
public class LegendaryBoss : Boss
{
    // ═══════════════════════════════════════════════════════
    //  Atribut Unik — Enkapsulasi (Getter & Setter)
    // ═══════════════════════════════════════════════════════

    [Header("Legendary Boss Settings")]
    [SerializeField] private string legendaryTitle = "The Ancient Destroyer";
    [SerializeField] private float  phaseMultiplier = 1f;   // naik saat HP kritis
    private bool isEnraged = false;

    public string LegendaryTitle
    {
        get => legendaryTitle;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidInputException("Legendary title tidak boleh kosong!");
            legendaryTitle = value;
        }
    }

    public float PhaseMultiplier
    {
        get => phaseMultiplier;
        set => phaseMultiplier = Mathf.Max(1f, value);
    }

    public bool IsEnraged => isEnraged;

    // ═══════════════════════════════════════════════════════
    //  Inisialisasi
    // ═══════════════════════════════════════════════════════

    protected override void Start()
    {
        base.Start();
        maxHealth       = 600f;   // 2× Boss biasa
        health          = maxHealth;
        moveSpeed       = 2.5f;
        damage          = 40f;
        phaseMultiplier = 1f;

        Debug.Log($"[LEGENDARY] {legendaryTitle} telah muncul!");
    }

    // ═══════════════════════════════════════════════════════
    //  Override TakeDamage — Polimorfisme
    //  Saat HP < 50%, masuk fase ENRAGE (phaseMultiplier naik)
    // ═══════════════════════════════════════════════════════

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        if (!isEnraged && health <= maxHealth * 0.5f)
        {
            isEnraged       = true;
            phaseMultiplier = 2.5f;
            moveSpeed       = 4f;

            Debug.Log($"[LEGENDARY] {legendaryTitle} ENRAGED! " +
                      $"Multiplier: {phaseMultiplier}× | Speed: {moveSpeed}");
        }
    }

    // ═══════════════════════════════════════════════════════
    //  Override Attack — Polimorfisme
    //  Damage dikalikan phaseMultiplier
    // ═══════════════════════════════════════════════════════

    public override void Attack()
    {
        if (target == null) return;

        FlipTowardsTarget(target);

        float finalDamage = damage * phaseMultiplier;
        target.GetComponent<IDamageable>()?.TakeDamage(finalDamage);

        string faseLabel = isEnraged ? "⚡ ENRAGED" : "Normal";
        Debug.Log($"[LEGENDARY] {legendaryTitle} menyerang! " +
                  $"Damage: {finalDamage} ({faseLabel}, ×{phaseMultiplier})");

        anim?.SetTrigger("AttackTrigger");
    }

    // ═══════════════════════════════════════════════════════
    //  TampilkanDetail — untuk UI / debug
    // ═══════════════════════════════════════════════════════

    public string TampilkanDetail()
    {
        string fase = isEnraged ? "⚡ ENRAGED" : "Normal";
        return $"[LEGENDARY BOSS]\n"              +
               $"Nama  : {legendaryTitle}\n"      +
               $"HP    : {health}/{maxHealth}\n"  +
               $"Fase  : {fase}\n"                +
               $"DMG   : {damage * phaseMultiplier} (×{phaseMultiplier})";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}