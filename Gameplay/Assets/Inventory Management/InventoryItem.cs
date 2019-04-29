using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory Item", order = 110)]
public class InventoryItem : ScriptableObject
{

    public Texture2D icon;

    // The maximum number of this kind of item that can be carried.
    // If zero, an unlimited number can be carried. 
    public uint maxCarryable;

}
