using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [Header("UI")]
    public GameObject pausePanel;

    private bool isPaused = false;
    private float savedVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ToggleVolume()
    {
        if (AudioListener.volume > 0f)
        {
            savedVolume = AudioListener.volume;
            AudioListener.volume = 0f; // mute
        }
        else
        {
            AudioListener.volume = savedVolume; // restaurar
        }
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 👉 Método global para preguntar si está en pausa
    public bool IsPaused()
    {
        return isPaused;
    }
}
