using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN moving_platform
// Moves an object at a fixed speed through a series of points.
public class MovingPlatform : MonoBehaviour {

    // The positions that the platform will move through stored in local
    // position.
    [SerializeField] Vector3[] points = {};

    // The speed at which it will move between them.
    [SerializeField] float speed = 10f;

    // The index into the 'points' array; this is the point we're trying to
    // move towards
    int nextPoint = 0;

    // Where the platform was when the game started
    Vector3 startPosition;

    // How fast this platform is currently moving, in units per second
    public Vector3 velocity { get; private set; }

    // Use this for initialization
    void Start () {
        if (points == null || points.Length < 2) {
            Debug.LogError("Platform needs 2 or more points to work.");
            return;
        }

        // All of our movement points are defined relative to where we are
        // when the game starts, so record that (since transform.position
        // will change over time)
        startPosition = transform.position;

        // Start our cycle at our first point
        transform.position = currentPoint;
    }

    // Returns the point that we're currently moving towards.
    Vector3 currentPoint {
        get {
            // If we have no points, return our current position
            if (points == null || points.Length == 0) {
                return transform.position;
            }
            // Return the point we're trying to get to
            return points[nextPoint] + startPosition;
        }
    }
    
    // Update every time physics updates
    void FixedUpdate () {
        
        // Move towards the target, at a fixed speed
        var newPosition = Vector3.MoveTowards(
            transform.position, currentPoint, speed * Time.deltaTime);

        // Have we reached the target?
        if (Vector3.Distance(newPosition, currentPoint) < 0.001) {
            // Snap to the target point
            newPosition = currentPoint;

            // Move to the next target, wrapping around to the start if 
            // necessary
            nextPoint += 1;
            nextPoint %= points.Length;
        }

        // Calculate our current velocity in units-per-second
        velocity = 
            (newPosition - transform.position) / Time.deltaTime;

        // Update to our new location
        transform.position = newPosition;

        
    }

    // Draw the path that the platform will follow
    private void OnDrawGizmosSelected()
    {
        if (points == null || points.Length < 2) {
            return;
        }

        // Our points are stored in local space, so we need to offset them
        // in order to know where they are in world space.
        Vector3 offsetPosition = transform.position;

        // If we're playing, our transform is moving, so we need to use the
        // cached start position to figure out where our points are in
        // world space.
        if (Application.isPlaying) {
            offsetPosition = startPosition;
        }

        Gizmos.color = Color.blue;

        // Loop over all the points
        for (int p = 0; p < points.Length; p++) {

            // Get this point and the next one, wrapping around to the
            // first
            var p1 = points[p];
            var p2 = points[(p + 1) % points.Length];

            // Draw the point
            Gizmos.DrawSphere(offsetPosition + p1, 0.1f);

            // Draw the line between the points
            Gizmos.DrawLine(offsetPosition + p1, offsetPosition + p2);
        }
    }
}
// END moving_platform
