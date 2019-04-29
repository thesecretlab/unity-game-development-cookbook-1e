using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN interacting
// Implements interacting with Interactable objects
public class Interacting : MonoBehaviour {

    // The key to press to interact with an object.
    [SerializeField] KeyCode interactionKey = KeyCode.E;

    // The range at which we can interact with objects.
    [SerializeField] float interactingRange = 2;

    void Update () {

        // Did the user just press the interaction key?
        if (Input.GetKeyDown(interactionKey)) {

            // Then attempt to interact.
            AttemptInteraction();
        }
    }

    void AttemptInteraction() {

        // Create a ray from the current position and forward direction
        var ray = new Ray(transform.position, transform.forward);

        // Store information about the hit in this variable
        RaycastHit hit;

        // Create a layer mask that represents every layer except the 
        // players
        var everythingExceptPlayers = 
            ~(1 << LayerMask.NameToLayer("Player"));

        // Combine this layer mask with the one that raycasts usually use; 
        // this has the effect of removing the player layer from the list 
        // of layers to raycast against
        var layerMask = Physics.DefaultRaycastLayers
                               & everythingExceptPlayers;

        // Perform the raycast out, hitting only object that are on layers 
        // described by the layer mask we just assembled
        if (Physics.Raycast(ray, out hit, interactingRange, layerMask)) {

            // Try and get the Interactable component on the object we hit
            var interactable = hit.collider.GetComponent<Interactable>();

            // Does it exist?
            if (interactable != null) {
                
                // Signal that it was interacted with.
                interactable.Interact(this.gameObject);
            }
        }

    }
}
// END interacting