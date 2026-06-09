using UnityEngine;

/// Menu interaktif Version 2.0 — Tambah LegendaryBoss, Filter, Sort.
public class GameMenuUI : MonoBehaviour
{
    private EnemyManager enemyManager;

    private string inputNama  = "";
    private string inputHp    = "";
    private string inputPhase = "";
    private string status     = "";
    private bool   showForm   = false;

    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 270, 130), "=== Game Menu V2.0 ===");

        if (GUI.Button(new Rect(20, 40, 240, 30), "Filter Legendary Boss"))
        {
            enemyManager.FilterLegendaryBoss();
            status = "Filter dijalankan. Lihat Console.";
        }

        if (GUI.Button(new Rect(20, 80, 240, 30), "Sort by HP Tertinggi"))
        {
            enemyManager.SortByHp();
            status = "Sort dijalankan. Lihat Console.";
        }

        if (GUI.Button(new Rect(20, 115, 240, 30), "Tambah Legendary Boss"))
            showForm = !showForm;

        // ── Form Input ──────────────────────────────────────────
        if (showForm)
        {
            GUI.Box(new Rect(10, 155, 270, 185), "Tambah Legendary Boss Baru");

            GUI.Label(new Rect(20, 180, 110, 22), "Legendary Title:");
            inputNama = GUI.TextField(new Rect(135, 180, 135, 22), inputNama);

            GUI.Label(new Rect(20, 212, 110, 22), "Max HP:");
            inputHp = GUI.TextField(new Rect(135, 212, 135, 22), inputHp);

            GUI.Label(new Rect(20, 244, 110, 22), "Phase Multiplier:");
            inputPhase = GUI.TextField(new Rect(135, 244, 135, 22), inputPhase);

            if (GUI.Button(new Rect(20, 280, 240, 35), "✅ Simpan"))
                TambahBossDariInput();

            GUI.Label(new Rect(20, 320, 250, 22), status);
        }
    }

    // ───────────────────────────────────────────────────────
    //  Validasi Input dengan try-catch + Custom Exception
    // ───────────────────────────────────────────────────────
    private void TambahBossDariInput()
    {
        try
        {
            // Validasi Nama
            if (string.IsNullOrWhiteSpace(inputNama))
                throw new InvalidInputException("Legendary Title tidak boleh kosong!");

            // Validasi HP
            if (!float.TryParse(inputHp, out float maxHp))
                throw new InvalidInputException("Max HP harus berupa angka!");
            if (maxHp <= 0)
                throw new InvalidInputException("Max HP harus lebih dari 0!");

            // Validasi Phase Multiplier
            if (!float.TryParse(inputPhase, out float phase) || phase < 1f)
                throw new InvalidInputException("Phase Multiplier minimal 1!");

            // Buat GameObject LegendaryBoss
            GameObject obj = new GameObject($"LegendaryBoss_{inputNama}");
            LegendaryBoss lb = obj.AddComponent<LegendaryBoss>();
            lb.LegendaryTitle   = inputNama;
            lb.PhaseMultiplier  = phase;

            enemyManager.TambahLegendaryBoss(lb);

            status      = $"✅ '{inputNama}' berhasil ditambahkan!";
            inputNama   = inputHp = inputPhase = "";
            showForm    = false;
        }
        catch (InvalidInputException e)
        {
            status = $"❌ {e.Message}";
            Debug.LogWarning($"[InvalidInputException] {e.Message}");
        }
        catch (System.Exception e)
        {
            status = "❌ Error tak terduga.";
            Debug.LogError($"[Error] {e.Message}");
        }
    }
}