using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN scripting_scriptableobject_using
public class SetColourOnStart : MonoBehaviour
{
    // The ScriptableObject that we'll draw data from
    [SerializeField] ObjectColour objectColour;

    private void Update()
    {
        // Don't try to use the ObjectColour if it hasn't been provided
        if (objectColour == null)
        {
            return;
        }

        GetComponent<Renderer>().material.color = objectColour.color;   
    }
}
// END scripting_scriptableobject_using