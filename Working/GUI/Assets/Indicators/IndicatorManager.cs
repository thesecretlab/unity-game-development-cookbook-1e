using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN indicator_manager
public class IndicatorManager : MonoBehaviour {

    // The indicator that appears over each tracked object.
    [SerializeField] RectTransform indicatorPrefab = null;

    // The object that all indicators will go into.
    [SerializeField] RectTransform indicatorContainer = null;

    // The single instance of the indicator manager.
    public static IndicatorManager manager;

    // Maps objects in the world to indicators on screen.
    Dictionary<TrackedObject, RectTransform> indicators = 
        new Dictionary<TrackedObject, RectTransform>();

    private void Awake()
    {
        // Set up the singleton variable to refer to this instance.
        manager = this;
    }

    private void LateUpdate()
    {
        // We do this in LateUpdate so that the calculation of the positions
        // can happen after the objects have moved, which prevents jitter.

        // Every frame, for each object that we're tracking, update the 
        // position of its indicator.
        foreach (var pair in indicators) {
            TrackedObject target = pair.Key;
            RectTransform indicator = pair.Value;

            // Has the target been removed from the scene?
            if (target == null) {
                // Skip this indicator
                continue;
            }

            // Update the indicator's position in the canvas.
            indicator.anchoredPosition = GetCanvasPositionForTarget(target);
        }
    }

    // Returns the location in canvas-space that an indicator should be
    // for a given object
    private Vector2 GetCanvasPositionForTarget(TrackedObject target)
    {
        // Convert the position of the object from world-space to viewport-space
        var indicatorPoint = 
            Camera.main.WorldToViewportPoint(target.transform.position);

        // Viewport coordinates are (0,0) to (1,1); (0,0) is the bottom-left
        // corner of the screen.

        // If a point is outside the screen, we clamp it to the edges.
        indicatorPoint.x = Mathf.Clamp01(indicatorPoint.x);
        indicatorPoint.y = Mathf.Clamp01(indicatorPoint.y);

        // If a point is behind the camera, we force it to the bottom of the
        // screen.
        if (indicatorPoint.z < 0) {
            indicatorPoint.y = 0;

            // We also have to flip it on the X axis, for it to appear 
            // correctly.
            indicatorPoint.x = 1f - indicatorPoint.x;
        }

        // Canvas coordinates are (0,0) -> (width, height); (0,0) is the
        // bottom-left corner of the canvas.

        // This means that we can scale by the canvas' size to get the position 
        // in canvas-space.

        // Get the canvas
        var canvas = indicatorContainer.GetComponentInParent<Canvas>();

        // Get its size
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        // Scale it
        indicatorPoint.Scale(canvasSize);

        // We've now calculated where it belongs in the canvas!
        return indicatorPoint;
    }

    public void AddTrackingIndicator(TrackedObject transform) {

        // Do we already have an indicator for this object?
        if (indicators.ContainsKey(transform)) {
            // Nothing to do; we already have an indicator for this transform
            return;
        }

        // Create our indicator from the prefab
        var indicator = Instantiate(indicatorPrefab);

        // Give it a useful name
        indicator.name = string.Format("Indicator for {0}", 
                                       transform.gameObject.name);

        // Move the indicator into the container
        indicator.SetParent(indicatorContainer, false);

        // Ensure the pivot point is in the center of the object, so that the
        // center of the image is right over the object's position
        indicator.pivot = new Vector2(0.5f, 0.5f);

        // Ensure the object doesn't adjust its size and position based on the 
        // size of its parent
        indicator.anchorMin = Vector2.zero;
        indicator.anchorMax = Vector2.zero;

        // Keep track of the relationship between the target and its indicator
        indicators[transform] = indicator;

        // Place the indicator in the right location
        indicator.anchoredPosition = GetCanvasPositionForTarget(transform);

    }

    // Stops tracking a target.
    public void RemoveTrackingIndicator(TrackedObject transform) {
        
        // If we have an indicator for this target object, remove it from the
        // scene
        if (indicators.ContainsKey(transform)) {
            // Destroy the indicator, if it isn't already gone from the scene.
            if (indicators[transform] != null) {
                Destroy(indicators[transform].gameObject);
            }
        }

        // And remove it from the list, if it's present. (The Remove method
        // won't throw an exception if 'transform' isn't in the dictionary.)
        indicators.Remove(transform);
    }
}
// END indicator_manager
