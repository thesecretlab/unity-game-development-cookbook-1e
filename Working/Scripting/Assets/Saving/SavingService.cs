using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We use LitJson to do reading and writing JSON data

// We use System.IO to handle the file input and output

// We use Linq to simplify the task of finding all saveable scripts in the 
// scene

// Using Linq allocates memory, which means that the garbage collector will 
// have to run at some point in the future. This causes hitches, and should
// normally be avoided. However, saving the game happens comparitively rarely,
// and is expected to be a moderately heavy operation anyway (that is, players
// expect the game to pause for a moment.) Additionally, by calling 
// System.GC.Collect() when we're done, we do much of the cleanup that would
// otherwise happen at some unpredictable amount of time in the future.

// Loading the game means loading a new scene, so we use classes in
// UnityEngine.SceneManagement to handle this


// BEGIN saving_service_using
using LitJson;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
// END saving_service_using

// BEGIN saving_service_isaveable
// Any MonoBehaviour that implements the ISaveable interface will be saved
// in the scene, and loaded back
public interface ISaveable {

    // The Save ID is a unique string that identifies a component in the
    // save data. It's used for finding that object again when the game is
    // loaded.
    string SaveID { get; }

    // The SavedData is the content that will be written to disk. It's asked for
    // when the game is saved.
    JsonData SavedData { get; }

    // LoadFromData is called when the game is being loaded. The object is 
    // provided with the data that was read, and is expected to use that 
    // information to restore its previous state.
    void LoadFromData(JsonData data);
}
// END saving_service_isaveable

// The SavingService is a static class that provides methods for saving 
// and loading the game. You don't create instances of this class; instead,
// you just call its SaveGame and LoadGame methods directy.

// BEGIN saving_service_class
public static class SavingService
{
    // BEGIN saving_service_keys
    // To avoid problems caused by typos, we'll store the names of the 
    // strings we use to store and look up items in the JSON as constant strings
    private const string ACTIVE_SCENE_KEY = "activeScene";
    private const string SCENES_KEY = "scenes";
    private const string OBJECTS_KEY = "objects";
    // use an unexpected character '$' here to reduce the chance of collisions
    private const string SAVEID_KEY = "$saveID";
    // END saving_service_keys

    // BEGIN saving_service_savegame
    // Saves the game, and writes it to a file called fileName in the 
    // app's persistent data directory.
    public static void SaveGame(string fileName) {

        // Create the JsonData that we will eventually write to disk
        var result = new JsonData();

        // Find all MonoBehaviours by first finding every single MonoBehaviour,
        // and filtering it to only include those that are ISaveable.
        var allSaveableObjects = Object
            .FindObjectsOfType<MonoBehaviour>()
            .OfType<ISaveable>();

        // Do we have any objects to save?
        if (allSaveableObjects.Count() > 0) {

            // Create the JsonData that will store the list of objects
            var savedObjects = new JsonData();

            // Iterate over every object we want to save
            foreach (var saveableObject in allSaveableObjects)
            {
                // Get the object's saved data
                var data = saveableObject.SavedData;

                // We expect this to be an object (JSON's term for a dictionary)
                // because we need to include the object's Save ID
                if (data.IsObject) {

                    // Record the Save ID for this object
                    data[SAVEID_KEY] = saveableObject.SaveID;

                    // Add the object's save data to the collection
                    savedObjects.Add(data);
                } else {
                    // Provide a helpful warning that we can't save this object.
                    var behaviour = saveableObject as MonoBehaviour;

                    Debug.LogWarningFormat(
                        behaviour,
                        "{0}'s save data is not a dictionary. The object was " +
                        "not saved.",
                        behaviour.name
                    );
                }
            }

            // Store the collection of saved objects in the result.
            result[OBJECTS_KEY] = savedObjects;
        } else {
            // We have no objects to save. Give a nice warning.
            Debug.LogWarningFormat("The scene did not include any saveable " +
                                   "objects.");
        }

        // Next, we need to record what scenes are open. Unity lets you have
        // multiple scenes open at the same time, so we need to store all of
        // them, as well as which scene is the 'active' scene (the scene that
        // new objects are added to, and which controls the lighting settings
        // for the game.)

        // Create a JsonData that will store the list of open scenes.
        var openScenes = new JsonData();

        // Ask the scene manager how many scenes are open, and for each one,
        // store the scene's name.
        var sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++) {
            var scene = SceneManager.GetSceneAt(i);

            openScenes.Add(scene.name);
        }

        // Store the list of open scenes
        result[SCENES_KEY] = openScenes;

        // Store the name of the active scene
        result[ACTIVE_SCENE_KEY] = SceneManager.GetActiveScene().name;

        // We've now finished generating the save data, and it's time to
        // write it to disk.

        // Figure out where to put the file by combining the persistent data
        // path with the filename that this method received as a parameter.
        var outputPath = Path.Combine(
            Application.persistentDataPath, fileName);

        // Create a JsonWriter, and configure it to 'pretty-print' the data.
        // This is optional (you could just call result.ToJson() with no
        // JsonWriter parameter and receive a string), but this way the 
        // resulting JSON is easier to read and understand, which is helpful
        // while developing.
        var writer = new JsonWriter();
        writer.PrettyPrint = true;

        // Convert the save data to JSON text.
        result.ToJson(writer);

        // Write the JSON text to disk.
        File.WriteAllText(outputPath, writer.ToString());

        // Notify where to find the saved game
        Debug.LogFormat("Wrote saved game to {0}", outputPath);


