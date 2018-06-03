using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN tracked_object
public class TrackedObject : MonoBehaviour {

	void Start () {
        // When the object first appears, request an indicator.
        IndicatorManager.manager.AddTrackingIndicator(this);
	}

    // Tell the indicator manager to remove our tracking indicator.
    // OnDestroy is called when either the object is removed from the scene,
    // or the scene is being unloaded (including when we exit play mode).
    private void OnDestroy()
    {
        IndicatorManager.manager.RemoveTrackingIndicator(this);

    }

}
// END tracked_object