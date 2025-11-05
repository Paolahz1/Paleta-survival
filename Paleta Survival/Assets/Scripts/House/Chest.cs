using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    private Inventario chestInventory;
    public Inventario ChestInventory { get { return chestInventory; } }
    private Animator animator;
    public Transform inventoryContent;
    public GameObject itemIconPrefab;

    public GameObject catnipLeavePrefab;
    public GameObject catnipPlantPrefab;
    public GameObject catnipZiplockPrefab;
    public GameObject cocatPrefab;
    public GameObject metcatPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();

        InitializeChest();
        
        chestInventory.AgregarItem(CrearItem("Catnip Leave", 10), "Chest");
        chestInventory.AgregarItem(CrearItem("Cocat", 2), "Chest");
        chestInventory.AgregarItem(CrearItem("Metcat", 1), "Chest");
        chestInventory.AgregarItem(CrearItem("Catnip Plant", 5), "Chest");
        chestInventory.AgregarItem(CrearItem("Catnip Ziplock", 3), "Chest");
    }

    void Update()
    {
        
    }

    private void InitializeChest()
    {
        if (chestInventory == null)
        {
            chestInventory = new Inventario(12);
        }
    }

    public Item CrearItem(string itemName, int cantidad)
    {
        
        InitializeChest();
        
        ItemPrefabs itemPrefabs = FindFirstObjectByType<ItemPrefabs>();
        GameObject prefab = itemPrefabs.GetPrefabByName(itemName);

        Item item = new Item(0, itemName, cantidad);
        GameObject go = Instantiate(itemIconPrefab, inventoryContent);

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

        return item;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Entered chest trigger: " + other.name);
        if (animator != null)
        {
            animator.SetBool("Opened", true);
            PaletaPrincipal.AsociarCofre(chestInventory);
            // PaletaPrincipal.ImprimirNombresItemsCofre();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Debug.Log("Exited chest trigger: " + other.name);
        if (animator != null)
        {
            animator.SetBool("Opened", false);
            PaletaPrincipal.DesasociarCofre();
        }
    }

}
