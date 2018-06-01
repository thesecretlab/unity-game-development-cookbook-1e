using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN list_item
public class ListItem : MonoBehaviour {

    // The Text object that displays our label
    [SerializeField] UnityEngine.UI.Text labelText;

    // Expose a string; setting or getting this will set or get the text of
    // the label.
	public string Label
    {
        get
        {
            return labelText.text;
        }
        set
        {
            labelText.text = value;
        }
    }

}
// END list_item