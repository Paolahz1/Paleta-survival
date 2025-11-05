using UnityEngine;
using TMPro;

public class ItemBehaviour : MonoBehaviour
{
    public string prefabName;

    [Header("Audio")]
    public AudioClip sonidoRecoger; // Clip de sonido al recoger
    private AudioSource audioSource; // Fuente de audio del objeto

    void Start()
    {
        // Crear o recuperar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configuración recomendada del AudioSource
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;        // Volumen máximo
        audioSource.spatialBlend = 0f;  // 2D
        audioSource.loop = false;

        // Configurar el sprite según el prefab
        ItemPrefabs itemPrefabs = FindFirstObjectByType<ItemPrefabs>();
        GameObject prefab = itemPrefabs.GetPrefabByName(prefabName);

        if (prefab != null)
        {
            SpriteRenderer prefabSpriteRenderer = prefab.GetComponent<SpriteRenderer>();
            SpriteRenderer thisSpriteRenderer = GetComponent<SpriteRenderer>();
            if (prefabSpriteRenderer != null && thisSpriteRenderer != null)
            {
                thisSpriteRenderer.sprite = prefabSpriteRenderer.sprite;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 🔊 Reproducir sonido
            if (sonidoRecoger != null)
            {
                audioSource.clip = sonidoRecoger;
                audioSource.Play();
            }

            // Desactivar visualmente el objeto para que desaparezca
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Destruir el objeto después de que termine el sonido
            if (sonidoRecoger != null)
                Destroy(gameObject, sonidoRecoger.length);
            else
                Destroy(gameObject);

            // Lógica de vida
            PlayerInteraction playerInteraction = collision.gameObject.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.gatoPrincipal.GanarVida();

                GameObject vidaObj = GameObject.Find("Vida");
                if (vidaObj != null)
                {
                    TextMeshProUGUI vidaText = vidaObj.GetComponent<TextMeshProUGUI>();
                    if (vidaText != null)
                    {
                        vidaText.text = playerInteraction.gatoPrincipal.vida.ToString();
                    }
                }
            }
        }

        Debug.Log("Collided with: " + collision.gameObject.name);
    }
}
