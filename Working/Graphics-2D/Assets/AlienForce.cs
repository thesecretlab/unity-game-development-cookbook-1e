using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN sprite_force_apply
public class AlienForce : MonoBehaviour {

    // The amount of force to apply.
    [SerializeField] float verticalForce = 1000;
    [SerializeField] float sidewaysForce = 1000;

    // A cached reference to the Rigidbody2D on this object.
    Rigidbody2D body;

    // On game start, get the reference to the rigid body and cache it.
    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    // You will get smoother movement if you apply physical forces in
    // FixedUpdate, because it's called a fixed number of times per second,
    // with a fixed simulation timestep, which means more stable simulation
    void FixedUpdate () {
    
    
        // Get user input, and scale it by the amount of force we want to apply
        var vertical = Input.GetAxis("Vertical") * verticalForce;
        var horizontal = Input.GetAxis("Horizontal") * sidewaysForce;

        // Generate a force vector from these inputs, scaled by time
        var force = new Vector2(horizontal, vertical) * Time.fixedDeltaTime;

        // Add the force to the sprite
        body.AddForce(force);
    }
}
// END sprite_force_apply