using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; }
    public static PauseController Instance { get; private set; }

    [SerializeField] private GameObject pauseMenu; // opcional, asigna en inspector

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Método público estático que llamas desde NPC: PauseController.setPause(true)
    public static void setPause(bool pause)
    {
        if (Instance == null)
        {
            // Si no hay instancia en escena, aplicamos lo básico para que no rompa las llamadas
            IsGamePaused = pause;
            Time.timeScale = pause ? 0f : 1f;
            return;
        }

        Instance.InternalSetPause(pause);
    }

    // Nombre con PascalCase por buena práctica (opcional)
    public static void SetPause(bool pause) => setPause(pause);

    private void InternalSetPause(bool pause)
    {
        IsGamePaused = pause;
        Time.timeScale = pause ? 0f : 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(pause);
    }

    // Método para usar con UI (botón continuar)
    public void TogglePause()
    {
        setPause(!IsGamePaused);
    }
}
