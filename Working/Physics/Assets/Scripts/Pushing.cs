using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implements pushing rigidbodies from a charactercollider.
public class Pushing : MonoBehaviour {

    // Defines the possible types of pushing we can apply.
    public enum PushMode
    {
        // Don't allow any pushing
        NoPushing,

        // Push by directly setting the velocity of things we hit
        DirectlySetVelocity,

        // Push by applying a physical force to the impact point
        ApplyForces
    }

    // The type of pushing we've selected.
    [SerializeField] PushMode pushMode = PushMode.DirectlySetVelocity;

    // The amount of force to apply, when push mode is set to ApplyForces.
    [SerializeField] float pushPower = 5;

    // Called when a character collider on the obejct that this script is 
    // attached to touches any other collider.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Immediately exit if pushing is disabled
        if (pushMode == PushMode.NoPushing)
        {
            return;
        }

        // Get the rigidbody attached to the collider we hit
        var hitRigidbody = hit.rigidbody;

        // Is this rigidbody something we can push?
        if (hitRigidbody == null || hitRigidbody.isKinematic == true)
        {
            // Either it doesn't have a rigidbody, or the rigid body is 
            // kinematic (that is, it doesn't respond to external forces.)

            // Since we're going to apply a force to it, we should respect its
            // settings.
            return;
        }

        // Get a reference to the controller that hit the object, since we'll
        // be making references to it often.
        CharacterController controller = hit.controller;

        // Calculate the world position of the lowest point on the controller.
        var footPosition = controller.transform.position.y 
                                     - controller.center.y  
                                     - controller.height / 2;

        // If the thing we've hit is underneath us, then we don't want to push
        // it - it would make it impossible for us to walk on top of it, beacuse
        // it would be "pushed".
        if (hit.point.y <= footPosition ) {
            return;
        }

        // Apply the push, based on our setting.
        switch (pushMode)
        {
            case PushMode.DirectlySetVelocity:
                // Directly apply the velocity. Less realistic, but can feel better.
                hitRigidbody.velocity = controller.velocity;
                break;
            case PushMode.ApplyForces:
                // Calculate how much push force to apply
                Vector3 force = controller.velocity * pushPower;

                // Apply this force to the object we're pushing
                hitRigidbody.AddForceAtPosition(force, hit.point);
                break;
        }
    }

}