        // We allocated a lot of memory here, which means that there's an 
        // increased chance of the garbage collector needing to run in the 
        // future. To tidy up, we'll release our reference to the saved data,
        // and then ask the garbage collector to run immediately. This will 
        // result in a slight performance hitch as the collector runs, but 
        // that's fine for this case, since users expect saving the game to
        // pause for a second.
        result = null;
        System.GC.Collect();
    }
    // END saving_service_savegame

    // BEGIN saving_service_loadgame
    // Loads the game from a given file, and restores its state.
    public static bool LoadGame(string fileName) {

        // Figure out where to find the file.
        var dataPath = Path.Combine(
            Application.persistentDataPath, fileName);

        // Ensure that a file actually exists there.
        if (File.Exists(dataPath) == false) {
            Debug.LogErrorFormat("No file exists at {0}", dataPath);
            return false;
        }

        // Read the data as Json.
        var text = File.ReadAllText(dataPath);

        var data = JsonMapper.ToObject(text);

        // Ensure that we successfully read the data, and that it's an object
        // (i.e. a JSON dictionary)
        if (data == null || data.IsObject == false) {
            Debug.LogErrorFormat("Data at {0} is not a JSON object", dataPath);
            return false;
        }

        // We need to know what scenes to load.
        if (!data.ContainsKey("scenes"))
        {
            Debug.LogWarningFormat(
                "Data at {0} does not contain any scenes; not loading any!",
                dataPath
            );
            return false;
        }

        // Get the list of scenes
        var scenes = data[SCENES_KEY];

        int sceneCount = scenes.Count;

        if (sceneCount == 0)
        {
            Debug.LogWarningFormat(
                "Data at {0} doesn't specify any scenes to load.",
                dataPath
            );
            return false;
        }

        // Load each specified scene.
        for (int i = 0; i < sceneCount; i++)
        {
            var scene = (string)scenes[i];

            // If this is the first scene we're loading, load it and replace
            // every other active scene.
            if (i == 0)
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
            else
            {
                // Otherwise, load that scene on top of the existing ones.
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            }

        }

        // Find the active scene, and set it
        if (data.ContainsKey(ACTIVE_SCENE_KEY))
        {
            var activeSceneName = (string)data[ACTIVE_SCENE_KEY];
            var activeScene = SceneManager.GetSceneByName(activeSceneName);

            if (activeScene.IsValid() == false)
            {
                Debug.LogErrorFormat(
                 "Data at {0} specifies an active scene that doesn't exist. " +
                    "Stopping loading here.",
                 dataPath
                );
                return false;
            }

            SceneManager.SetActiveScene(activeScene);
        } else {
            // This is not an error, since the first scene in the list will
            // be treated as active, but it's worth warning about.
            Debug.LogWarningFormat("Data at {0} does not specify an active " +
                                   "scene.");
        }

        // Find all objects in the scene and load them
        if (data.ContainsKey(OBJECTS_KEY)) {
            var objects = data[OBJECTS_KEY];

            // We cannot update the state of the objects right away, because
            // Unity will not complete loading the scene until some time in
            // the future. Any changes we made to the objects would be reverted
            // back to how they're defined in the original scene. As a result,
            // we need to run the code after the scene manager reports that 
            // a scene has finished loading.

            // To do this, we create a new delegate that contains our object-
            // loading code, and store that in LoadObjectsAfterSceneLoad.
            // This delegate is added to the SceneManager's sceneLoaded
            // event, which makes it run after the scene has finished loading.

            LoadObjectsAfterSceneLoad = (scene, loadSceneMode) => {

                // Find all ISaveable objects, and build a dictionary that maps 
                // their  Save IDs to the object (so that we can quickly look
                // them up)
                var allLoadableObjects = Object
                    .FindObjectsOfType<MonoBehaviour>()
                    .OfType<ISaveable>()
                    .ToDictionary(o => o.SaveID, o => o);

                // Get the collection of objects we need to load
                var objectsCount = objects.Count;

                // For each item in the list...
                for (int i = 0; i < objectsCount; i++)
                {
                    // Get the saved data
                    var objectData = objects[i];

                    // Get the Save ID from that data
                    var saveID = (string)objectData[SAVEID_KEY];

                    // Attempt to find the object in the scene(s) that has that
                    // Save ID
                    if (allLoadableObjects.ContainsKey(saveID))
                    {
                        var loadableObject = allLoadableObjects[saveID];

                        // Ask the object to load from this data
                        loadableObject.LoadFromData(objectData);
                    }
                }

                // Tidy up after ourselves; remove this delegate from the
                // sceneLoaded event so that it isn't called next time
                SceneManager.sceneLoaded -= LoadObjectsAfterSceneLoad;

                // Release the reference to the delegate
                LoadObjectsAfterSceneLoad = null;

                // And ask the garbage collector to tidy up (again, this will
                // cause a performance hitch, but users are fine with this since
                // they're already waiting for the scene to finish loading)
                System.GC.Collect();
            };

            // Register the object-loading code to run after the scene loads.
            SceneManager.sceneLoaded += LoadObjectsAfterSceneLoad;
        }



        return true;
    }
    // END saving_service_loadgame

    // BEGIN saving_service_loadobjects_delegate
    // A reference to the delegate that runs after the scene loads, which 
    // performs the object state restoration.
    static UnityEngine.Events.UnityAction<Scene, LoadSceneMode> 
        LoadObjectsAfterSceneLoad;
    // END saving_service_loadobjects_delegate

}
// END saving_service_class