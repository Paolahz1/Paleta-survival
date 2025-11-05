using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;   // arrastra aquí tu clip
    private AudioSource audioSource;

    void Start()
    {
        // Busca un AudioSource en la escena con tag "UIAudio" o este mismo GameObject
        audioSource = GameObject.FindGameObjectWithTag("UIAudioMenu")?.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("No se encontró un AudioSource con tag 'UIAudioMenu'.");
        }

        // Añade el evento al botón
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
