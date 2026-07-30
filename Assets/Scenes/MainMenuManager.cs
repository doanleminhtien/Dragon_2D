using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Map1"); // Thay "Map1" b?ng ?úng tên Scene ch?i game c?a m
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("?ã b?m thoát Game!");
        UnityEngine.Application.Quit();
    }
}