using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN cube_importer
#if UNITY_EDITOR
using UnityEditor;

// At the time of writing, asset importers are in the Experiemental module.
// By the time you're reading this, the API may have changed, so check the
// documentation.
using UnityEditor.Experimental.AssetImporters;

// A CubeDescription contains the variables that define our cubes. We'll create
// them by loading them from text files that contain JSON.
public struct CubeDescription {
    public Vector3 size;

    // storing the r, g, b, a values in this 4-component vector
    public Color color;
}

// Indicate to Unity that this script imports files with the file extension
// ".cube", and that this is version 0 of the importer (changing the number will
// make Unity re-import assets of this type)
[ScriptedImporter(0, "cube")]
public class CubeImporter : ScriptedImporter {

    // Called by Unity to perform an import
    public override void OnImportAsset(AssetImportContext ctx)
    {

        // "ctx" contains information about the import that Unity wants us to
        // do; it contains the path to the file, and we'll put the Unity
        // objects into it when we're done

        // "cube" files will contain JSON that describes the color and size
        // of the cube.

        // Create a variable to load the cube description into
        CubeDescription cubeDescription;
            
        // Attempt to load the JSON.
        try {
            var text = System.IO.File.ReadAllText(ctx.assetPath);

            cubeDescription = JsonUtility.FromJson<CubeDescription>(text);
        } catch (System.ArgumentException e) {
            // We failed to load the JSON. Maybe it's not valid. Report the error.
            Debug.LogErrorFormat("{0} is not a valid cube: {1}", ctx.assetPath, e.Message);
            return;
        } catch (System.Exception e) {
            // We caught some other kind of exception, and can't continue. Re-throw
            // the error.
            throw e;
        }

        // Create a generic cube object, which we'll make changes to and save
        // as a new asset.
        var cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Get the last part of the file path, and use it as the cube's name
        string name = System.IO.Path.GetFileNameWithoutExtension(ctx.assetPath);

        // Next, we'll create a cube that's the right size. The default cube mesh
        // is 1x1x1; we'll scale it based on the size that was passed in.

        // Copy the default cube mesh.
        var cubeMesh = Instantiate(cubeObject.GetComponent<MeshFilter>().sharedMesh);

        // Create a matrix that scales vertices by the given X, Y and Z amounts.
        var scaleMatrix = Matrix4x4.Scale(cubeDescription.size);

        // Get a copy of the vertices in the mesh.
        var vertices = cubeMesh.vertices;

        // For each of these vertices, apply the scale by multiplying the matrix
        // against the vertex.
        for (int v = 0; v < vertices.Length; v++) {
            vertices[v] = scaleMatrix.MultiplyPoint(vertices[v]);
        }

        // Store these scaled vertices in the mesh.
        cubeMesh.vertices = vertices;

        // Tell the cube's MeshFilter to use this new mesh.
        cubeObject.GetComponent<MeshFilter>().sharedMesh = cubeMesh;

        // Give the mesh a name.
        cubeMesh.name = name + " Mesh";

        // Create a new material, using the Standard shader (which is the 
        // default)
        var cubeMaterial = new Material(Shader.Find("Standard"));

        // Apply the color that we loaded.
        cubeMaterial.color = cubeDescription.color;

        // Give it a name, too.
        cubeMaterial.name = name + " Material";

        // Tell the cube's MeshRenderer to use this material.
        cubeObject.GetComponent<MeshRenderer>().material = cubeMaterial;

        // Now we store the objects we just created as assets.

        // First, store the GameObject (the collection of components that uses
        // and renders the mesh and material), and mark it as the "main" object.
        ctx.AddObjectToAsset(name, cubeObject);
        ctx.SetMainObject(cubeObject);

        // We also need to store the mesh and material as well.
        ctx.AddObjectToAsset(cubeMaterial.name, cubeMaterial);
        ctx.AddObjectToAsset(cubeMesh.name, cubeMesh);

    }

}
#endif
// END cube_importer
