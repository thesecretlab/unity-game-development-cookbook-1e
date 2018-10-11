using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public class ItemAmount {
        public InventoryItem item { get; internal set; }

        int _count = 0;

        public int count {
            get {
                return _count;
            }
            set {
                // clamp this value between zero and the largest 
                // possible integer
                _count = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }
    }

    List<ItemAmount> _amounts = new List<ItemAmount>();

    // Exposes the list of amounts as a read-only collection
    public IEnumerable<ItemAmount> amounts { get { return _amounts; } } 

    ItemAmount AmountForItem(InventoryItem item) {
        foreach (var amount in _amounts) {
            if (amount.item == item) {
                return amount;
            }
        }

        // It didn't exist; create one and add it

        var newAmount = new ItemAmount();
        newAmount.item = item;

        _amounts.Add(newAmount);

        return newAmount;
    }

    public void AddItem(InventoryItem item, uint count = 1) {
        AmountForItem(item).count += 1;
    }

    public void RemoveItem(InventoryItem item, uint count = 1)
    {
        AmountForItem(item).count -= 1;

    }

    
}
