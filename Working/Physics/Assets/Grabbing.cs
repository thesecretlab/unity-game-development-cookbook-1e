using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We need a rigidbody to connect our joint to
[RequireComponent(typeof(Rigidbody))]
public class Grabbing : MonoBehaviour {
    
    // The range from this object at which an object can be picked up.
    [SerializeField] float grabbingRange = 3;

    // The location at which objects that are picked up will be placed.
    [SerializeField] Transform holdPoint = null;

    // The key to press to pick up or drop an object.
    [SerializeField] KeyCode grabKey = KeyCode.E;

    // If the grab joint encounters this much force, break it.
    [SerializeField] float grabBreakingForce = 100f;

    // If the grab joint encounters this much torque, break it.
    [SerializeField] float grabBreakingTorque = 100f;

    // The joint that holds our grabber object. Null if we're not holding 
    // anything.
    FixedJoint grabJoint;

    // The rigidbody that we're holding. Null if we're not holding anything.
    Rigidbody grabbedRigidbody;

    private void Awake()
    {
        // Do some quick validity checks when we start up

        if (holdPoint == null) {
            Debug.LogError("Grab hold point must not be null!");
        }

        if (holdPoint.IsChildOf(transform) == false) {
            Debug.LogError("Grab hold point must be a child of this object");            
        }

    }

    private void Update()
    {
        // Does the user want to perform a grab interaction?
        if (Input.GetKeyDown(grabKey)) {
            
            // If we're not holding something, attempt to pick something up.
            // If we are holding something, drop it.

            // Are we currently holding something?
            if (grabJoint == null) {
                // Attempt to grab the object under the cursor
                AttemptGrab();
            } else {
                // Drop the object.
                Drop();
            }
        }


    }

    // Attempts to pick up the object directly ahead of this object. When this
    // script is attached to a camera, it will try to pick up the object 
    // directly in the middle of the camera's view. (You may want to add a 
    // reticle to the GUI to help the player know where the precise center of
    // the screen is.
    private void AttemptGrab()
    {
        // Perform a raycast. If we hit something that has a rigidbody and 
        // is not kinematic, pick it up.

        var ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabbingRange) == false)
        {
            // Our raycast hit nothing.
            return;
        }

        // We hit something! Is it something we can pick up?

        grabbedRigidbody = hit.rigidbody;

        if (grabbedRigidbody == null || grabbedRigidbody.isKinematic)
        {
            // We can't pick this up - it either has no rigidbody, or it's
            // kinematic.
            return;
        }

        // Move the body to our grab position
        grabbedRigidbody.transform.position = holdPoint.position;

        // Create a joint that will hold this in place, and configure it
        grabJoint = gameObject.AddComponent<FixedJoint>();
        grabJoint.connectedBody = grabbedRigidbody;
        grabJoint.breakForce = grabBreakingForce;
        grabJoint.breakTorque = grabBreakingTorque;

        // Ensure that this grabbed object doesn't collide with this collider,
        // or any collider in our parent, which could cause problems
        foreach (var myCollider in GetComponentsInParent<Collider>()) {
            Physics.IgnoreCollision(myCollider, hit.collider, true);
        }

    }

    // Drops the object
    private void Drop()
    {
        
        if (grabJoint != null)
        {
            Destroy(grabJoint);
        }

        // Bail out if the object we were holding isn't there anymore
        if (grabbedRigidbody == null)
        {
            return;
        }

        // Re-enable collisions between this object and our collider(s)
        foreach (var myCollider in GetComponentsInParent<Collider>())
        {
            Physics.IgnoreCollision(myCollider, grabbedRigidbody.GetComponent<Collider>(), false);
        }

        grabbedRigidbody = null;
    }

    // Draw the location of the hold point
    private void OnDrawGizmos()
    {
        if (holdPoint == null) {
            return;
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(holdPoint.position, 0.2f);
    }

    // Called when a joint that's attached to the gameobject this component is
    // on has broken.
    private void OnJointBreak(float breakForce)
    {
        // When our joint breaks, call Drop to ensure that
        // we clean up after ourselves.
        Drop();
    }
}
