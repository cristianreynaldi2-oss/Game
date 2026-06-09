using UnityEngine;

public class SpearProjectile : MonoBehaviour
{
    private float speed = 12f; // Kecepatan terbang tombak
    private float damage;
    private Vector3 targetDirection;

    public void Setup(Vector3 direction, float damageValue)
    {
        this.targetDirection = direction.normalized;
        this.damage = damageValue;

        // Atur rotasi gambar tombak agar menghadap ke arah terbangnya
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Hancurkan otomatis dalam 4 detik jika meleset (tidak kena player)
        Destroy(gameObject, 4f);
    }

    private void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IDamageable>()?.TakeDamage(damage);
            Destroy(gameObject); // Hancurkan tombak setelah kena player
        }
    }
}