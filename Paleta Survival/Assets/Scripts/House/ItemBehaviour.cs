using UnityEngine;
using TMPro;

public class ItemBehaviour : MonoBehaviour
{
    public string prefabName;

    void Start()
    {
        // gameObject = GetComponent<GameObject>();
        // Get the prefab's name and create a new Item with amount 1
        // string prefabName = gameObject.name.Replace("(Clone)", "").Trim();
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

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        Debug.Log("Collided with: " + collision.gameObject.name);

        PlayerInteraction playerInteraction = collision.gameObject.GetComponent<PlayerInteraction>();
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
