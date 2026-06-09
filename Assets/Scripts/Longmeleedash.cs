using UnityEngine;

public class Longmeleedash : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private float damage = 15f; // Dipisah jadi variabel agar mudah di-tweak di Inspector

    // Menambahkan variabel ini karena di script LongMelee.cs kamu sempat memanggil 'BefoDash.dashpoint'
    // Jika posisinya diatur dari firePoint milik Enemy, variabel ini opsional, tetapi aman untuk tetap ada.
    public Vector3 dashpoint => transform.position; 

    public void Setup(Vector3 direction)
    {
        // Atur rotasi gambar dash agar menghadap ke arah terbangnya
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        // Bergerak maju searah dengan sumbu X kanan objek yang sudah diputar
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Memberikan damage ke Player jika memiliki interface IDamageable
            collision.GetComponent<IDamageable>()?.TakeDamage(damage); 
            Destroy(gameObject); // Hancurkan dash setelah kena player
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject); // Hancurkan dash jika menabrak dinding/rintangan
        }
    }

    // Menghemat memori: Jika meleset dan keluar dari layar camera, otomatis hancur
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}