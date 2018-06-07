using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN interactable
// Implements being interacted with by an Interacting component.
// Requires a collider, because Interacting objects find their targets by
// casting rays that hit colliders.
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour {

    public void Interact(GameObject fromObject) {
        Debug.LogFormat("I've been interacted with by {0}!", fromObject);
    }
}
// END interactable