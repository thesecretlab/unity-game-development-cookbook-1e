using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinButton : MonoBehaviour {
    // BEGIN spin_button2
    // The amount of time needed to perform a full spin
    [SerializeField] float spinTime = 0.5f;

    // Controls the pacing of the animation.
    [SerializeField] AnimationCurve curve = 
        AnimationCurve.EaseInOut(0, 0, 1, 1);
    // END spin_button2

    // BEGIN spin_button3
    public void Spin() {

        // Start a spin.
        StartCoroutine(StartSpinning());

    }
    // END spin_button3

    // A coroutine that updates the rotation every frame, until it runs out
    // of time.

    // BEGIN spin_button1
    private IEnumerator StartSpinning()
    {
        // Don't do any spinning if spin time is zero or less (
        if (spinTime <= 0) {
            yield break;
        }

        // Keep track of how long we've been spinning for.
        float elapsed = 0f;

        while (elapsed < spinTime) {
            elapsed += Time.deltaTime;

            // Calculate how far along the animation we are, measured
            // between 0 and 1.
            var t = elapsed / spinTime;

            // Use this value to figure out how many degrees we should be 
            // rotated at on this frame.
            var angle = curve.Evaluate(t) * 360f;

            // Calculate the rotation by rotating this many angles around
            // the X axis.
            transform.localRotation = 
                Quaternion.AngleAxis(angle, Vector3.right);

            // Wait a new frame.
            yield return null;
        }

        // The animation is now complete. Reset the rotation to normal.
        transform.localRotation = Quaternion.identity;
    }
    // END spin_button1
}
