using UnityEngine;
using System.Collections;
using LitJson;

// SaveableBehaviour is a MonoBehaviour that automatically generates a
// unique SaveID when you save the scene in Unity. Subclass this class
// instead of MonoBehaviour, and you won't need to implement SaveID
// yourself.

// BEGIN saveable_behaviour
public abstract class SaveableBehaviour : MonoBehaviour, 
    ISaveable,                     // Marks this class as saveable
    ISerializationCallbackReceiver // Asks Unity to run code when the 
                                   // scene file is saved in the editor
{
    // This class doesn't implement SavedData or LoadFromData; that's the
    // job of our subclasses.
    public abstract JsonData SavedData { get; }
    public abstract void LoadFromData(JsonData data);

    // This class _does_ implement the SaveID property; it wraps the
    // _saveID field. (We need to this manually, rather than using
    // automatic property generation (i.e. "public string SaveID
    // {get;set;}"), because Unity won't store automatic properties when
    // saving the scene file. 
    public string SaveID
    {
        get
        {
            return _saveID;
        }
        set
        {
            _saveID = value;
        }
    }

    // The _saveID field stores the actual data that SaveID uses. We mark
    // it as serialized so that Unity editor saves it with the rest of the
    // scene, and as HideInInspector to make it not appear in the Inspector
    // (there's no reason for it to be edited.)
    [HideInInspector]
    [SerializeField]
    private string _saveID;

    // OnBeforeSerialize is called when Unity is about to save this object 
    // as part of a scene file.
    public void OnBeforeSerialize()
    {
        // Do we not currently have a Save ID?
        if (_saveID == null)
        {
            // Generate a new unique one, by creating a GUID and getting
            // its string value.
            _saveID = System.Guid.NewGuid().ToString();
        }

    }

    // OnAfterDeserialize is called when Unity has loaded this object as
    // part of a scene file.
    public void OnAfterDeserialize()
    {
        // Nothing special to do here, but the method must exist in order
        // to implement ISerializationCallbackReceiver
    }
}
// END saveable_behaviour