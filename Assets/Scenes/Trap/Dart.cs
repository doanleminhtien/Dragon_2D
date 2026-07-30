using UnityEngine;

public class Dart : MonoBehaviour
{
    public float speed = 15f; // T?c ?? bay c?a phi tiêu

    [Header("--- Âm Thanh Phi Tiêu ---")]
    public AudioClip arrowSFX; // Kéo file 'Arrow' vào ô này

    void Start()
    {
        // ?? PHÁT ÂM THANH KHI PHI TIÊU B?N RA
        if (arrowSFX != null)
        {
            // Phát âm thanh t?i v? trí b?n ra, không lo b? ng?t ti?ng khi phi tiêu b? Destroy
            AudioSource.PlayClipAtPoint(arrowSFX, transform.position);
        }

        // T? ??ng h?y sau 3 giây ?? rác không bay ??y map gây lag game
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // Liên t?c bay th?ng v? phía tr??c (theo tr?c X)
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // N?u ch?m trúng r?ng
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(GetComponent<Collider2D>());
            }
            Destroy(gameObject); // Trúng r?ng thì m?i tên bi?n m?t
        }
        // N?u phi tiêu c?m vào t??ng ho?c ??t
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}