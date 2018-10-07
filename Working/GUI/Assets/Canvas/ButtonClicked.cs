using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN button_clicked
public class ButtonClicked : MonoBehaviour {

    // This will appear in the list of methods
    // BEGIN button_clicked
    public void ButtonWasClicked() {
        Debug.Log("The button was clicked!");
    }
    // END button_clicked
    // This will appear in the list of methods, and let you specify a parameter
    // BEGIN button_clicked2
    public void ButtonWasClickedWithParameter(string parameter) {
        string message = 
            string.Format("The button was clicked: {0}", parameter);
        
        Debug.Log(message);
    }
    // END button_clicked2
    // This won't appear in the list of methods, because it's private
    // (even though we didn't specify its protection level, because all class
    // methods are private by default in C#)
    // BEGIN button_clicked3
    void PrivateButtonWasClicked() {
        Debug.Log("This won't run as the direct result of a button click!");
    }
    // END button_clicked3

}
