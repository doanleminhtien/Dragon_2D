using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    [Header("Cài ??t Th?i Gian")]
    public float offDuration = 3f;
    public float hitDuration = 2f;
    public float onDuration = 3f;

    // === ?? THÊM CÁC BI?N ÂM THANH CHO B?Y L?A ===
    [Header("Cài ??t Âm Thanh")]
    public AudioClip fireSFX; // Kéo file 'Firetrap' vào ?ây
    private AudioSource audioSource;

    private Animator anim;
    private BoxCollider2D fireCollider;

    private bool isFireOn = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        fireCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>(); // L?y loa AudioSource trên b?y

        StartCoroutine(FireCycle());
    }

    IEnumerator FireCycle()
    {
        while (true)
        {
            // 1. TR?NG THÁI OFF: T?t l?a
            anim.Play("Fire_Off");
            isFireOn = false;
            yield return new WaitForSeconds(offDuration);

            // 2. TR?NG THÁI HIT: Ch?p c?nh báo
            anim.Play("Fire_Hit");
            yield return new WaitForSeconds(hitDuration);

            // 3. TR?NG THÁI ON: L?a ph?t lên!
            anim.Play("Fire_On");
            isFireOn = true;

            // ?? PHÁT ÂM THANH B?Y L?A PH?T BÙM LÊN!
            if (fireSFX && audioSource)
            {
                audioSource.PlayOneShot(fireSFX);
            }

            yield return new WaitForSeconds(onDuration);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isFireOn)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(fireCollider);
                }
            }
        }
    }
}