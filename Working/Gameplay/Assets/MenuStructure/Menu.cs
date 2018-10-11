using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN menu
// Contains UnityEvent, which this script uses
using UnityEngine.Events; 

public class Menu : MonoBehaviour {
 
    // Invoked when a menu appears on screen.
    public UnityEvent menuDidAppear = new UnityEvent();
    
    // Invoked when a menu is removed from the screen.
    public UnityEvent menuWillDisappear = new UnityEvent();

}
// END menu