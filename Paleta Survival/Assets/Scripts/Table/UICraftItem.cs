using UnityEngine;

public class UICraftItem : MonoBehaviour
{
    public AudioClip clickSound;

    public string itemName;

    public int cantidad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        // Debug.Log("Clicked on TableItem");
        PlayerInteraction playerInteraction = FindFirstObjectByType<PlayerInteraction>();

        if (playerInteraction != null)
        {
            Item[] ingredientes = PaletaPrincipal.mesaUsada.GetIngredientesByResultadoName(itemName);

            bool hasAllIngredients = true;
            foreach (var ingredient in ingredientes)
            {
                var item = playerInteraction.playerInventory.Items.Find(x => x.Nombre == ingredient.Nombre);
                if (item == null || item.Cantidad < ingredient.Cantidad)
                {
                    hasAllIngredients = false;
                    break;
                }
            }

            // Debug.Log($"Has all ingredients: {hasAllIngredients}");

            if (hasAllIngredients)
            {
                // Remove ingredients from player inventory
                foreach (var ingredient in ingredientes)
                {
                    var item = playerInteraction.playerInventory.Items.Find(x => x.Nombre == ingredient.Nombre);
                    if (item != null)
                    {
                        playerInteraction.playerInventory.RemoverUnItem(item);
                    }
                }
                // Debug.Log($"Crafted item: {itemName}");
                // Add result item to player inventory
                playerInteraction.playerInventory.AgregarItem(playerInteraction.CrearItem(itemName, cantidad), "Player");

                // Play click sound
                AudioSource audioSource = GameObject.FindGameObjectWithTag("UIAudioMenu")?.GetComponent<AudioSource>();
                if (audioSource != null && clickSound != null)
                {
                    audioSource.PlayOneShot(clickSound);
                }
            }
        }
    }
}
