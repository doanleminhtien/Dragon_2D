using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("--- UI PANELS ---")]
    public GameObject pausePanel;     // B?ng Pause
    public GameObject settingsPanel;  // B?ng Cài ??t Volume/Music
    public GameObject gameOverPanel;  // B?ng thua (Game Over)
    public GameObject winPanel;       // B?ng chúc m?ng th?ng (Win)

    [Header("--- AUDIO SOURCE ---")]
    public AudioSource bgmSource;     // Object phát nh?c n?n

    public static bool isPaused = false;
    private GameObject previousPanel; // Ghi nh? b?ng tr??c ?ó ?? khi Back t? Settings s? quay v? ?úng ch?

    void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
        HideAllPanels(); // T?t h?t t?t c? b?ng khi v?a vào game
    }

    void Update()
    {
        bool isGameOver = gameOverPanel != null && gameOverPanel.activeSelf;
        bool isWin = winPanel != null && winPanel.activeSelf;

        // B?m ESC ?? B?t/T?t Pause ho?c ?óng Settings
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && !isWin)
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseSettingsMenu(); // ?ang ? Settings b?m ESC s? Back ra b?ng c?
            }
            else if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // ================= HÀM ??C TR? L?I ?È UI: CH? B?T DUY NH?T 1 B?NG =================
    public void ShowOnly(GameObject panelToShow)
    {
        // Step 1: Ép T?T H?T t?t c? các b?ng UI tr??c
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        // Step 2: Ch? B?T DUY NH?T b?ng ???c ch? ??nh (n?u có)
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
    }

    public void HideAllPanels()
    {
        ShowOnly(null); // T?t toàn b? b?ng UI
    }

    // ================= PAUSE & RESUME =================
    public void Pause()
    {
        ShowOnly(pausePanel); // Ch? b?t duy nh?t PausePanel
        Time.timeScale = 0f;  // ?óng b?ng game
        isPaused = true;
    }

    public void Resume()
    {
        ShowOnly(null);      // T?t s?ch b?ng UI ?? ch?i ti?p
        Time.timeScale = 1f;
        isPaused = false;
    }

    // ================= WIN & GAME OVER =================
    public void TriggerWin()
    {
        ShowOnly(winPanel);  // Ch? b?t duy nh?t WinPanel
        Time.timeScale = 0f;
    }

    public void TriggerGameOver()
    {
        ShowOnly(gameOverPanel); // Ch? b?t duy nh?t GameOverPanel
        Time.timeScale = 0f;
    }

    // ================= SETTINGS & VOLUME =================
    public void OpenSettingsMenu()
    {
        // 1. Ghi nh? xem ?ang m? Settings t? ?âu (Pause, GameOver hay Win)
        if (pausePanel != null && pausePanel.activeSelf) previousPanel = pausePanel;
        else if (gameOverPanel != null && gameOverPanel.activeSelf) previousPanel = gameOverPanel;
        else if (winPanel != null && winPanel.activeSelf) previousPanel = winPanel;
        else previousPanel = null;

        // 2. Ch? b?t duy nh?t SettingsPanel
        ShowOnly(settingsPanel);
    }

    public void CloseSettingsMenu()
    {
        // N?u tr??c ?ó ?ang ? b?ng nào thì quay v? ?ÚNG DUY NH?T b?ng ?ó
        if (previousPanel != null)
        {
            ShowOnly(previousPanel);
            previousPanel = null;
        }
        else
        {
            Resume(); // N?u không có b?ng tr??c thì quay l?i game ch?i bình th??ng
        }
    }

    // Ch?nh Âm l??ng T?NG (Dành cho thanh VOLUME)
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    // Ch?nh Âm l??ng NH?C N?N (Dành cho thanh MUSIC)
    public void SetMusicVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    // ================= SCENE & QUIT =================
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}