using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN level_loading_using
using UnityEngine.SceneManagement;
// END level_loading_using

public class LevelLoading : MonoBehaviour
{
    public void LoadLevel() {
        // BEGIN level_loading_sync
        // Loads the scene named "Game", replacing the current scene; 
        // stops the game until loading is done (so don't load big scenes
        // this way!)
        SceneManager.LoadScene("Game");
        // END level_loading_sync
    }

    public void LoadLevelAsync() {
        
        // BEGIN level_loading_async
        // Start loading the scene; we'll get back an object
        // that represents the scene loading operation
        var operation = SceneManager.LoadSceneAsync("Game");

        Debug.Log("Starting load...");

        // Don't proceed to the scene once loading has finished
        operation.allowSceneActivation = false;

        // Start a coroutine that will run while scene loads, and
        // will run some code after loading finishes
        StartCoroutine(WaitForLoading(operation));
        // END level_loading_async
    }

    // BEGIN level_loading_async_coroutine
    IEnumerator WaitForLoading(AsyncOperation operation)
    {
        
        // Wait for the scene load to reach at least 90%
        while (operation.progress < 0.9f) {
            yield return null;            
        }  
        
        // We're done!

        Debug.Log("Loading complete!");

        // Enabling scene activation will immediately start loading
        // the scene
        operation.allowSceneActivation = true;
    }
    // END level_loading_async_coroutine

    public void LoadLevelAdditive() {
        // BEGIN level_loading_additive
        // Load the scene in addition to the current one
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        // END level_loading_additive
    }

    public void UnloadLevel() {
        // BEGIN level_unloading
        // Unloading a scene is an async operation, much like loading can
        // be
        var unloadOperation = SceneManager.UnloadSceneAsync("Game");

        // If you want to run code after the unloading has completed, 
        // start a coroutine (again, just like loading)
        StartCoroutine(WaitForUnloading(unloadOperation));
        // END level_unloading
    }

    // BEGIN level_unloading_coroutine
    IEnumerator WaitForUnloading(AsyncOperation operation) {

        yield return new WaitUntil(() => operation.isDone);
    
        // Unloading has completed, and all objects that were in the
        // scene have been removed. However, Unity will not unload
        // the assets that those objects referred to, like the textures.
        // These will stay in memory for later use by other objects; if
        // you want to free up the memory, do this:

        Resources.UnloadUnusedAssets();

    }
    // END level_unloading_coroutine
}
