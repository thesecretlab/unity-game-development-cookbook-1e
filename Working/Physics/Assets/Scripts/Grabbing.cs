using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN grabbing
// Implements pulling, grabbing, holding and throwing.
// A rigidbody is required because we need one to connect our grabbing 
// joint to
[RequireComponent(typeof(Rigidbody))]
public class Grabbing : MonoBehaviour {
    
    // The range from this object at which an object can be picked up.
    [SerializeField] float grabbingRange = 3;

    // The range from this object at which an object can be pulled towards 
    // us.
    [SerializeField] float pullingRange = 20;

    // The location at which objects that are picked up will be placed.
    [SerializeField] Transform holdPoint = null;

    // The key to press to pick up or drop an object.
    [SerializeField] KeyCode grabKey = KeyCode.E;

    // The key to press to throw an object
    [SerializeField] KeyCode throwKey = KeyCode.Mouse0;

    // The amount of force to apply on a thrown object
    [SerializeField] float throwForce = 100f;

    // The amount of force to apply on objects that we're pulling towards 
    // us. Don't forget that objects we're pulling will have friction 
    // working against us, so the value might need to be higher than you 
    // think.
    [SerializeField] float pullForce = 50f;

    // If the grab joint encounters this much force, break it.
    [SerializeField] float grabBreakingForce = 100f;

    // If the grab joint encounters this much torque, break it.
    [SerializeField] float grabBreakingTorque = 100f;

    // The joint that holds our grabber object. Null if we're not holding 
    // anything.
    FixedJoint grabJoint;

    // The rigidbody that we're holding. Null if we're not holding 
    // anything.
    Rigidbody grabbedRigidbody;

    private void Awake()
    {
        // Do some quick validity checks when we start up

        if (holdPoint == null) {
            Debug.LogError("Grab hold point must not be null!");
        }

        if (holdPoint.IsChildOf(transform) == false) {
            Debug.LogError(
                "Grab hold point must be a child of this object"
                );   
        }

        var playerCollider = GetComponentInParent<Collider>();

        playerCollider.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        // Is the user holding the grab key, and we're not holding 
        // something?
        if (Input.GetKey(grabKey) && grabJoint == null) {

            // Attempt to perform a pull or a grab
            AttemptPull();

        } 
        // Did the user just press the grab key, and we're holding 
        // something?
        else if (Input.GetKeyDown(grabKey) && grabJoint != null) {
            Drop();
        }
        // Does the user want to throw the held object, and we're holding 
        // something?
        else if (Input.GetKeyDown(throwKey) && grabJoint != null) {
            // Now apply the throw force
            Throw ();
        }




    }

    // Throws a held object
    void Throw()
    {
        // Can't throw if we're not holding anything!
        if (grabbedRigidbody == null) {
            return;
        }

        // Keep a reference to the body we were holding, because Drop will 
        // reset  it
        var thrownBody = grabbedRigidbody;


        // Calculate the force to apply
        var force = transform.forward * throwForce;

        // And apply it
        thrownBody.AddForce(force);

        // We need to drop what we're holding before we can throw it
        Drop();

    }

    // Attempts to pull or pick up the object directly ahead of this 
    // object. When this script is attached to a camera, it will try to 
    // get the object directly in the middle of the camera's view. (You 
    // may want to add a reticle to the GUI to help the player know where 
    // the precise center of the screen is.
    private void AttemptPull()
    {
        // Perform a raycast. If we hit something that has a rigidbody and 
        // is not kinematic, pick it up.


        // Create a ray that goes from our current position, and goes out 
        // along our current direction.
        var ray = new Ray(transform.position, transform.forward);

        // Create a variable to store the results of what we hit.
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

        // Perform a raycast that uses this layermask to ignore the 
        // players. We use our pulling range because it's the longest; if 
        // the object is actually within our (shorter) grabbing range, 
        // we'll grab it instead of pulling it.
        var hitSomething = 
            Physics.Raycast(ray, out hit, pullingRange, layerMask);
        
        if (hitSomething == false)
        {
            // Our raycast hit nothing within the pulling range.
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

        // We now have an object that's within our pulling range.

        // Is the object within the grabbing range, too?
        if (hit.distance < grabbingRange) {
            
            // We can pick it up.

            // Move the body to our grab position.
            grabbedRigidbody.transform.position = holdPoint.position;

            // Create a joint that will hold this in place, and configure 
            // it
            grabJoint = gameObject.AddComponent<FixedJoint>();
            grabJoint.connectedBody = grabbedRigidbody;
            grabJoint.breakForce = grabBreakingForce;
            grabJoint.breakTorque = grabBreakingTorque;

            // Ensure that this grabbed object doesn't collide with this 
            // collider,  or any collider in our parent, which could cause 
            // problems
            foreach (var myCollider in GetComponentsInParent<Collider>())
            {
                Physics.IgnoreCollision(myCollider, hit.collider, true);
            }
        } else {
            // It's not in grabbing range, but it is in pulling range. 
            // Pull it towards us, until it's in grabbing range.

            var pull = -transform.forward * this.pullForce;

            grabbedRigidbody.AddForce(pull);

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
            Physics.IgnoreCollision(
                myCollider, 
                grabbedRigidbody.GetComponent<Collider>(), 
                false);
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

    // Called when a joint that's attached to the gameobject this 
    // component is on has broken.
    private void OnJointBreak(float breakForce)
    {
        // When our joint breaks, call Drop to ensure that
        // we clean up after ourselves.
        Drop();
    }
}
// END grabbing
