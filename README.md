# RPG Game - Final Project PBO

## Update Versi 2.0

Version 2.0 Update

Pada versi 2.0
Ditambahkan karakter baru bernama SwordMaster yang merupakan turunan dari class Melee. SwordMaster memiliki atribut unik ThrowSwordDamage dan mengimplementasikan polymorphism melalui override method Attack().

Selain itu ditambahkan EnemyManager yang menggunakan List<Enemy> untuk menyimpan berbagai jenis musuh dalam satu collection serta fitur sorting berdasarkan Health. Sistem juga dilengkapi dengan Custom Exception untuk memvalidasi nilai damage agar tidak bernilai negatif.

## Deskripsi

RPG Game merupakan game sederhana berbasis Unity dan C# yang dibuat untuk memenuhi tugas Final Project Pemrograman Berorientasi Objek (PBO). Game mengimplementasikan konsep Object-Oriented Programming seperti Encapsulation, Inheritance, Abstraction, Polymorphism, dan Interface.

## Requirement

### Software

- Unity
- Visual Studio / Visual Studio Code
- Git

### Bahasa Pemrograman

- C#

## Cara Menjalankan

1. Buka project menggunakan Unity.
2. Buka scene utama permainan.
3. Tekan tombol Play pada Unity Editor.
4. Gunakan tombol:
   - W = Bergerak ke atas
   - A = Bergerak ke kiri
   - S = Bergerak ke bawah
   - D = Bergerak ke kanan

## Cara Menjalankan Test

### Functional Testing

Lakukan pengujian berdasarkan skenario berikut:

- TS01 - Player bergerak ke kanan
- TS02 - Player bergerak ke kiri
- TS03 - Player bergerak ke atas
- TS04 - Player bergerak ke bawah
- TS05 - Flip Character
- TS06 - Enemy Chase
- TS07 - Enemy Idle
- TS08 - Enemy Attack
- TS09 - Health Reduction
- TS10 - Health Regeneration
- TS11 - Knockback
- TS12 - Knockback Recovery

### Hasil Pengujian

- Pass : 11
- Fail : 1

## Contributor

### Role 1 - Class Architect

Muhammad Adistya Rafif Rasendriya

Perancangan Class Diagram dan Struktur OOP.

### Role 2 - Data & Logic Engineer

Daffa Hafist Atha Kuncoro

Implementasi Logika Gameplay.

### Role 3 - UI & Robustness Engineer

Deco Akbar Prasetya

Implementasi UI dan Validasi Sistem.

### Role 4 - QA & Repository Manager

Cristian Reynaldi

Testing, Code Review, Refactoring Recommendation, dan Dokumentasi Repository.
