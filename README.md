# Dokumentasi Pembaruan Sistem Karakter RPG (UAS Versi 2.0)

Dokumentasi ini dibuat untuk memenuhi seluruh target capaian mata kuliah (**SCPMK**) pada ujian akhir semester. Pembaruan pada versi 2.0 ini berfokus pada perluasan hierarki karakter Unity menggunakan konsep Object-Oriented Programming (OOP) murni, pengelolaan data terpusat (*Collections*), serta mekanisme *Exception Handling* yang kokoh pada antarmuka pengguna (UI).

---

## 1. Ikhtisar Perubahan & Fitur Baru (Mulai dari Subclass Monk)

Dalam pembaruan ini, struktur dasar kelas `Character` bawaan Unity dipertahankan (untuk menjaga fungsionalitas komponen `Player`, `EnemyMelee`, `EnemyRange`, dan `EnemyBoss`). Perubahan dan penambahan fitur baru diimplementasikan secara modular melalui komponen-komponen berikut:

### A. Penambahan Subclass Baru: `Monk.cs`
Mengimplementasikan kelas turunan (*Subclass*) baru dari `Character` untuk memenuhi target **SCPMK0721602 (Arsitektur OOP & Polimorfisme)**.
* **Enkapsulasi Murni:** Menyembunyikan data internal menggunakan *private backing field* `mana` dan menyediakan akses publik yang aman melalui Properti `Mana` (Getter/Setter).
* **Polimorfisme (Override):** Melakukan metode *override* pada fungsi `Attack()` dari kelas induk untuk menghasilkan mekanik serangan unik (*Palm Strike*) berbasis konsumsi Mana.
* **Interaksi Real-time Unity:** Dilengkapi dengan logika *Trigger Collider 2D* untuk mendeteksi area interaksi dengan objek `Player` dan UI tombol pemulihan (*Heal*).

### B. Arsitektur Pengelola Terpusat: `CharacterManager.cs`
Mengimplementasikan sistem manajemen karakter terpusat untuk memenuhi target **SCPMK0721601 (Struktur Data & Logika Sorting)**.
* **Polymorphic Collections:** Memanfaatkan `List<Character>` untuk menampung seluruh objek karakter yang aktif di dalam Scene Unity (`Player`, `EnemyMelee`, `EnemyRange`, `EnemyBoss`, dan `Monk`) secara dinamis saat *runtime*.
* **Algoritma Sorting (Descending):** Menyediakan fitur pengurutan seluruh data karakter di dalam *List* berdasarkan nilai `maxHealth` tertinggi hingga terendah menggunakan ekspresi Lambda, kemudian mencetak hasilnya secara rapi di Console Unity.

### C. UI & Exception Handling: `InputTidakValidException.cs`
Mengimplementasikan pembatasan error tingkat tinggi pada input pengguna untuk memenuhi target **SCPMK0721603 (UI & Exception Handling)**.
* **Custom Exception:** Membuat kelas pengecualian kustom `InputTidakValidException` yang diwarisi dari `System.Exception`.
* **Mekanisme Try-Catch:** Membungkus proses pembuatan karakter via UI dalam blok `try-catch`. Jika pengguna memasukkan nama kosong atau nilai mana yang tidak valid (bukan angka/negatif), sistem akan menangkap error tersebut, menampilkan pesan peringatan di Console, dan memastikan game **tidak mengalami crash/freeze**.

---

## 2. Struktur Hierarki Kelas (OOP)

Berikut adalah visualisasi hierarki pewarisan kelas setelah penambahan kelas `Monk`:

```text
         ┌─────────────────────────────────┐
         │     Character (MonoBehaviour)   │  <── Kelas Induk (Superclass)
         └─────────────────────────────────┘
           ▲       ▲         ▲        ▲
   ┌───────┘       │         │        └────────┐
┌──┴───┐     ┌─────┴─────┐ ┌─┴─────────┐    ┌──┴──┐
│Player│     │EnemyMelee │ │EnemyBoss  │    │Monk │ <── Subclass Baru (UAS V2.0)
└──────┘     └───────────┘ └───────────┘    └─────┘
                                              │
                                              └── Properti: Mana (Float)
                                              └── Override: Attack()