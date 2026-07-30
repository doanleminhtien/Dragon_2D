using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Ki?m tra xem có ?úng con r?ng ??ng ph?i không
        if (collision.CompareTag("Player"))
        {
            // 2. Tìm script PlayerController trên ng??i con r?ng
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                // 3. L?y chính cái Collider2D c?a c?c gai này ?? truy?n vào hàm TakeDamage
                Collider2D myCollider = GetComponent<Collider2D>();

                // 4. G?i hàm và truy?n collider sang ?? r?ng bi?t h??ng mà v?ng lùi l?i (Knockback)
                player.TakeDamage(myCollider);
            }
        }
    }
}