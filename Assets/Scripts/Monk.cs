using UnityEngine;
using UnityEngine.UI;

public class Monk : Character
{
    // === TARGET OOP & ENKAPSULASI ===
    // Atribut unik lengkap dengan Getter dan Setter (Enkapsulasi)
    private float mana;
    public float Mana 
    { 
        get { return mana; } 
        set { mana = value; } 
    }

    [Header("Monk UI Settings")]
    [SerializeField] private GameObject interactionPromptUI; 
    [SerializeField] private Button healButton;               

    private Player playerInRange; 

    protected override void Awake()
    {
        base.Awake();
        
        // Inisialisasi atribut unik
        this.mana = 100f; 
        
        // Setup UI
        if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
        if (healButton != null) healButton.onClick.AddListener(OnHealButtonClicked);
    }

    // === TARGET POLIMORFISME (OVERRIDE) ===
    // Melakukan Override pada method utama dari Superclass Character
    public override void Attack()
    {
        if (mana >= 20f)
        {
            mana -= 20f;
            Debug.Log($"{gameObject.name} melakukan 'Palm Strike' (Polimorfisme)! Sisa Mana: {mana}");
            // Logika attack visual Unity kamu bisa ditaruh di sini
        }
        else
        {
            Debug.Log($"{gameObject.name} kehabisan mana untuk menyerang.");
        }
    }

    // === LOGIKA INTERAKSI (UNITY TRIGGER) ===
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = collision.GetComponent<Player>();
            if (playerInRange != null) interactionPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = null;
            interactionPromptUI.SetActive(false);
        }
    }

    private void OnHealButtonClicked()
    {
        if (playerInRange != null && mana >= 30f)
        {
            mana -= 30f;
            playerInRange.RestoreFullHealth(); // Pastikan fungsi ini ada di Player kamu
            interactionPromptUI.SetActive(false); 
        }
    }

    private void OnDestroy()
    {
        if (healButton != null) healButton.onClick.RemoveListener(OnHealButtonClicked);
    }
}