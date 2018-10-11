using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN 3d_movement
// Implements character controller movement.
public class Movement : MonoBehaviour {

    // The speed at which we can move, in units per second.
    [SerializeField] float moveSpeed = 6;

    // The height of a jump, in units.
    [SerializeField] float jumpHeight = 2;

    // The rate at which our vertical speed will be reduced, in units per
    // second. 
    [SerializeField] float gravity = 20;

    // The degree to which we can control our movement while in mid-air.
    [Range(0, 10), SerializeField] float airControl = 5;

    // Our current movement direction. If we're on the ground, we have
    // direct control over it, but if we're in the air, we only have
    // partial control over it.
    Vector3 moveDirection = Vector3.zero;

    // A cached reference to the character controller, which we'll be using
    // often.
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // We do our movement logic in FixedUpdate so that our movement can
    // happen at the same pace as physics updates. If it didn't, we'd see
    // jitter when we interact with physics objects that can move around.
    void FixedUpdate () {
        

        // The input vector describes the user's desired local-space
        // movement; if we're on the ground, this will immediately become
        // our movement, but if we're in the air, we'll interpolate between
        // our current movement and this vector, to simulate momentum.
        var input = new Vector3(
            Input.GetAxis("Horizontal"), 
            0, 
            Input.GetAxis("Vertical")
        );

        // Multiply this movement by our desired movement speed
        input *= moveSpeed;

        // The controller's Move method uses world-space directions, so we
        // need to convert this direction to world space
        input = transform.TransformDirection(input);

        // Is the controller's bottom-most point touching the ground?
        if (controller.isGrounded)
        {
            // Figure out how much movement we want to apply in
            // local-space.
            moveDirection = input;

            // Is the user pressing the jump button right now?
            if (Input.GetButton("Jump"))
            {
                // Calculate the amount of upwards speed we need,
                // considering that we add moveDirection.y to our height
                // every frame, and we reduce moveDirection.y by gravity
                // every frame.
                moveDirection.y = Mathf.Sqrt(2 * gravity * jumpHeight);
            } else {
                // We're on the ground, but not jumping. Set our downwards
                // movement to zero (otherwise, because we're continuously 
                // reducing our Y movement, if we walk off a ledge, we'd
                // suddenly have a huge amount of downwards momentum.)
                moveDirection.y = 0;
            }
        } else {
            // Slowly bring our movement towards the user's desired input,
            // but preserve our current y direction (so that the arc of the
            // jump is preserved)
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, 
                                         airControl * Time.fixedDeltaTime);
        }

        // Bring our movement down by applying gravity over time
        moveDirection.y -= gravity * Time.fixedDeltaTime;

        // Move the controller. The controller will refuse to move into
        // other colliders, which means that we won't clip through the
        // ground or other colliders. (However, this doesn't stop other
        // colliders from moving into us. For that, we'd need to detect
        // when we're overlapping another collider, and move away from
        // them. We'll cover this in another recipe!)
        controller.Move(moveDirection * Time.fixedDeltaTime);
    }
}
// END 3d_movement