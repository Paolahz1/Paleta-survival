using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trade : MonoBehaviour
{
    public Inventario traderInventory;
    // public Inventario ChestInventory { get { return traderInventory; } }
    public Transform inventoryContent;
    public GameObject clickableItemIconPrefab;
    public GameObject itemIconPrefab;

    public GameObject catnipLeavePrefab;
    public GameObject catnipPlantPrefab;
    public GameObject catnipZiplockPrefab;
    public GameObject cocatPrefab;
    public GameObject metcatPrefab;

    public GameObject coinPrefab;

    void Start()
    {
        InitializeChest();

        traderInventory.AgregarItem(CrearItem("Catnip Plant", 50), "Trade");
        // traderInventory.AgregarItem(CrearItem("Catnip Leave", 10), "Trade");
        traderInventory.AgregarItem(CrearItem("Componente A", 100), "Trade");
        traderInventory.AgregarItem(CrearItem("Componente B", 200), "Trade");
        traderInventory.AgregarItem(CrearItem("Componente C", 400), "Trade");
        traderInventory.AgregarItem(CrearItem("Componente X", 500), "Trade");
        traderInventory.AgregarItem(CrearItem("Componente Y", 600), "Trade");
        traderInventory.AgregarItem(CrearItem("Componente Z", 1000), "Trade");
        // traderInventory.AgregarItem(CrearItem("Catnip Ziplock", 3), "Trade");
        
        // traderInventory.AgregarItem(CrearItem("Coin", 0), "Coin");
    }

    void Update()
    {
        
    }

    private void InitializeChest()
    {
        if (traderInventory == null)
        {
            traderInventory = new Inventario(12);
        }
    }

    public Item CrearItem(string itemName, int cantidad)
    {

        InitializeChest();

        ItemPrefabs itemPrefabs = FindFirstObjectByType<ItemPrefabs>();
        GameObject prefab = itemPrefabs.GetPrefabByName(itemName);

        Item item = new Item(0, itemName, cantidad);

        GameObject prefabToUse = itemName == "Coin" ? itemIconPrefab : clickableItemIconPrefab;
        GameObject go = Instantiate(prefabToUse, inventoryContent);

        Image img = go.GetComponent<Image>();
        img.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        item.GameObject = go;
        item.Imagen = img.sprite;

        item.GameObject.SetActive(false);

        var textMesh = prefab.GetComponent<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMesh.text = cantidad.ToString();
            // Debug.Log($"Set text mesh to: {cantidad}");
        }

        var uiItem = go.GetComponent<UITradeItem>();
        if (uiItem != null)
        {
            uiItem.itemName = itemName;
            uiItem.number = cantidad;
        }

        return item;
    }
    
}
