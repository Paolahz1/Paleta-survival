using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TableBehaviour : MonoBehaviour
{

    public Mesa mesa;

    public string tableName;

    public Transform tableCanvasContent;

    public GameObject clickableItemIconPrefab;

    public GameObject itemIconPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeTable();

        // Debug.Log($"Table '{tableName}' initialized with {mesa.recetas.Length} recipes.");
        int recetaCount = 0;
        foreach (Receta receta in mesa.recetas)
        {
            // Debug.Log($"Adding recipe {recetaCount + 1}: {receta.Resultado.Nombre}");
            foreach (var ingrediente in receta.Ingredientes)
            {
                // Debug.Log($"Adding ingredient: {ingrediente.Nombre}, Quantity: {ingrediente.Cantidad}");
                Item item = CrearItem(ingrediente.Nombre, ingrediente.Cantidad, false);
                if (recetaCount == 0)
                {
                    // Debug.Log($"Adding to Crafteo1: {ingrediente.Nombre}");
                    mesa.AgregarItem(item, "Crafteo1");
                }
                else if (recetaCount == 1)
                {
                    mesa.AgregarItem(item, "Crafteo2");
                }
            }
            Item resultadoItem = CrearItem(receta.Resultado.Nombre, receta.Resultado.Cantidad, true);
            mesa.AgregarItem(resultadoItem, "Resultado");
            recetaCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PaletaPrincipal.AsociarMesa(mesa);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PaletaPrincipal.DesasociarMesa();
        }
    }

    private void InitializeTable()
    {
        if (mesa == null)
        {
            mesa = new Mesa(unlockXp: 0, nombre: tableName, cantidad: 1, precioCompra: 0, recetas: new Receta[] { });
            mesa.mesaCanvasContent = tableCanvasContent;
        }
    }

    public Item CrearItem(string itemName, int cantidad, bool isResult)
    {
        InitializeTable();

        ItemPrefabs itemPrefabs = FindFirstObjectByType<ItemPrefabs>();
        GameObject prefab = itemPrefabs.GetPrefabByName(itemName);

        Item item = new Item(0, itemName, cantidad);

        GameObject prefabToUse = !isResult ? itemIconPrefab : clickableItemIconPrefab;
        GameObject go = Instantiate(prefabToUse, tableCanvasContent);

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

        var uiItem = go.GetComponent<UICraftItem>();
        if (uiItem != null)
        {
            uiItem.itemName = itemName;
            uiItem.cantidad = cantidad;
        }

        return item;
    }
    
    public void disableMesaCanvas()
    {
        string[] tableScreenNames = { "TableScreen", "TableScreen1", "TableScreen2" };
        
        foreach (string screenName in tableScreenNames)
        {
            GameObject tableScreen = GameObject.Find(screenName);
            if (tableScreen != null)
            {
                Canvas tableCanvas = tableScreen.GetComponent<Canvas>();
                if (tableCanvas != null)
                {
                    tableCanvas.enabled = false;
                }
            }
        }
    }
}
