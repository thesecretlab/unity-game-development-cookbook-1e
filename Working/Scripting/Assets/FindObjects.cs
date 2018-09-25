using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // BEGIN find_single_object
        // Find a single Mover object in the scene
        var mover = FindObjectOfType<Mover>();
        // END find_single_object

        // BEGIN find_multiple_objects
        // Find all Mover objects in the scene  
        var allMovers = FindObjectsOfType<Mover>();
        // END find_multiple_objects
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
