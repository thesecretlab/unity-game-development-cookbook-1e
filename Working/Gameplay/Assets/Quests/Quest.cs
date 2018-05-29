using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN quest
#if UNITY_EDITOR
using UnityEditor;
#endif

// A Quest stores information about a quest: its name, and its objectives.

// CreateAssetMenu makes the Create Asset menu contain an entry that creates
// a new Quest asset.
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 100)]
public class Quest : ScriptableObject
{

    // Represents the status of objectives and quests
    public enum Status {
        NotYetComplete, // the objective or quest has not yet been completed
        Complete, // the objective or quest has been successfully completed
        Failed // the objective or quest has failed
    }

    // The name of the quest
    public string questName;

    // The list of objectives that form this quest
    public List<Objective> objectives;

    // Objectives are the specific tasks that make up a quest.
    [System.Serializable]
    public class Objective
    {
        // The visible name that's shown the player.
        public string name = "New Objective";

        // If true, this objective doesn't need to be complete in order for
        // the quest to be considered complete.
        public bool optional = false;

        // If false, the objective will not be shown to the user if it's not
        // yet complete. (It will be shown if it's Complete or Failed.)
        public bool visible = true;

        // The status of the objective when the quest begins. Usually this will
        // be "not yet complete", but you might want an objective that starts
        // as Complete, and can be Failed.
        public Status initalStatus = Status.NotYetComplete;
    }

}

#if UNITY_EDITOR
// Draw a custom editor that lets you build the list of objectives.
[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor {

    // Called when Unity wants to draw the Inspector for a Quest.
    public override void OnInspectorGUI()
    {

        // Ensure that the object we're displaying has had any pending changes 
        // done.
        serializedObject.Update();

        // Draw the name of the quest.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("questName"), new GUIContent("Name"));

        // Draw a header for the list of objectives
        EditorGUILayout.LabelField("Objectives");

        // Get the property that contains the list of objectives
        var objectiveList = serializedObject.FindProperty("objectives");

        // Indent the objectives
        EditorGUI.indentLevel += 1;

        // For each objective in the list, draw an entry
        for (int i = 0; i < objectiveList.arraySize; i++)
        {
            // Draw a single line of controls
            EditorGUILayout.BeginHorizontal();

            // Draw the objective itself (its name, and its flags)
            EditorGUILayout.PropertyField(
                objectiveList.GetArrayElementAtIndex(i), 
                includeChildren: true
            );

            // Draw a button that moves the item up in the list
            if (GUILayout.Button("Up", EditorStyles.miniButtonLeft, 
                                 GUILayout.Width(25)))
            {
                objectiveList.MoveArrayElement(i, i - 1);
            }

            // Draw a button that moves the item down in the list
            if (GUILayout.Button("Down", EditorStyles.miniButtonMid, 
                                 GUILayout.Width(40)))
            {
                objectiveList.MoveArrayElement(i, i + 1);
            }

            // Draw a button that removes the item from the list
            if (GUILayout.Button("-", EditorStyles.miniButtonRight, 
                                 GUILayout.Width(25)))
            {
                objectiveList.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndHorizontal();

        }

        // Remove the indentation
        EditorGUI.indentLevel -= 1;

        // Draw a button at adds a new objective to the list
        if (GUILayout.Button("Add Objective"))
        {
            objectiveList.arraySize += 1;
        }

        // Save any changes
        serializedObject.ApplyModifiedProperties();

    }
}

#endif
// END quest

