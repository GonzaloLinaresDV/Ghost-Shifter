using Fusion;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInventory : NetworkBehaviour
{
    [SerializeField] private int maxWeight = 100;

    private List<InventoryItem> items = new();


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            DebugInventory();
        }
    }

    public int MaxWeight => maxWeight;

    public int CurrentWeight
    {
        get
        {
            int totalWeight = 0;

            foreach (var item in items)
            {
                totalWeight += item.definition.weight * item.amount;
            }

            return totalWeight;
        }
    }

    public int RemainWeight => MaxWeight - CurrentWeight;

    public bool CanAddItem(LootDefinition loot, int amount)
    {
        return CurrentWeight + (loot.weight * amount) <= MaxWeight;
    }
    public bool AddItem(LootDefinition loot, int amount)
    {
        if (!CanAddItem(loot, amount))
            return false;

        InventoryItem existingItem = items.Find(i => i.definition == loot);

        if (existingItem != null)
        {
            existingItem.amount += amount;
        }
        else
        {
            items.Add(new InventoryItem
            {
                definition = loot,
                amount = amount
            });
        }

        return true;
    }
    public bool RemoveItem(LootDefinition loot, int amount)
    {
        InventoryItem existingItem = items.Find(i => i.definition == loot);

        if (existingItem == null)
            return false;

        if (existingItem.amount < amount)
            return false;

        existingItem.amount -= amount;

        if (existingItem.amount <= 0)
        {
            items.Remove(existingItem);
        }

        return true;
    }

    private void DebugInventory()
    {
        Debug.Log("===== INVENTARIO =====");
        Debug.Log($"Peso: {CurrentWeight}/{MaxWeight}");

        if (items.Count == 0)
        {
            Debug.Log("Inventario vacío");
            return;
        }

        foreach (var item in items)
        {
            Debug.Log(
                $"{item.definition.itemName} x{item.amount} | Peso: {item.definition.weight * item.amount}"
            );
        }

        Debug.Log("======================");
    }


}
