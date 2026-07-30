using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Cài ??t M?t ??t")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Cài ??t Phun L?a")]
    public GameObject fireballPrefab;
    public Transform firePoint;

    [Header("Cài ??t Máu & ??y Lùi (Knockback)")]
    public int maxHealth = 3;
    public float knockbackForceX = 8f;
    public float knockbackForceY = 6f;
    public float knockbackDuration = 0.6f;

    [Header("Giao di?n Máu & Game Over")]
    public GameObject[] heartUI;
    public GameObject gameOverPanel; // ?? M?I: Ô kéo th? GameOverPanel

    // === ?? THÊM CÁC BI?N ÂM THANH CHO R?NG ===
    [Header("--- Âm Thanh R?ng ---")]
    public AudioClip jumpSFX;     // Kéo file 'Jump'
    public AudioClip shootSFX;    // Kéo file 'Fireball'
    public AudioClip hurtSFX;     // Kéo file 'Hurt'
    public AudioClip healSFX;     // Kéo file 'Life'
    public AudioClip dieSFX;      // Kéo file 'Death'
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float moveInput;

    private int currentHealth;
    private bool isDead = false;
    private bool isKnockedBack = false;

    private Vector2 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;

        UpdateHealthUI();
        startPosition = transform.position;
    }

    void Update()
    {
        if (isDead) return;

        if (!isKnockedBack)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            animator.SetBool("isGrounded", isGrounded);

            // ?? Phát ti?ng Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                if (jumpSFX && audioSource) audioSource.PlayOneShot(jumpSFX);
            }

            // ?? Phát ti?ng Shoot
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Shoot();
            }

            animator.SetBool("isWalking", moveInput != 0);

            if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
            else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        if (isDead || isKnockedBack) return;
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Shoot()
    {
        animator.SetTrigger("attack");
        if (shootSFX && audioSource) audioSource.PlayOneShot(shootSFX);

        GameObject fb = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        fb.GetComponent<Fireball>().Launch(transform.localScale.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap") && !isDead && !isKnockedBack)
        {
            TakeDamage(collision);
        }

        if (collision.CompareTag("HeartItem") && !isDead)
        {
            Heal();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Finish") && !isDead)
        {
            SceneManager.LoadScene("Map2");
        }

        if (collision.CompareTag("Pit") && !isDead)
        {
            FallIntoPit();
        }
    }

    void FallIntoPit()
    {
        currentHealth--;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            PlayerRealDie();
        }
        else
        {
            transform.position = startPosition;
            if (rb != null) rb.linearVelocity = Vector2.zero;
            isKnockedBack = false;
            animator.Play("Idle");
        }
    }

    void Heal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            UpdateHealthUI();
            if (healSFX && audioSource) audioSource.PlayOneShot(healSFX);
        }
    }

    public void TakeDamage(Collider2D trap)
    {
        currentHealth--;
        UpdateHealthUI();

        if (hurtSFX && audioSource) audioSource.PlayOneShot(hurtSFX);

        isKnockedBack = true;
        float knockbackDirection = (transform.position.x > trap.transform.position.x) ? 1f : -1f;
        rb.linearVelocity = new Vector2(knockbackDirection * knockbackForceX, knockbackForceY);

        animator.Play("Die", 0, 0f);

        if (currentHealth <= 0)
        {
            PlayerRealDie();
        }
        else
        {
            Invoke("GetUpAndContinue", knockbackDuration);
        }
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < heartUI.Length; i++)
        {
            if (i < currentHealth) heartUI[i].SetActive(true);
            else heartUI[i].SetActive(false);
        }
    }

    void GetUpAndContinue()
    {
        if (!isDead)
        {
            isKnockedBack = false;
            animator.Play("Idle");
        }
    }

    // === ??? S?A L?I HÀM CH?T BÊN D??I ===
    void PlayerRealDie()
    {
        isDead = true;
        if (dieSFX && audioSource) audioSource.PlayOneShot(dieSFX);

        Invoke("MakeStaticOnDeath", 0.4f);

        // Hi?n b?ng Game Over sau 1.5 giây ?? ch?i xong animation ch?t
        Invoke("ShowGameOverPanel", 1.5f);
    }

    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // B?t b?ng Game Over
        }
    }

    void MakeStaticOnDeath()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }
}