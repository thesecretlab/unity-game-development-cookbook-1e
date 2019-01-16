using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN range_checker
public class RangeChecker : MonoBehaviour {

    // The object we want to check the distance to
    [SerializeField] Transform target;

    // If the target is within this many units of us, it's in range
    [SerializeField] float range = 5;

    // Remembers if the target was in range on the previous frame.
    private bool targetWasInRange = false;

    void Update () {

        // Calculate the distance between the objects
        var distance = (target.position - transform.position).magnitude;

        if (distance <= range && targetWasInRange == false) {
            // If the object is now in range, and wasn't before, log it
            Debug.LogFormat("Target {0} entered range!", target.name);

            // Remember that it's in range for next frame
            targetWasInRange = true;

        } else if (distance > range && targetWasInRange == true) {
            // If the object is not in range, but was before, log it
            Debug.LogFormat("Target {0} exited range!", target.name);

            // Remember that it's no longer in range for next frame
            targetWasInRange = false;
        }

    }
}
// END range_checker
