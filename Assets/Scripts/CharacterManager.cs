using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// POIN B: MEMBUAT CUSTOM EXCEPTION
public class InputKarakterValidasiException : Exception
{
    public InputKarakterValidasiException(string message) : base(message) { }
}

public class CharacterManager : MonoBehaviour
{
    // List Koleksi Superclass (Character)
    private List<Character> daftarKarakter = new List<Character>();

    // Variabel bantu untuk menampung inputan user di layar Unity
    private string inputNama = "Input Nama Karakter";
    private string inputHP = "100";
    private string statusLog = "Status: Siap.";

    // POIN A: MEMBUAT MENU INTERAKTIF (Menggunakan GUI Bawaan Unity)
    private void OnGUI()
    {
        // Kotak Area Menu
        GUI.Box(new Rect(10, 10, 300, 320), "MENU INTERAKTIF KARAKTER");

        // Input Text untuk Nama
        GUI.Label(new Rect(20, 40, 80, 20), "Nama:");
        inputNama = GUI.TextField(new Rect(100, 40, 190, 20), inputNama);

        // Input Text untuk HP
        GUI.Label(new Rect(20, 70, 80, 20), "HP Awal:");
        inputHP = GUI.TextField(new Rect(100, 70, 190, 20), inputHP);

        // TOMBOL 1: Tambah Karakter Baru (Subclass) ke dalam List
        if (GUI.Button(new Rect(20, 110, 270, 30), "Tambah Enemy Baru (Subclass)"))
        {
            TambahKarakterProses();
        }

        // TOMBOL 2: Panggil Fitur Sorting HP Tertinggi
        if (GUI.Button(new Rect(20, 150, 270, 30), "Urutkan HP Tertinggi (Sorting)"))
        {
            UrutkanKarakter();
        }

        // Tampilan Status Log di Layar
        GUI.Label(new Rect(20, 200, 270, 100), statusLog);
    }


    // POIN C: PENERAPAN TRY-CATCH DENGAN CUSTOM EXCEPTION
    private void TambahKarakterProses()
    {
        try
        {
            // Validasi 1: Apakah input nama kosong?
            if (string.IsNullOrEmpty(inputNama) || inputNama == "Input Nama Karakter")
            {
                throw new InputKarakterValidasiException("Eror: Nama karakter tidak boleh kosong!");
            }

            // Validasi 2: Apakah format HP berupa angka valid?
            if (!float.TryParse(inputHP, out float hasilHP))
            {
                throw new InputKarakterValidasiException("Eror: HP harus berupa angka yang valid!");
            }

            // Validasi 3: Apakah HP bernilai minus atau nol?
            if (hasilHP <= 0)
            {
                throw new InputKarakterValidasiException("Eror: HP Karakter tidak boleh kurang dari atau sama dengan 0!");
            }

            // JIKA LOLOS VALIDASI -> Buat Game Object Baru secara runtime
            GameObject goBaru = new GameObject(inputNama);

            // Menempelkan Subclass Enemy (longMelee) ke GameObject baru
            longMelee komponenBaru = goBaru.AddComponent<longMelee>();
            
            // Masukkan ke dalam koleksi List yang sama (Polimorfisme bekerja di sini)
            daftarKarakter.Add(komponenBaru);

            statusLog = $"Sukses: Berhasil menambah objek [{inputNama}] dengan HP: {hasilHP}";
            Debug.Log(statusLog);
        }
        catch (InputKarakterValidasiException ex)
        {
            // Menangkap custom exception jika input data salah/kosong
            statusLog = ex.Message;
            Debug.LogWarning(ex.Message); // Game tidak crash, hanya memberi peringatan kuning
        }
        catch (Exception ex)
        {
            // Menangkap eror tidak terduga lainnya
            statusLog = $"Eror Sistem: {ex.Message}";
            Debug.LogError(ex.Message);
        }
    }

    // Fitur Sorting menggunakan LINQ
    private void UrutkanKarakter()
    {
        if (daftarKarakter.Count == 0)
        {
            statusLog = "Peringatan: List kosong! Tambah karakter dulu.";
            return;
        }

        // Proses Sorting menggunakan LINQ berdasarkan HP saat ini
        List<Character> daftarTerurut = daftarKarakter.OrderByDescending(k => k.GetHealth()).ToList();

        // Cetak hasil di Console Unity
        string hasilCetak = "--- HASIL SORTING HP TERTINGGI ---\n";
        for (int i = 0; i < daftarTerurut.Count; i++)
        {
            hasilCetak += $"{i+1}. {daftarTerurut[i].gameObject.name} (HP: {daftarTerurut[i].GetHealth()})\n";
        }
        
        statusLog = "Sukses Sorting! Cek Console Unity (Shift+Ctrl+C)";
        Debug.Log(hasilCetak);
    }
}