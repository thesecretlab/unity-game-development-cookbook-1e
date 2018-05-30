using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] UnityEngine.UI.Text text;

    Inventory _inventory;

    public Inventory inventory
    {
        get
        {
            if (_inventory == null) {
                _inventory = GetComponent<Inventory>();
            }
            return _inventory;
        }
    }

    void UpdateDisplay() {
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var amount in inventory.amounts) {
            stringBuilder.AppendFormat("{0} ({1})\n", amount.item.name, amount.count);
        }

        text.text = stringBuilder.ToString();
    }

    public void AddInventoryItem(InventoryItem item) {
        _inventory.AddItem(item);

        UpdateDisplay();
    }

    public void RemoveInventoryItem(InventoryItem item)
    {
        _inventory.RemoveItem(item);

        UpdateDisplay();
    }
}
