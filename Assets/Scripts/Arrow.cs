using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed = 10f;
    private float damage;
    private Vector3 targetDirection;

    // Fungsi untuk inisialisasi arah dan damage panah saat dtembakkan
    public void Setup(Vector3 direction, float damageValue)
    {
        this.targetDirection = direction.normalized;
        this.damage = damageValue;

        // Mengatur rotasi panah agar menghadap ke arah terbangnya (2D)
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Hancurkan panah otomatis setelah 3 detik jika tidak mengenai apapun (agar tidak menumpuk di memori)
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        // Gerakkan panah maju setiap frame
        transform.position += targetDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah yang ditabrak adalah Player
        if (collision.CompareTag("Player"))
        {
            // Berikan damage ke Player
            collision.GetComponent<IDamageable>()?.TakeDamage(damage);
            
            // Hancurkan panah setelah mengenai Player
            Destroy(gameObject);
        }
    }
}