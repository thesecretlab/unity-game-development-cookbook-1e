using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;


// TransformSaver is an example of how to use SaveableBehaviour. This class
// stores information about the position, rotation and scale of the game
// object that it's attached to.

// To use this, add a TransformSaver component to your game object. It will
// automatically be included in saved games, and its state will be restored
// when the game is loaded.

// BEGIN saveable_transform_saver
public class TransformSaver : SaveableBehaviour
{
    // Store the keys we'll be including in the saved game as constants, to
    // avoid problems with typos.
    private const string LOCAL_POSITION_KEY = "localPosition";
    private const string LOCAL_ROTATION_KEY = "localRotation";
    private const string LOCAL_SCALE_KEY = "localScale";

    // SerializeValue is a helper function that converts an object that
    // Unity already knows how to serialize (like Vector3, Quaternion, and
    // others) into a JsonData that can be included in the saved game.
    private JsonData SerializeValue(object obj) {
        // This is very inefficient (we're converting an object to JSON
        // text, then immediately parsing this text back into a JSON
        // representation), but it means that we don't need to write the
        // (de)serialization code for built-in Unity types
        return JsonMapper.ToObject(JsonUtility.ToJson(obj));
    }

    // DeserializeValue works in reverse - given a JsonData, it produces a
    // value of the desired type, as long as that type is one that Unity
    // already knows how to serialize.
    private T DeserializeValue<T>(JsonData data) {
        return JsonUtility.FromJson<T>(data.ToJson());
    }

    // Provides the saved data for this component.
    public override JsonData SavedData {
        get {
            // Create the JsonData that we'll return to the saved game
            // system
            var result = new JsonData();

            // Store our position, rotation and scale
            result[LOCAL_POSITION_KEY] = 
                SerializeValue(transform.localPosition);

            result[LOCAL_ROTATION_KEY] = 
                SerializeValue(transform.localRotation);

            result[LOCAL_SCALE_KEY] = 
                SerializeValue(transform.localScale);

            return result;
        }
    }

    // Given some loaded data, updates the state of the component.
    public override void LoadFromData(JsonData data)
    {
        // We can't assume that the data will contain every piece of data
        // that we store; remember the programmer's adage, "be strict in
        // what you generate, and forgiving in what you accept".

        // Accordingly, we test to see if each item exists in the saved
        // data

        // Update position
        if (data.ContainsKey(LOCAL_POSITION_KEY))
        {
            transform.localPosition =
                DeserializeValue<Vector3>(data[LOCAL_POSITION_KEY]);
        }

        // Update rotation
        if (data.ContainsKey(LOCAL_ROTATION_KEY))
        {
            transform.localRotation =
                DeserializeValue<Quaternion>(data[LOCAL_ROTATION_KEY]);
        }

        // Update scale
        if (data.ContainsKey(LOCAL_SCALE_KEY))
        {
            transform.localScale =
                DeserializeValue<Vector3>(data[LOCAL_SCALE_KEY]);
        }
    }
}
// END saveable_transform_saver