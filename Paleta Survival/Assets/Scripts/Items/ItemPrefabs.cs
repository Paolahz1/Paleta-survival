using UnityEngine;

public class ItemPrefabs: MonoBehaviour
{
    public GameObject catnipLeavePrefab;
    public GameObject catnipPlantPrefab;
    public GameObject catnipZiplockPrefab;
    public GameObject cocatPrefab;
    public GameObject metcatPrefab;

    public GameObject componenteAPrefab;
    public GameObject componenteBPrefab;
    public GameObject componenteCPrefab;
    public GameObject componenteXPrefab;
    public GameObject componenteYPrefab;
    public GameObject componenteZPrefab;

    public GameObject candyPrefab;

    public GameObject coinPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetPrefabByName(string itemName)
    {
        switch (itemName)
        {
            case "Catnip Leave":
                return catnipLeavePrefab;
            case "Catnip Plant":
                return catnipPlantPrefab;
            case "Catnip Ziplock":
                return catnipZiplockPrefab;
            case "Cocat":
                return cocatPrefab;
            case "Metcat":
                return metcatPrefab;
            case "Componente A":
                return componenteAPrefab;
            case "Componente B":
                return componenteBPrefab;
            case "Componente C":
                return componenteCPrefab;
            case "Componente X":
                return componenteXPrefab;
            case "Componente Y":
                return componenteYPrefab;
            case "Componente Z":
                return componenteZPrefab;
            case "Coin":
                return coinPrefab;
            case "Candy":
                return candyPrefab;
            default:
                Debug.LogWarning("Unknown item name: " + itemName);
                return null;
        }
    }
}
