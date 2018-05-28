using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN orbiting_camera
public class OrbitingCamera : MonoBehaviour
{

    // The object we're orbiting around
	[SerializeField] Transform target;

    // The speed at which we change our rotation and elevation
	[SerializeField] float rotationSpeed = 120.0f;
	[SerializeField] float elevationSpeed = 120.0f;

    // The minimum and maximum angle of elevation
	[SerializeField] float elevationMinLimit = -20f;
	[SerializeField] float elevationMaxLimit = 80f;

    // The distance we're at from the target
	[SerializeField] float distance = 5.0f;
	[SerializeField] float distanceMin = .5f;
	[SerializeField] float distanceMax = 15f;

    // The angle at which we're rotated around the target
	float rotationAroundTarget = 0.0f;
    
    // The angle at which we're looking down or up at the target
	float elevationToTarget = 0.0f;

	// BEGIN orbiting_camera_clip_variables
    // When true, the camera will adjust its distance when there's a collider 
	// between it and the target
	[SerializeField] bool clipCamera;
	// END orbiting_camera_clip_variables

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

	// Every frame, after all Update() functions are called, update the camera
    // position and rotation
    //
    // We do this in LateUpdate so that if the object we're tracking has its 
	// position changed in the Update method, the camera will be correctly
	// positioned, because LateUpdate is always run afterwards.
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

			// BEGIN orbiting_camera_clip_code
			if (clipCamera) {

                // We'll cast out a ray from the target to the position we just
                // computed. If the ray hits something, we'll update our position
                // to where the ray hit.

                // Store info about any hit in here.
				RaycastHit hitInfo;

                // Generate a ray from the target to the camera
                var ray = new Ray(target.position, position - target.position);

				// Perform the ray cast; if it hit anything, it returns true,
                // and updates the hitInfo variable
                var hit = Physics.Raycast(ray, out hitInfo, distance);

				if (hit) {
					// We hit something. Update the camera position to where
                    // the ray hit an object.
					position = hitInfo.point;
				}
			}
			// END orbiting_camera_clip_code

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