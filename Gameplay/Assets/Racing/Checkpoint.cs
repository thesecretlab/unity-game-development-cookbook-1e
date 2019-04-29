using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN checkpoint
#if UNITY_EDITOR
// Include the UnityEditor namespace when this class is being used in the
// Editor.
using UnityEditor;
#endif

public class Checkpoint : MonoBehaviour
{
    
    // If true, this is the start of the circuit
    [SerializeField] public bool isLapStart;

    // The next checkpoint in the circuit. If we're visiting these in
    // reverse, or skipping too many, then we're going the wrong way.
    [SerializeField] public Checkpoint next;
    
    // The index number, used by LapTracker to figure out if we're going
    // the wrong way.
    internal int index = 0;
    
    // Checkpoints are invisible, so we draw a marker in the scene view to
    // make it easier to visualise.
    private void OnDrawGizmos()
    {
        // Draw the markers as a blue sphere, except for the lap start,
        // which is yellow.
        if (isLapStart)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.blue;
        }

        Gizmos.DrawSphere(transform.position, 0.5f);
        
        // If we have a next node set up, draw a blue line to it.
        if (next != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, next.transform.position);
        }
    }
}

#if UNITY_EDITOR
// Code that adds additional controls for building a track to the
// Checkpoint inspector.
[CustomEditor(typeof(Checkpoint))]
public class CheckpointEditor : Editor {

    // Called when Unity needs to display the Inspector for this Checkpoint
    // component.
    public override void OnInspectorGUI()
    {
        // First, draw the Inspector contents that we'd normally get.
        DrawDefaultInspector();

        // Get a reference to the Checkpoint component we're editing, by
        // casting 'target' (which is just an Object) to Checkpoint.
        var checkpoint = this.target as Checkpoint;

        // Display a button that inserts a new checkpoint between us and
        // the next one. GUILayout.Button both displays the button, and
        // returns true if it was clicked.
        if (GUILayout.Button("Insert Checkpoint")) {

            // Make a new object, and add a Checkpoint component to it
            var newCheckpoint = new GameObject("Checkpoint")
                .AddComponent<Checkpoint>();

            // Make it point to our next one, and make ourself point to it
            // (in other words, insert it between us and our next
            // checkpoint)
            newCheckpoint.next = checkpoint.next;
            checkpoint.next = newCheckpoint;

            // Make it one of our siblings
            newCheckpoint.transform
                .SetParent(checkpoint.transform.parent, true);

            // Position it as our next sibling in the hierarchy.
            // Not technically needed, and doesn't affect the game at all,
            // but it looks nicer.
            var nextSiblingIndex = 
                checkpoint.transform.GetSiblingIndex() + 1;

            newCheckpoint.transform.SetSiblingIndex(nextSiblingIndex);

            // Move it slightly so that it's visibly not the same one
            newCheckpoint.transform.position = 
                checkpoint.transform.position + new Vector3(1, 0, 0);

            // Select it, so that we can immediately start moving it
            Selection.activeGameObject = newCheckpoint.gameObject;
        }

        // Disable this button if we don't have a next checkpoint, or if
        // the next checkpoint is the lap start

        var disableRemoveButton = checkpoint.next == null || 
                                            checkpoint.next.isLapStart;

        using (new EditorGUI.DisabledGroupScope(disableRemoveButton)) {
            // Display a button that removes the next checkpoint
            if (GUILayout.Button("Remove Next Checkpoint"))
            {
                // Get the node that this next checkpoint was linking to
                var next = checkpoint.next.next;

                // Remove the next one
                DestroyImmediate(checkpoint.next.gameObject);

                // Aim ourselves at what it was pointing at
                checkpoint.next = next;
            }
        }              
    }    
}
#endif
// END checkpoint