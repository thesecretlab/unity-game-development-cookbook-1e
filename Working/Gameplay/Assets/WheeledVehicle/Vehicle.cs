using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN vehicle
// Configures a single wheel's control parameters.
[System.Serializable]
public class Wheel {
	// The collider this wheel uses
	public WheelCollider collider;

    // Whether this wheel should be powered by the engine
	public bool powered;

    // Whether this wheel is steerable
	public bool steerable;

    // Whether this wheel can apply brakes
	public bool hasBrakes;
}

// Controls the power, braking and steering applied to wheels.
public class Vehicle : MonoBehaviour {

    // The list of wheels on this vehicle
	[SerializeField] Wheel[] wheels = {};

	// The settings used for controlling the wheels:

    // Maximum motor torque
	[SerializeField] float motorTorque = 1000;

    // Maximum brake torque
	[SerializeField] float brakeTorque = 2000;

    // Maximum steering angle
	[SerializeField] float steeringAngle = 45;

	private void Update() {

        // If the Vertical axis is positive, apply motor torque and no brake torque.
        // If it's negative, apply brake torque and no motor torque.
		var vertical = Input.GetAxis("Vertical");

		float motorTorqueToApply;
		float brakeTorqueToApply;

		if (vertical >= 0) {
			motorTorqueToApply = vertical * motorTorque;
			brakeTorqueToApply = 0;
		} else {
			// If the vertical axis is negative, cut the engine and step on the
            // brakes.

            // We use Mathf.Abs here to ensure that we use the positive value
            // of 'vertical' (because applying negative braking torque would
            // lead to weirdness.)
			motorTorqueToApply = 0;
			brakeTorqueToApply = Mathf.Abs(vertical) * brakeTorque;
		}

        // Scale the maximum steering angle by the horizontal axis.
		var currentSteeringAngle = Input.GetAxis("Horizontal") * steeringAngle;

        // Update all wheels

        // Using a for loop, rather than a foreach loop, because foreach loops
        // allocate temporary memory, which is turned into garbage at the end of
        // the frame. We want to minimise garbage, because the more garbage that
        // gets generated, the more often the garbage collector has to run, which
        // causes performance problems.
		for (int wheelNumber = 0; wheelNumber < wheels.Length; wheelNumber++) {

            var wheel = wheels[wheelNumber];

            // If a wheel is powered, it updates its motor torque
			if (wheel.powered) {
				wheel.collider.motorTorque = motorTorqueToApply;
			}

            // If a wheel is steerable, it updates its steer angle
			if (wheel.steerable) {
				wheel.collider.steerAngle = currentSteeringAngle;
			}

            // If a wheel has brakes, it updates its brake torque
			if (wheel.hasBrakes) {
				wheel.collider.brakeTorque = brakeTorqueToApply;
			}
		}
	}
}
// END vehicle
