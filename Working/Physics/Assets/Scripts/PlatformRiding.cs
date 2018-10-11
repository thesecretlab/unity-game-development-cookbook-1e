using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN platform_riding
// Implements platform riding (standing on a moving platform means we'll
// move with the platform), and pushing (if an object moves into us, it
// will push us away)
[RequireComponent(typeof(CharacterController))]
public class PlatformRiding : MonoBehaviour {

    // The CharacterController on this object.
    CharacterController controller;

    private void Start()
    {
        // We'll be checking our character controller a lot. Cache a
        // reference to it.
        controller = GetComponent<CharacterController>();
    }

    // Every time physics updates, check to see if our collider is
    // overlapping something, and if it is, push ourselves out of it.
    private void FixedUpdate()
    {
        // First, we'll handle pushing the character collider out of the
        // way if another object moves into it.

        // A character collider's physical shape is a capsule. We need to
        // ask the physics system if this capsule is overlapping anything
        // else; to do this, we need to figure out the values that define
        // this capsule.

        // You can think of a capsule as a cylinder with two spheres on
        // either end, where the spheres have the same radius as the
        // cylinder. This means that a capsule can be defined by three
        // values: the locations of the centres of the two spheres, and the
        // radius.

        // Given that a character collider exposes its total height
        // (including spheres!) and the radius, we can use this to figure
        // out the location of the two capsule points in world-space.

        // The center of the sphere at the top of the controller's capsule
        var capsulePoint1 = transform.position + new Vector3(
            0, (controller.height / 2) - controller.radius, 0);

        // The center of the sphere at the bottom of the controller's
        // capsule
        var capsulePoint2 = transform.position - new Vector3(
            0, (controller.height / 2) + controller.radius, 0);

        // The list of colliders we may be overlapping. We're unlikely to
        // overlap more than ten colliders, so make the list that long.
        // (Adjust this if you're encountering lots of overlaps.)
        Collider[] overlappingColliders = new Collider[10];

        // Figure out which colliders we're overlapping. We pass in the
        // overlappingColliders array, and it when this function returns,
        // the array will be filled with references to other colliders. The
        // function returns the number of colliders that overlap the
        // capsule.
        var overlapCount  = Physics.OverlapCapsuleNonAlloc(
            capsulePoint1, capsulePoint2,  // the centers of the spheres
            controller.radius,  // the radius of the spheres
            overlappingColliders);

        // (Note: we _could_ have used OverlapCapsule, which returns a
        // brand- new array, but that requires the function to allocate the
        // memory for it on the heap. Because we don't use this array after
        // this function ends, the array would turn into garbage. More
        // garbage means the garbage collector will run more often, which
        // means performance hitches. By creating our own array locally,
        // it's stored on the stack; data on the stack doesn't get turned
        // into garbage when it goes away, but it can't stay around after
        // this function returns, which is fine for this case.)

        // For each item we were told the capsule overlaps...
        for (int i = 0; i < overlapCount; i++) {

            // Get the collider the capsule overlaps
            var overlappingCollider = overlappingColliders[i];

            // If this collider is our controller, ignore it
            if (overlappingCollider == controller)  {
                continue;
            }

            // We need to compute how much movement we need to perform to
            // not overlap this collider.

            // First, define some variables to store the direction and
            // distance.
            Vector3 direction;
            float distance;

            // Next, provide information about both our collider and the
            // other one. Our direction and distance variables will be
            // filled with data.
            Physics.ComputePenetration(
                controller,  // our collider
                transform.position, // its position
                transform.rotation, // its orientation
                overlappingCollider, // the other collider
                overlappingCollider.transform.position,  // its position
                overlappingCollider.transform.rotation,  // its orientation
                out direction, // will hold the direction we should move in
                out distance // will contain the distance we should move by
            );

            // Don't get pushed vertically; that's what 1. gravity and 2.
            // moving platforms are for.
            direction.y = 0;

            // Update our position to move out of the way.
            transform.position += direction * distance;

        }

        // Next, we'll handle standing on a moving platform.

        // Cast a ray down to our feet. If it hit a MovingPlatform, inherit
        // its velocity.

        // (We don't need to worry about avoiding the character controller
        // here, because the raycast starts inside the controller, so it
        // won't hit it.)

        var ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        // The maximum distance we want to look for.
        float maxDistance = (controller.height / 2f) + 0.1f;

        // Cast the ray. Did it hit anything?
        if (Physics.Raycast(ray, out hit, maxDistance)) {

            // It did!

            // Did it have a MovingPlatform component?
            var platform = hit.collider.gameObject
                              .GetComponent<MovingPlatform>();

            if (platform != null) {
                // If it did, update our position based on the platform's
                // current velocity.
                transform.position += 
                    platform.velocity * Time.fixedDeltaTime;
            }
        }
    }
}
// END platform_riding
