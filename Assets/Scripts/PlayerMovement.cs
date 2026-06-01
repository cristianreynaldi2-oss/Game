using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2;
    public int facingDirection = 1;
    public Rigidbody2D rb;
    public Animator anim;

    private bool isKnockingback = false;

    // Update is called once per frame
    void Update()
    {
        if (isKnockingback)
        {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (
            horizontal > 0 && transform.localScale.x < 0
            || horizontal < 0 && transform.localScale.x > 0
        )
        {
            Flip();
        }
        ;

        // Debug.Log($"H: {horizontal}, V: {vertical}, Anim: {anim}");

        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));

        rb.velocity = new Vector2(horizontal, vertical) * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    public void Knockback(Transform enemy, float force, float durations)
    {
        if (isKnockingback)
            return;
        isKnockingback = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.velocity = direction * force;

        StartCoroutine(KnockbackDurations(durations));
    }

    IEnumerator KnockbackDurations(float durations)
    {
        yield return new WaitForSeconds(durations);
        rb.velocity = Vector2.zero;
        isKnockingback = false;
    }
}
