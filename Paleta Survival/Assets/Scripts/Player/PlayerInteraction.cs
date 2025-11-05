using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable interactable;

    public Inventario playerInventory;
    // public Inventario ChestInventory { get { return playerInventory; } }
    public Transform playerInventoryContent;

    public GameObject clickableItemIconPrefab;
    public GameObject itemIconPrefab;

    public Canvas playerInventoryCanvas;

    public GameObject catnipLeavePrefab;
    public GameObject catnipPlantPrefab;
    public GameObject catnipZiplockPrefab;
    public GameObject cocatPrefab;
    public GameObject metcatPrefab;
    public GameObject coinPrefab;

    public PaletaPrincipal gatoPrincipal;

    public Buyer buyer;

    void Awake()
    {
        InitializeInventory();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // print("Entraaaa");
        if (other.TryGetComponent<IInteractable>(out var npc))
        {
            interactable = npc;
        }

        if (other.TryGetComponent<Buyer>(out var buyer))
        {
            this.buyer = buyer;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IInteractable>() == interactable)
        {
            interactable = null;
        }

        if (other.GetComponent<Buyer>() == buyer)
        {
            buyer = null;
        }
    }

    void Update()
    {
        if (interactable != null && Input.GetKeyDown(KeyCode.E))
        {
            interactable.Interact();
        }
        else if (PaletaPrincipal.mesaUsada != null && Input.GetKeyDown(KeyCode.E))
        {
            Transform tableScreen = PaletaPrincipal.mesaUsada.mesaCanvasContent;
            if (tableScreen != null)
            {
                Canvas tableCanvas = tableScreen.GetComponent<Canvas>();
                if (tableCanvas != null)
                {
                    tableCanvas.enabled = true;
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el GameObject 'TableScreen' en la escena.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // ShowInventory();
        }
    }

    public Item CrearItem(string itemName, int cantidad)
    {

        InitializeInventory();

        ItemPrefabs itemPrefabs = FindFirstObjectByType<ItemPrefabs>();
        GameObject prefab = itemPrefabs.GetPrefabByName(itemName);

        Item item = new Item(0, itemName, cantidad);

        playerInventoryContent = GameObject.Find("PlayerInventory").transform;
        
        GameObject prefabToUse = itemName == "Coin" ? itemIconPrefab : clickableItemIconPrefab;
        GameObject go = Instantiate(prefabToUse, playerInventoryContent);

        Image img = go.GetComponent<Image>();
        img.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        item.GameObject = go;
        item.Imagen = img.sprite;

        item.GameObject.SetActive(false);

        var textMesh = prefab.GetComponent<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMesh.text = cantidad.ToString();
            Debug.Log($"Set text mesh to: {cantidad}");
        }

        var uiItem = go.GetComponent<UISellItem>();
        if (uiItem != null)
        {
            uiItem.itemName = itemName;
        }

        return item;
    }

    public Item CrearYAgregarItem(string itemName, int cantidad)
    {
        Item item = CrearItem(itemName, cantidad);
        if (item != null)
        {
            print("Adding item to player inventory: " + itemName);
            playerInventory.AgregarItem(item, "Player");
        }
        return item;
    }

    private void InitializeInventory()
    {
        if (playerInventory == null)
        {
            gatoPrincipal = new PaletaPrincipal("Gato Principal", 12);
            gatoPrincipal.Coins = 1500;
            playerInventory = gatoPrincipal.Inventario;
        }
    }

    public void ShowInventory()
    {
        Time.timeScale = 0;
        if (playerInventoryCanvas != null)
        {
            playerInventoryCanvas.enabled = true;
            playerInventory.RemoverTodosLosItem(new Item("Coin"));
            playerInventory.AgregarItem(CrearItem("Coin", gatoPrincipal.Coins), "Coin");
        }
    }

    public void HideInventory()
    {
        if (Buyer.InteractingWithBuyer)
        {
            playerInventory.RemoveAskedItem();
            Buyer.InteractingWithBuyer = false;
        }
        if (playerInventoryCanvas != null)
        {
            playerInventoryCanvas.enabled = false;
        }
        Time.timeScale = 1;
    }
    
    public void completeBuyerOrder()
    {
        if (buyer != null)
        {
            buyer.OrderCompleted = true;
            // coins TODO bonus
        }else{
            Debug.LogWarning("No buyer to complete order for.");
        }
    }
}
