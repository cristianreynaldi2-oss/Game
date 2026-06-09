using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    // Collections (List) yang menyimpan objek bertipe Superclass (Character)
    public List<Character> daftarKarakter = new List<Character>();

    [Header("UI Input Components")]
    [SerializeField] private InputField inputNamaMonk;
    [SerializeField] private InputField inputManaMonk;
    [SerializeField] private Button tombolTambahMonk;
    [SerializeField] private Button tombolSortHP;

    [Header("Prefab Monk")]
    [SerializeField] private GameObject monkPrefab; // Prefab karakter Monk kamu

    void Start()
    {
        // Daftarkan fungsi ke tombol UI Unity
        if (tombolTambahMonk != null) tombolTambahMonk.onClick.AddListener(TambahMonkDariUI);
        if (tombolSortHP != null) tombolSortHP.onClick.AddListener(FiturSortingHP);

        // Otomatis mendata semua karakter yang ada di Scene saat game dimulai
        Character[] karakterDiScene = FindObjectsOfType<Character>();
        daftarKarakter.AddRange(karakterDiScene);
    }

    // === TARGET UI & EXCEPTION HANDLING (SCPMK0721603) ===
    public void TambahMonkDariUI()
    {
        try
        {
            string nama = inputNamaMonk.text;
            if (string.IsNullOrEmpty(nama))
            {
                throw new InputTidakValidException("Nama Monk tidak boleh kosong!");
            }

            if (!float.TryParse(inputManaMonk.text, out float mana) || mana < 0)
            {
                throw new InputTidakValidException("Mana harus berupa angka positif!");
            }

            // Spawn Monk di Unity
            GameObject newMonkObj = Instantiate(monkPrefab, Vector3.zero, Quaternion.identity);
            newMonkObj.name = nama;

            Monk scriptMonk = newMonkObj.GetComponent<Monk>();
            scriptMonk.Mana = mana;

            // Masukkan ke dalam Collections yang sama dengan Superclass (SCPMK0721601)
            daftarKarakter.Add(scriptMonk);
            
            Debug.Log($"✔ [UAS SUCCESS] Berhasil menambahkan {nama} ke dalam List CharacterManager.");
        }
        catch (InputTidakValidException ex)
        {
            // Menangkap error agar game tidak crash/freeze
            Debug.LogError($"[UAS EXCEPTION] Gagal Menambahkan: {ex.Message}");
        }
    }

    // === TARGET FITUR COLLECTIONS: SORTING DATA (SCPMK0721601) ===
    public void FiturSortingHP()
    {
        Debug.Log("--- Mengurutkan Karakter Berdasarkan HP Tertinggi ---");
        
        // Mengurutkan data di List secara Descending (menurun) berdasarkan Max Health
        daftarKarakter.Sort((x, y) => y.GetMaxHealth().CompareTo(x.GetMaxHealth()));

        foreach (Character c in daftarKarakter)
        {
            Debug.Log($"Nama: {c.gameObject.name} | Max HP: {c.GetMaxHealth()}");
        }
    }
}