using UnityEngine;
using UnityEngine.SceneManagement; // B?t bu?c ph?i có dòng này ?? load map

public class NextLevel : MonoBehaviour
{
    [Header("?i?n tên Map ti?p theo vào ?ây")]
    public string nextMapName = "Map3"; // Nh? gõ chính xác tên file c?a Map 3

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ki?m tra xem ng??i ch?m vào c?ng có ph?i là con r?ng (Player) không
        if (collision.CompareTag("Player"))
        {
            // Bê nguyên con r?ng qu?ng sang map m?i!
            SceneManager.LoadScene(nextMapName);
        }
    }
}