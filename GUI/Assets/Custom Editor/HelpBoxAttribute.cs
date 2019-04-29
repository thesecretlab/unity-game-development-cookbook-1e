using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN helpbox_attribute
#if UNITY_EDITOR
using UnityEditor;
#endif

// A HelpBoxAttribute attribute can be placed above a variable to make it
// display a help box above it in the inspector.

// Note how we define the HelpBoxAttribute class _outside_ the #if
// UNITY_EDITOR areas. This because code that refers to the HelpBox will be
// compiled outside of the editor context (that is, with UNITY_EDITOR not
// defined), and it will fail to compile if the class doesn't exist.

public class HelpBoxAttribute : PropertyAttribute
{
    // The text that will appear in the help box.
    public string text;
}

#if UNITY_EDITOR
// The code that draws the help box, as well as the original property.
[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
public class HelpBoxAttributePropertyDrawer : PropertyDrawer {
    public override void OnGUI(
        Rect position, SerializedProperty property, GUIContent label)
    {

        // Let's start by calculating the rectangle in which we'll draw the
        // help box.

        // 'position' is the rectangle that we've been given to draw
        // everything to do with this property. It's calculated by taking
        // the width of the Inspector tab, and the height returned by
        // GetPropertyHeight.

        // The help box will be at the top of the property, so we just take
        // the original position, and reduce the height.
        var helpBoxPosition = position;
        helpBoxPosition.height = HelpBoxHeight;

        // Next, we figure out the rectangle we need to draw the property
        // in.

        // We'll start with the entire available area...
        var propertyPosition = position;

        // Shift it down by the help box's height, plus line spacing
        propertyPosition.y += EditorGUIUtility.standardVerticalSpacing + 
            helpBoxPosition.height;
        
        // And update its height to be however tall the property wants to
        // be, including any child properties.
        propertyPosition.height = 
            EditorGUI.GetPropertyHeight(property, includeChildren: true);

        // Get the text from the HelpBoxAttribute.
        HelpBoxAttribute helpBox = (attribute as HelpBoxAttribute);
        string text = helpBox.text;

        // Draw the help box itself.
        EditorGUI.HelpBox(helpBoxPosition, text, MessageType.Info);

        // Draw the original property underneath.
        EditorGUI.PropertyField(
            propertyPosition, property, includeChildren: true);


    }

    public override float GetPropertyHeight(
        SerializedProperty property, GUIContent label)
    {
        // Calculate the height of the help box, given the editor width
        // (the text might wrap over multiple lines)

        float lineSpacing = EditorGUIUtility.standardVerticalSpacing;
        float propertyHeight = 
            EditorGUI.GetPropertyHeight(property, includeChildren: true);

        return HelpBoxHeight + lineSpacing + propertyHeight;

    }

    // Calculates the height of the help box.
    private float HelpBoxHeight
    {
        get
        {
            var width = EditorGUIUtility.currentViewWidth;
            var helpBoxAttribute = attribute as HelpBoxAttribute;
            var content = new GUIContent(helpBoxAttribute.text);
            float helpBoxHeight =
                EditorStyles.helpBox.CalcHeight(content, width);

            // Add a single line's height to ensure that text doesn't get 
            // clipped
            return helpBoxHeight + EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif 
// END helpbox_attribute