using UnityEngine;
using UnityEngine.UI; // Wajib dimasukkan untuk mengakses komponen UI Image

public class HealthBarUI : MonoBehaviour
{
    [Header("UI Element")]
    [SerializeField] private Image healthBarFill; // Taruh Image bertipe Filled kamu di sini

    [Header("Character Reference")]
    [SerializeField] private Character character; // Referensi ke skrip Character (Base Class)

    private void Start()
    {
        // Jaga-jaga jika lupa memasukkan referensi karakter via Inspector,
        // skrip akan mencoba mencarinya di objek yang sama atau di Player.
        if (character == null)
        {
            character = GetComponentInParent<Character>();
            if (character == null)
            {
                character = GameObject.FindWithTag("Player")?.GetComponent<Character>();
            }
        }
    }

    private void Update()
    {
        if (character != null && healthBarFill != null)
        {
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        // Hitung persentase darah (0.0 sampai 1.0)
        // Gunakan float division agar hasilnya presisi
        float currentHealth = character.GetHealth();
        float maxHealth = character.GetMaxHealth();

        // Antisipasi pembagian dengan angka nol agar tidak error
        if (maxHealth > 0)
        {
            float healthPercentage = currentHealth / maxHealth;

            // Masukkan hasil persentase ke fillAmount Image UI
            healthBarFill.fillAmount = Mathf.Clamp01(healthPercentage);
        }
    }
}