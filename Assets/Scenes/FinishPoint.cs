using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public GameUIManager gameUIManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi nhân v?t ch?m vào Cúp End
        if (collision.CompareTag("Player"))
        {
            if (gameUIManager != null)
            {
                gameUIManager.TriggerWin();
            }
        }
    }
}