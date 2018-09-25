using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutines : MonoBehaviour
{
    // Start is allowed to be a coroutine. Just set its
    // return type to IEnumerator.
    // BEGIN coroutines_start
    IEnumerator Start()
    {
        Debug.Log("Hello...");

        yield return new WaitForSeconds(1);

        Debug.Log("...world!");
    }
    // END coroutines_start

    private void Awake() {
        // BEGIN coroutines_simple_usage
        StartCoroutine(LogAfterDelay());
        // END coroutines_simple_usage

        // BEGIN coroutines_parameters_usage
        // Parameters for coroutines are supplied as part of the 
        // function call
        StartCoroutine(Countdown(5));
        // END coroutines_parameters_usage
    }

    // BEGIN coroutines_yield_null
    IEnumerator RunEveryHundredFrames() {
        while (true) {
            // Yielding 'null' will wait one frame.
            yield return null;   

            if (Time.frameCount % 100 == 0) {
                Debug.LogFormat("Frame {0}!", Time.frameCount);
            } 
            
        }        
    }
    // END coroutines_yield_null

    // BEGIN coroutines_simple
    IEnumerator LogAfterDelay() {
        Debug.Log("Back in a second!");

        yield return new WaitForSeconds(1);

        Debug.Log("I'm back!");
    }
    // END coroutines_simple

    // Be careful while using while(true)!
    // If you don't yield at some point in the loop,
    // Unity will not be able to break out of it, and
    // will freeze up. You won't be able to leave play mode,
    // and you won't be able to save any unsaved changes
    // in your scene. Be careful!
    
    // BEGIN coroutines_loops
    IEnumerator LogEveryFiveSeconds() {

        // Enter an infinite loop, in which we wait for
        // five seconds and then do something useful
        while (true) {
            yield return new WaitForSeconds(5);
            Debug.Log("Hi!");
        }
        
    }
    // END coroutines_loops

    // Coroutines can receive parameters
    // BEGIN coroutines_parameters
    IEnumerator Countdown(int times) {
        for (int i = times; i > 0; i++) {
            Debug.LogFormat("{0}...", i);
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Done counting!");
    }
    // END coroutines_parameters

    IEnumerator StoppingCoroutine() {
        // BEGIN coroutines_breaking
        while (true) {
            yield return null;
            
            // Stop on frame 354
            if (Time.frameCount == 354) {
                yield break;
            }
        }
        // END coroutines_breaking
    }

    IEnumerator WaitWhileAndUntilExample() {

        // BEGIN coroutines_waitwhile
        // Wait while the Y position of this object is below 5
        yield return new WaitWhile(() => transform.position.y < 5);
        // END coroutines_waitwhile

        // BEGIN coroutines_waituntil
        // Wait _until_ the Y position of this object is below 5
        yield return new WaitUntil(() => transform.position.y < 5);
        // END coroutines_waituntil
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
