using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN texture_loading_using
using System.IO;
// END texture_loading_using

public class TextureLoading : MonoBehaviour
{

    // BEGIN texture_loading
    // Loads a texture file from disk
    public Texture2D LoadTexture(string fileName) {
        // Create the path to this file

        // Application.dataPath is the path to where the built application 
        // is; in the editor, it is in the project's root folder (the one 
        // that contains the Assets folder)
        var imagePath = Path.Combine(
            Application.dataPath,
            fileName
        );

        // Double-check that a file exists
        if (File.Exists(imagePath) == false) {
            // Warn that there's no file there and give up
            Debug.LogWarningFormat("No file exists at path {0}", 
                                   imagePath);
            return null;
        }

        // Load the file data.
        var fileData = File.ReadAllBytes(imagePath);

        // Create a new texture. When you create any texture, you 
        // specify the width and height; however, when you load a texture, 
        // it will automatically resize itself
        var tex = new Texture2D(2, 2);

        // Upload the image data. Unity will decompress it from PNG 
        // into raw pixels.
        var success = tex.LoadImage(fileData);

        if (success) {
            // We now have a texture!
            return tex;
        } else {
            // Warn that we can't read this file
            Debug.LogWarningFormat("Failed to load texture at path {0}",
                                   imagePath);
                                   
            return null;
        }
        
    }
    // END texture_loading

    // Start is called before the first frame update
    void Start()
    {
        
        // BEGIN texture_loading_usage
        // Load the texture from disk
        var tex = LoadTexture("ImageToLoad.png");

        // Check that we got it before we try to use it
        if (tex == null) {
            return;
        }

        // We can now use this texture just like any other!
        
        // For example, here's how we can set the main texture of the
        // renderer that this script is on the same object as
        GetComponent<Renderer>().material.mainTexture = tex;
        // END texture_loading_usage


    }
}
