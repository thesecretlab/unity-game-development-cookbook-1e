using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN multivaluedemo
// A simple component to demo the MultiValueChooser property drawer.
public class MultiValueDemo : MonoBehaviour {

    [SerializeField] 
    MultiValue multiValue = new MultiValue("One", "Two", "Three");
}
// END multivaluedemo
