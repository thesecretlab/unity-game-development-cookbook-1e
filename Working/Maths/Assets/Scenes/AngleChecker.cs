using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN angle_checker
public class AngleChecker : MonoBehaviour {

    // The object we want to find the angle to
    [SerializeField] Transform target;

    
    void Update () {

        // Get the normalised direction to the target
        var directionToTarget = 
            (target.position - transform.position).normalized;

        // Take the dot product between that direction and our forward
        // direction
        var dotProduct = Vector3.Dot(transform.forward, directionToTarget);

        // Get the angle
        var angle = Mathf.Acos(dotProduct);

        // Log the angle, limiting it to 1 decimal place
        Debug.LogFormat(
            "The angle between my forward direction and {0} is {1:F1}°",
            target.name, angle * Mathf.Rad2Deg
        );

    }
}
// END angle_checker