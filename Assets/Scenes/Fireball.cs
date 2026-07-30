using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    private float direction = 1f;

    [Header("Cài ??t hi?u ?ng n?")]
    public GameObject impactEffectPrefab; // Kéo file FireballImpact vào ?ây nha

    public void Launch(float dir)
    {
        direction = dir;
        // L?t hình c?c l?a theo h??ng bay c?a R?ng
        transform.localScale = new Vector3(dir, transform.localScale.y, transform.localScale.z);
    }

    void Update()
    {
        // ÉP C?C L?A CH? BAY THEO TR?C X (PH??NG NGANG)
        Vector3 moveDirection = new Vector3(direction, 0, 0);
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    // HÀM KI?M TRA VA CH?M: N? KHI TRÚNG ??CH HO?C V?T C?N
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ki?m tra xem có trúng Layer "Ground" (??t/Thùng) HO?C có Tag "Enemy" (K? ??ch) không
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.CompareTag("Enemy"))
        {
            // Sinh ra hi?u ?ng n? bùm tr?ng l?p lánh
            if (impactEffectPrefab != null)
            {
                GameObject effect = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 0.4f); // D?n d?p hi?u ?ng sau 0.4 giây cho nh? máy
            }

            // Xóa c?c l?a ?i
            Destroy(gameObject);
        }
    }

    // T? ??ng xóa ??n n?u nó bay ra kh?i góc nhìn c?a Camera (ch?ng lag)
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}