using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN range_checker
public class RangeChecker : MonoBehaviour {

    // The object we want to check the distance to
    [SerializeField] Transform target;

    // If the target is within this many units of us, it's in range
    [SerializeField] float range = 5;

    // Remembers if 
    private bool targetInRange = false;

	void Update () {

        // Calculate the distance between the objects
        var distance = (target.position - transform.position).magnitude;

        if (distance <= range && targetInRange == false) {
            // If the object is now in range, and wasn't before, log it
            Debug.LogFormat("Target {0} entered range!", target.name);

            // Remember that it's in range for next frame
            targetInRange = true;

        } else if (distance > range && targetInRange == true) {
            // If the object is not in range, but was before, log it
            Debug.LogFormat("Target {0} exited range!", target.name);

            // Remember that not it's in range for next frame
            targetInRange = false;
        }

	}
}
// END range_checker
