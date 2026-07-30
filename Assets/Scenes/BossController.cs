using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("--- Di Chuyen ---")]
    public float speed = 2f;
    public float patrolDistance = 3f;
    private Vector2 startPos;
    private bool movingRight = true;

    [Header("--- Tan Cong ---")]
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public float attackDelay = 0.35f;
    private float nextAttackTime;

    [Header("--- Mau & Bi Danh ---")]
    public int health = 2;
    public float knockbackForce = 5f;

    // === 🔊 THÊM CÁC BIẾN ÂM THANH CHO BOSS ===
    [Header("--- Âm Thanh Boss ---")]
    public AudioClip attackSFX; // Kéo file 'SwordHit'
    public AudioClip hitSFX;    // Kéo file 'Impact'
    public AudioClip dieSFX;    // Kéo file 'Death'
    private AudioSource audioSource;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform player;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Lấy loa phát âm thanh
        startPos = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        anim.SetBool("isWalking", true);
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            transform.localScale = new Vector3(1, 1, 1);
            if (transform.position.x >= startPos.x + patrolDistance) movingRight = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
            if (transform.position.x <= startPos.x - patrolDistance) movingRight = true;
        }
    }

    void AttackPlayer()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isWalking", false);

        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("attack");

            // 🔊 Phát tiếng vung kiếm
            if (attackSFX && audioSource) audioSource.PlayOneShot(attackSFX);

            Invoke("DealDamageToPlayer", attackDelay);

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void DealDamageToPlayer()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange + 0.3f)
        {
            PlayerController playerScript = player.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(GetComponent<Collider2D>());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Fireball"))
        {
            float knockbackDir = (transform.position.x - collision.transform.position.x > 0) ? 1f : -1f;

            TakeBossDamage(1, knockbackDir);

            Destroy(collision.gameObject);
        }
    }

    public void TakeBossDamage(int damageAmount, float knockbackDir)
    {
        if (isDead) return;

        CancelInvoke("DealDamageToPlayer");

        health -= damageAmount;

        // 🔊 Phát tiếng trúng đạn lửa giật lùi
        if (hitSFX && audioSource) audioSource.PlayOneShot(hitSFX);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(knockbackDir * knockbackForce, rb.linearVelocity.y), ForceMode2D.Impulse);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        CancelInvoke("DealDamageToPlayer");
        anim.SetTrigger("die");

        // 🔊 Phát tiếng Boss ngã xuống
        if (dieSFX && audioSource) audioSource.PlayOneShot(dieSFX);

        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}