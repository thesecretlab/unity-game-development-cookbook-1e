using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN helpbox_demo
public class HelpBoxDemo : MonoBehaviour {

    [HelpBox(text = "Here's a help box above the variable!")]
    [SerializeField] int integer;

}
// END helpbox_demo
