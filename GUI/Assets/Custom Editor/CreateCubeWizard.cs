using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN cube_wizard
#if UNITY_EDITOR
// This entire class only exists in the Editor. It doesn't need to be
// included in built games.
using UnityEditor;

// Create a wizard that generates a new cube, as well as a colour.
public class CreateCubeWizard : ScriptableWizard {

    // Create a new entry in the GameObject menu, called "Cube with Color".
    // When it's selected, a CreateCubeWizard will appear. Note that this
    // method must be both public and static for MenuItem to work.
    [MenuItem("GameObject/Cube with Color")]
    public static void CreateWizard() {

        // Create and display the wizard.
        DisplayWizard<CreateCubeWizard>("Create Cube");
    }

    // Stores temporary information about the cube that the user wants to
    // make. These variables are drawn in the window, just like variables
    // in a MonoBehaviour component are.
    [SerializeField] Vector3 size = Vector3.zero;
    [SerializeField] Color color = Color.white;

    // Run when the Create button is clicked.
    private void OnWizardCreate()
    {

        // Create a cube
        var newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Scale it
        newCube.transform.localScale = size;

        // Create a new material, using the Standard shader (which is the 
        // default)
        var tintedMaterial = new Material(Shader.Find("Standard"));

        // Give it a colour
        tintedMaterial.color = color;

        // Materials need to be saved to disk. To do this, we need to
        // figure out where we can save the file. GenerateUniqueAssetPath
        // will give us a path that's guaranteed to not already have a file
        // present.
        var desiredPath = 
            AssetDatabase.GenerateUniqueAssetPath("Assets/Tinted.mat");

        // Create and save the new asset.
        AssetDatabase.CreateAsset(tintedMaterial, desiredPath);
        AssetDatabase.SaveAssets();

        // Visually "ping" the asset, as though we'd selected it in the
        // Editor. This will show the user that a new file has been
        // created, and where to find it.
        EditorGUIUtility.PingObject(tintedMaterial);

        // Finally, make the new cube use this new material.
        newCube.GetComponent<MeshRenderer>().material = tintedMaterial;

    }
}
#endif
// END cube_wizard