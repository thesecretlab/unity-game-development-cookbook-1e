using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN fade_after_delay
public class FadeAfterDelay : MonoBehaviour {

    // The number of seconds before a fade starts
    [SerializeField] float delayBeforeFading = 2f;

    // The amount of time to take while fading out
    [SerializeField] float fadeTime = 0.25f;

    // Notice the return type - this Start method is a coroutine!
    IEnumerator Start () {

        // Wait the required amount of time
        yield return new WaitForSeconds(delayBeforeFading);

        // We need a canvas group in order to fade
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null) {
            Debug.LogWarning("Cannot fade - no canvas group attached!");
            yield break;
        }

        // Fade time must be more than zero in order for a fade to be 
        // animated
        if (fadeTime <= 0) {
            yield break;
        }

        // Keep track of how much time we've spent fading
        var fadeTimeElapsed = 0f;

        // Perform the fade every frame
        while (fadeTimeElapsed < fadeTime) {

            fadeTimeElapsed += Time.deltaTime;

            // Calculate the fraction of the fade time (between 0 and 1)
            var t = fadeTimeElapsed / fadeTime;

            // Calculate our alpha; it starts at 1, and goes to 0
            var alpha = 1f - t;

            // Apply the fade
            canvasGroup.alpha = alpha;

            // Wait for the next frame
            yield return null;
        }

        // Remove this game object from the scene
        Destroy(gameObject);
	}
	
	
}
// END fade_after_delay
