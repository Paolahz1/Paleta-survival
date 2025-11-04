using UnityEngine;

public class Buyer : MonoBehaviour, IInteractable
{
    public static bool InteractingWithBuyer = false;
    public bool OrderCompleted = false;
    private string orderItemName;

    public
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] orderOptions = { "Catnip Ziplock", "Cocat", "Metcat" };
        orderItemName = orderOptions[Random.Range(0, orderOptions.Length)];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CanInteract()
    {
        return !OrderCompleted;
    }

    public void Interact()
    {
        if (CanInteract())
        {
            InteractingWithBuyer = true;
            PlayerInteraction player = FindFirstObjectByType<PlayerInteraction>();
            Item orderItem = player.CrearItem(orderItemName, 1);
            player.playerInventory.AgregarItem(orderItem, "Asked");
            player.ShowInventory();
        }
    }
}
