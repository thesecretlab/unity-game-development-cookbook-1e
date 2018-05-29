using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BEGIN speed_boost
public class SpeedBoost : MonoBehaviour {

    // The amount of time the boost should apply
    [SerializeField] float boostDuration = 1;

    // The amount of force to apply in the forward direction
    [SerializeField] float boostForce = 50;

    // Called when a rigidbody enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Ensure this collider has a rigidbody, either on itself
        // or on a parent object
        var body = other.GetComponentInParent<Rigidbody>();

        if (body == null) {
            return;
        }

        // Attach a ConstantForce component to it
        var boost = body.gameObject.AddComponent<ConstantForce>();

        // Make the ConstantForce boost the object forward by the specified
        // amount
        boost.relativeForce = Vector3.forward * boostForce;

        // Remove this ConstantForce after boostDuration seconds
        Destroy(boost, boostDuration);
    }
}
// END speed_boost
