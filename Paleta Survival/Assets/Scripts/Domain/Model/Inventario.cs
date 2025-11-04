using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Inventario
{
    public int Espacio { get; set; }
    public List<Item> Items { get; set; }
    public Item coinItem;
    public Item askedItem;

    public Inventario(int espacio)
    {
        this.Espacio = espacio;
        this.Items = new List<Item>(espacio);
    }

    public bool AgregarItem(Item item, string nombreInventario)
    {
        // Check if an equal item is already in inventory
        foreach (var existingItem in Items)
        {
            if (existingItem.Nombre == item.Nombre && nombreInventario != "Asked")
            {
                existingItem.Cantidad += item.Cantidad; // Add amounts
                var textComponent = existingItem.GameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = existingItem.Cantidad.ToString();
                }
                return true;
            }
        }

        if (nombreInventario == "Coin")
        {
            coinItem = item;

            item.GameObject.SetActive(true);
            DrawItemForCoin(item);

            var textComponent = item.GameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = item.Cantidad.ToString();
            }

            return true;
        }
        else if (nombreInventario == "Asked")
        {
            askedItem = item;

            item.GameObject.SetActive(true);
            DrawAskedItem(item);

            var textComponent = item.GameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = item.Cantidad.ToString();
            }
            
            return true;
        }
        else
        {
            if (Items.Count < Espacio)
            {
                item.GameObject.SetActive(true);
                item.SlotIndex = FillAndGetNextAvailableSlotIndex();

                if (nombreInventario == "Player")
                {
                    DrawItemForPlayer(item);
                }
                else if (nombreInventario == "Chest")
                {
                    DrawItemForChest(item);
                }
                else if (nombreInventario == "Trade")
                {
                    DrawItemForTrade(item);
                }
                else
                {
                    Debug.LogWarning($"Unknown inventory name: {nombreInventario}. Item will not be positioned.");
                }

                var textComponent = item.GameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                // Debug.Log($"Found text component: {textComponent}");
                if (textComponent != null)
                {
                    // Debug.Log($"Setting text for item: {item.Nombre} to {item.Cantidad}");
                    textComponent.text = item.Cantidad.ToString();
                    // Debug.Log($"Updated text position for item: {item.Nombre}");
                }

                Items.Add(item);
                return true;
            }
        }
        return false; // Inventario lleno
    }

    public Item RemoverTodosLosItem(Item item)
    {
        var existingItem = Items.Find(i => i.Nombre == item.Nombre);
        if (existingItem != null)
        {
            ClearSlotIndex(existingItem.SlotIndex);
            Items.Remove(existingItem);
            existingItem.GameObject.SetActive(false);
            return existingItem;
        }
        else
        {
            if (item.Nombre == "Coin" && coinItem != null)
            {
                coinItem.GameObject.SetActive(false);
                return coinItem;
            }
        }
        return null;
    }

    public Item RemoverUnItem(Item item)
    {
        foreach (var existingItem in Items)
        {
            if (existingItem.Nombre == item.Nombre)
            {
                if (existingItem.Cantidad > 0)
                {
                    existingItem.Cantidad--;
                    var textComponent = existingItem.GameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        textComponent.text = existingItem.Cantidad.ToString();
                    }
                    if (existingItem.Cantidad == 0)
                    {
                        ClearSlotIndex(existingItem.SlotIndex);
                        Items.Remove(existingItem);
                        existingItem.GameObject.SetActive(false);
                    }
                    return existingItem;
                }
                else if (item.Nombre == "Coin")
                {
                    Items.Remove(existingItem);
                    existingItem.GameObject.SetActive(false);
                    return existingItem;
                }
            }
        }
        return null;
    }

    public int CantidadItems()
    {
        return Items.Count;
    }

    int[] slotIndices = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

    private int FillAndGetNextAvailableSlotIndex()
    {
        for (int i = 0; i < slotIndices.Length; i++)
        {
            if (slotIndices[i] == 0)
            {
                slotIndices[i] = 1; // Mark slot as used
                return i;
            }
        }
        return -1; // No available slots
    }

    private void ClearSlotIndex(int index)
    {
        if (index >= 0 && index < slotIndices.Length)
        {
            slotIndices[index] = 0; // Mark slot as free
        }
    }

    private void DrawItemForPlayer(Item item)
    {
        if (item.SlotIndex < 4)
        {
            item.GameObject.transform.localPosition += (Vector3.right * (item.SlotIndex + 1)) * 485;
        }
        else
        {
            item.GameObject.transform.localPosition += (Vector3.right * (item.SlotIndex - 3)) * 485 + (Vector3.down * 505);
        }
    }

    private void DrawItemForChest(Item item)
    {
        // Logic to position the item in the chest's inventory UI
    }

    private void DrawItemForTrade(Item item)
    {
        if (CantidadItems() < 4)
        {
            item.GameObject.transform.localPosition += (Vector3.right * (CantidadItems() + 1)) * 485;
        }
        else
        {
            item.GameObject.transform.localPosition += (Vector3.right * (CantidadItems() - 3)) * 485 + (Vector3.down * 505);
        }
    }

    private void DrawItemForCoin(Item item)
    {
        item.GameObject.transform.localPosition = new Vector3(-882, 468, 0);
    }

    public void DrawAskedItem(Item item)
    {
        item.GameObject.transform.localPosition = new Vector3(22, 479, 0);
    }
    
    public void RemoveAskedItem()
    {
        if (askedItem != null)
        {
            Items.Remove(askedItem);
            askedItem.GameObject.SetActive(false);
            askedItem = null;
        }
    }
}
