using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN scripting_scriptableobject
// Create an entry in the Asset -> Create menu that makes it easy to create
// a new asset of this type
[CreateAssetMenu]

// Don't forget to change the parent class from 'MonoBehaviour' to 
// 'ScriptableObject'!
public class ObjectColour : ScriptableObject
{
    public Color color;
}
// END scripting_scriptableobject