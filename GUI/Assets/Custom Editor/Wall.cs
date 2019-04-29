using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bring in the UnityEditor namespace, if this file is being compiled for
// the editor. (Code between the #if and #endif won't be included in the
// final game; it will only be available in the editor.)

// BEGIN wall4
#if UNITY_EDITOR
using UnityEditor;
#endif
// END wall4

// A wall.
public class Wall : MonoBehaviour
{
    // BEGIN wall1
    [SerializeField] public int rows = 5;
    [SerializeField] public int columns = 5;

    [SerializeField] public Renderer brickPrefab;
    // END wall1
}

// BEGIN wall2
#if UNITY_EDITOR
// The Editor object that will manage the Inspector for Wall components.
[CustomEditor(typeof(Wall))]
public class WallEditor : Editor {
// END wall2
    
    // Called by Unity to display the contents of the Inspector for this
    // object.

    // BEGIN wall5
    public override void OnInspectorGUI()
    {
        // Make sure that we have the latest data stored in the 
        serializedObject.Update();

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("rows"));

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("columns"));

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("brickPrefab"));

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Create Wall")) {
            CreateWall();
        }
    }
    // END wall5

    void CreateWall() {
        // Register the state of this object before we make changes to its
        // contents

        // BEGIN wall6
        Undo.RegisterFullObjectHierarchyUndo(target, "Create Wall");
        // END wall6

        // BEGIN wall7
        var wall = target as Wall;

        if (wall == null) {
            return;
        }
        // END wall7

        // Temporarily store all current children
        // BEGIN wall8
        GameObject[] allChildren = 
            new GameObject[wall.transform.childCount];

        int i = 0;
        // END wall8

        // We can't call DestroyImmediate on the objects in a list that
        // we're iterating over, because doing that would change the size
        // of the list as we're iterating over it. Instead, we copy
        // references to them into an array of fixed size, and then destroy
        // that.

        // Find all child objects, and temporarily put them in the array

        // BEGIN wall9
        foreach (Transform child in wall.transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }
        // END wall9

        // We can now iterate over that array and destroy them
        // BEGIN wall10
        foreach (GameObject child in allChildren)
        {
            // Destroy the object, and also record it as an undo-able
            // action
            DestroyImmediate(child.gameObject);
        }
        // END wall10

        // We can now replace them with new objects
        // BEGIN wall11
        var brickSize = 
            wall.brickPrefab.GetComponent<Renderer>().bounds.size;
        // END wall11

        // BEGIN wall12
        for (int row = 0; row < wall.rows; row++) {

            // Figure out where the row should be
            var rowPosition = Vector3.zero;
            rowPosition.y += brickSize.y * row;

            for (int column = 0; column < wall.columns; column++)
            {
                // Figure out where the brick should be
                var columnPosition = rowPosition;
                columnPosition.x += brickSize.x * column;

                // Every second row is offset a bit
                if (row % 2 == 0) {
                    columnPosition.x += brickSize.x / 2f;
                }

                // PrefabUtility.InstantiatePrefab is like Instantiate, but
                // it remembers that it was a prefab, and maintains the
                // connection. (We have to cast it to GameObject because
                // there's no generic version of InstantiatePrefab - the
                // compiler won't figure out the type automatically based
                // on the type that was passed in.

                var brick = PrefabUtility
                    .InstantiatePrefab(wall.brickPrefab.gameObject) 
                    as GameObject;

                // Give it a name appropriate to its position
                brick.name = string.Format("{0} ({1},{2})",
                                           wall.brickPrefab.name, 
                                           column, 
                                           row);

                // Place it in the scene
                brick.transform.SetParent(wall.transform, false);

                // Update its position, relative to its parent
                brick.transform.localPosition = columnPosition;

                // Don't rotate it, relative to its parent
                brick.transform.localRotation = Quaternion.identity;

            }
        }
        // END wall12
    }
// BEGIN wall3
}
#endif 
// END wall3
