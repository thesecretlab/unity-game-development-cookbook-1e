using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotCapturing : MonoBehaviour
{
       
    // BEGIN persistent_data_path
    public string PathForFilename(string filename) {

        // Application.persistentDataPath contains a path where we can safely
        // store data
        var folderToStoreFilesIn = Application.persistentDataPath;

        // System.IO.Path.Combine combines two paths, using the current system's
        // directory separator ( \ on Windows, / on just about every 
        // other platform)
        var path = System.IO.Path.Combine(folderToStoreFilesIn, filename);

        return path;

    }
    // END persistent_data_path

    public void TakeScreenshot() {

        // BEGIN capture_screenshot_normal
        // Capturing a screenshot at current resolution
        ScreenCapture.CaptureScreenshot("MyScreenshot.png");
        // END capture_screenshot_normal

        // BEGIN capture_screenshot_double
        // Capturing a screenshot at double resolution (eg. if the screen
        // resolution is 1920x1080, the resulting file will be 3840x2160)
        ScreenCapture.CaptureScreenshot("MyScreenshotDoubleSized.png", 2);
        // END capture_screenshot_double

        // BEGIN capture_screenshot_texture
        // Capturing a screenshot as a texture
        // Note that this captures the screenshot immediately, and it may
        // not contain what you expect.
        Texture2D capturedTexture = ScreenCapture.CaptureScreenshotAsTexture();
        // END capture_screenshot_texture

        // BEGIN capture_screenshot_texture_coroutine_start
        // Begin the coroutine
        StartCoroutine(CaptureScreenshotAtEndOfFrame());
        // END capture_screenshot_texture_coroutine_start
    }

    // BEGIN capture_screenshot_texture_coroutine
    IEnumerator CaptureScreenshotAtEndOfFrame() {

        // Wait until the very end of the frame, after all rendering has
        // completed
        yield return new WaitForEndOfFrame();

        // Capture the screenshot
        Texture2D capturedTexture = ScreenCapture.CaptureScreenshotAsTexture();

        // capturedTexture now contains the contents of the screen, after
        // everything in the frame has rendered
    }
    // END capture_screenshot_texture_coroutine

    private void Awake()
    {
        TakeScreenshot();
    }
}