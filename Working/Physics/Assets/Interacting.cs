using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implements interacting with Interactable objects
public class Interacting : MonoBehaviour {

    [SerializeField] KeyCode interactionKey = KeyCode.E;

    [SerializeField] float interactingRange = 2;

	void Update () {
        if (Input.GetKeyDown(interactionKey)) {
            AttemptInteraction();
        }
	}

    void AttemptInteraction()
    {
        RaycastHit hit;

        var ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, interactingRange)) {
            var interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null) {
                interactable.Interact(this.gameObject);
            }
        }

    }
}
