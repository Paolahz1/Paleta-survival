using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantener entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }
}
