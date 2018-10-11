using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN list
public class List : MonoBehaviour {

    // The number of items to create
    [SerializeField] int itemCount = 5;

    // Each list item will be of this type
    [SerializeField] ListItem itemPrefab;

    // The object that new items should be inserted into
    [SerializeField] RectTransform itemContainer;

    void Start () {

        // Create as many items as we need to
        for (int i = 0; i < itemCount; i++)
        {
            var label = string.Format("Item {0}", i);

            // Create a new item
            CreateNewListItem(label);

        }

    }

    public void CreateNewListItem(string label)
    {
        var newItem = Instantiate(itemPrefab);

        // Place it in the container; tell it to not keep its current
        // position or scale, so it will be laid out correctly by the UI
        // system
        newItem.transform.SetParent(
            itemContainer, 
            worldPositionStays: false
        );

        // Give it a label
        newItem.Label = label;
    }

}
// END list