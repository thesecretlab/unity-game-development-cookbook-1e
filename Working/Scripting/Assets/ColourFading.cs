using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN getcomponent_demo
public class ColourFading : MonoBehaviour
{
    
    // BEGIN getcomponent_demo_cached
    // The mesh renderer component will be stored in this after Start
    MeshRenderer meshRenderer; 

    void Start()
    {
        // Get the component and cache it
        meshRenderer = GetComponent<MeshRenderer>();
    }
    // END getcomponent_demo_cached

    // BEGIN getcomponent_demo_update
    void Update()
    {
        // BEGIN getcomponent_demo_noncached
        var meshRenderer = GetComponent<MeshRenderer>();
        // END getcomponent_demo_noncached

        // Check to make sure that it's valid before we use it
        if (meshRenderer == null) {
            return;
        }
        // BEGIN getcomponent_demo_cached
        
        // meshRenderer has already been stored, so we can just
        // start using it
        // END getcomponent_demo_cached

        var sineTime = Mathf.Sin(Time.time) + 1 / 2f;
        var color = new Color(sineTime, 0.5f, 0.5f);
        meshRenderer.material.color = color;
    }
    // END getcomponent_demo_update
}
// END getcomponent_demo