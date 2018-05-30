using UnityEngine;
using System.Collections;
using System;

// BEGIN box_selectable
// Handles the input and display of a selection box.
public class BoxSelectable: MonoBehaviour {
    
    public void Selected() {

        Debug.LogFormat("{0} was selected!", gameObject.name);
        
    }

}
// END box_selectable
