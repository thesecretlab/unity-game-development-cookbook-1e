﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN orbiting_camera
public class OrbitingCamera : MonoBehaviour
{

    // The object we're orbiting around
    public Transform target;

    // The speed at which we change our rotation and elevation
    public float rotationSpeed = 120.0f;
	public float elevationSpeed = 120.0f;

    // The minimum and maximum angle of elevation
	public float elevationMinLimit = -20f;
	public float elevationMaxLimit = 80f;

    // The distance we're at from the target
	public float distance = 5.0f;
    public float distanceMin = .5f;
    public float distanceMax = 15f;

    // The angle at which we're rotated around the target
	float rotationAroundTarget = 0.0f;
    
    // The angle at which we're looking down or up at the target
	float elevationToTarget = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationAroundTarget = angles.y;
        elevationToTarget = angles.x;

		if (target) {
			// Take the current distance from the camera to the target
			float currentDistance = (transform.position - target.position).magnitude;

            // Clamp it to our required minimum/maximum
			distance = Mathf.Clamp(currentDistance, distanceMin, distanceMax);
		}

  
    }

    void LateUpdate()
    {
        if (target)
        {
			// Update our rotation and elevation based on mouse movement
            rotationAroundTarget += 
				Input.GetAxis("Mouse X") * rotationSpeed * distance * 0.02f;
			
            elevationToTarget -= 
				Input.GetAxis("Mouse Y") * elevationSpeed * 0.02f;

            // Limit the elevation to between the minimum and the maximum
            elevationToTarget = ClampAngle(
				elevationToTarget, 
                elevationMinLimit, 
				elevationMaxLimit
			);

			// Compute the rotation based on these two angles
            Quaternion rotation = Quaternion.Euler(
				elevationToTarget, 
				rotationAroundTarget, 
				0
			);

			// Update the distance based on mouse movement
			distance = distance - Input.GetAxis("Mouse ScrollWheel") * 5;

            // And limit it to the minimum and maximum
            distance = Mathf.Clamp(distance, distanceMin, distanceMax);

            // Figure out a position that's 'distance' units away from the target
			// in the reverse direction to where we're looking
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            // Update the position 
			transform.position = position;

            // Update the rotation so we're looking at the target
            transform.rotation = rotation;
            
        }
    }

    // Clamps an angle between 'min' and 'max', wrapping it if it's less than
    // 360 degrees or higher than 360 degrees.
    public static float ClampAngle(float angle, float min, float max)
    {

        // Wrap the angle at -360 and 360
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;

        // Clamp this wrapped angle
        return Mathf.Clamp(angle, min, max);
    }
}
// END orbiting_camera