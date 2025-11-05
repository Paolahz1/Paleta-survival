using UnityEngine;

public class UITradeItem : MonoBehaviour
{

    public PlayerInteraction playerInteraction;

    public string itemName;

    public int number;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnAddItemButtonClicked()
    {
        print("Add Item Button Clicked");
        playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        if (playerInteraction != null && !string.IsNullOrEmpty(itemName))
        {
            if (number <= playerInteraction.gatoPrincipal.Coins)
            {

                playerInteraction.gatoPrincipal.TakeCoins(number);

                Trade trade = FindFirstObjectByType<Trade>();
                trade.traderInventory.RemoverTodosLosItem(new Item("Coin"));
                trade.traderInventory.AgregarItem(trade.CrearItem("Coin", playerInteraction.gatoPrincipal.Coins), "Coin");

                playerInteraction.CrearYAgregarItem(itemName, 1);
            }
        }
        else
        {
            Debug.LogWarning("PlayerInteraction reference is missing or itemName is empty.");
        }
    }
}
