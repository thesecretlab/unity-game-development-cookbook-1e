using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN top_down_camera
// Allows for limited top-down movement of a camera.
public class TopDownCameraMovement : MonoBehaviour {

    // The speed that the camera will move, in units per second
    [SerializeField] float movementSpeed = 20;

    // The lower-left position of the camera, on its current X-Z plane.
    [SerializeField] Vector2 minimumLimit = -Vector2.one;

    // The upper-right position of the camera, on its current X-Z plane.
    [SerializeField] Vector2 maximumLimit = Vector2.one;

    // Every frame, update the camera's position
    void Update()
    {
        // Get how much the user wants to move the camera
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        // Compute how much movement we want to apply this frame, in
        // world-space.
        var offset = new Vector3(horizontal, 0, vertical) 
            * Time.deltaTime * movementSpeed;

        // Figure out what our new position would be.
        var newPosition = transform.position + offset;

        // Is this new position within our permitted bounds?
        if (bounds.Contains(newPosition)) {
            // Then move to it.
            transform.position = newPosition;
        } else {
            // Otherwise, figure out the closest point to the boundary, and
            // move there instead.
            transform.position = bounds.ClosestPoint(newPosition);
        }
    }

    // Computes the bounding box that the camera is allowed to be in.
    Bounds bounds {
        get {

            // We'll create a bounding box that's zero units high,
            // and positioned at the current height of the camera.
            var cameraHeight = transform.position.y;

            // Figure out the position of the corners of the boxes in
            // world-space
            Vector3 minLimit = 
                new Vector3(minimumLimit.x, cameraHeight, minimumLimit.y);
            Vector3 maxLimit = 
                new Vector3(maximumLimit.x, cameraHeight, maximumLimit.y);

            // Create a new Bounds using these values and return it
            var newBounds = new Bounds();
            newBounds.min = minLimit;
            newBounds.max = maxLimit;

            return newBounds;
        }
    }

    // Draw the bounding box.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
// END top_down_camera