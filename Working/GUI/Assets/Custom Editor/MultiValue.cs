using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN multivalue1
#if UNITY_EDITOR
using UnityEditor;
#endif
// END multivalue1

// BEGIN multivalue2
[System.Serializable]
public class MultiValue {

    // The index of the currently selected value.
    [SerializeField] int _selectedIndex = 0;

    // The list of available options
    [SerializeField] string[] options;

    // Manages the selected index, and keeps it from going out of bounds.
    public int SelectedIndex {
        get {
            return _selectedIndex;
        }
        set {
            value = Mathf.Clamp(value, 0, options.Length);
        }
    }

    // Creates a new chooser, using the specified values.
    public MultiValue(params string[] values) {
        this.options = values;
    }

    // Returns the text of the currently selected value.
    public string SelectedValue {
        get {
            if (options.Length > 0) {
                return options[_selectedIndex];
            } else {
                return null;
            }
        }
    }
}
// END multivalue2

// BEGIN multivalue_editor
#if UNITY_EDITOR
// Overrides how Unity will draw a MultiValueproperty.
[CustomPropertyDrawer(typeof(MultiValue))]
public class MultiValuePropertyDrawer : PropertyDrawer {

    // Called by Unity when it needs to draw a MultiValue property in the
    // Inspector.
    public override void OnGUI(Rect position, SerializedProperty property,
                               GUIContent label)
    {
        // Ensure that the controls found in the GUI class behave properly. This
        // also tells Unity that any edit to any field in here should be recorded
        // for the purposes of Undoing them.
        EditorGUI.BeginProperty(position, label, property);

        // Get a reference to the variables that store the info we need
        var indexProperty = property.FindPropertyRelative("_selectedIndex");
        var valuesProperty = property.FindPropertyRelative("options");

        // Calculate the rectangle to draw the first line in. This will hold
        // our Toolbar (our list of buttons).
        var firstLinePosition = position;
        firstLinePosition.height = EditorGUI.GetPropertyHeight(indexProperty);

        // Use this to calculate the rectangle to draw the second property in.
        // (This will vary, depending on whether the user has elected to expand
        // the list in the Inspector or not.)
        var secondLinePosition = firstLinePosition;
        secondLinePosition.y += 2 + firstLinePosition.height;
        secondLinePosition.height = EditorGUI.GetPropertyHeight(valuesProperty);

        // Display the label in front of the toolbar, and get back a new rectangle
        // to draw the toolbar in.
        firstLinePosition = EditorGUI.PrefixLabel(
            firstLinePosition, new GUIContent(property.displayName));

        // Get every string inside the "options" property, as an array
        string[] labels = new string[valuesProperty.arraySize];

        for (int i = 0; i < labels.Length; i++) {
            labels[i] = valuesProperty.GetArrayElementAtIndex(i).stringValue;
        }

        // Because Toolbar is not in the EditorGUI class, it won't automatically
        // report to the editor that it was updated in a way that the editor
        // can track for the purposes of the Undo system. So, we use 
        // BeginChangeCheck before drawing the toolbar, and call EndChangeCheck.
        // If EndChangeCheck returns true, the user made a change.
        EditorGUI.BeginChangeCheck();
        var index = indexProperty.intValue;
        var newValue = GUI.Toolbar(firstLinePosition, index, labels);
        if (EditorGUI.EndChangeCheck()) {
            // The toolbar was changed.
            indexProperty.intValue = newValue;
        }

        // Draw the 'options' list as a regular list. This will also draw things
        // like the expand arrow, the items in the list, and the number of items
        // in the list.
        EditorGUI.indentLevel += 1;
        EditorGUI.PropertyField(secondLinePosition, valuesProperty, true);
        EditorGUI.indentLevel -= 1;

        // We're done editing this property.
        EditorGUI.EndProperty();
    }

    // Called by Unity to determine the height of the MultiValue property.
    public override float GetPropertyHeight(SerializedProperty property, 
                                            GUIContent label)
    {

        // The height of a MultiValue property is the height of both of its two
        // child properties, plus the spacing between them.

        float lineSpacing = EditorGUIUtility.standardVerticalSpacing;

        // Get the child properties 
        var indexProperty = property.FindPropertyRelative("_selectedIndex");
        var valuesProperty = property.FindPropertyRelative("options");

        // Calculate the height of this property by getting the height of both
        // properties (including the strings inside the options, if it's been
        // expanded), plus the line spacing
        float indexHeight = EditorGUI.GetPropertyHeight(indexProperty);
        float optionsHeight = EditorGUI.GetPropertyHeight(valuesProperty, true);

        return indexHeight + lineSpacing + optionsHeight;
    }

}

#endif 
// END multivalue_editor