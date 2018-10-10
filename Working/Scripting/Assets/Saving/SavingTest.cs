using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SavingTest : MonoBehaviour
{
    public void Save() {
        // BEGIN saving_test
        // Save the game to a file called "SaveGame.json"
        SavingService.SaveGame("SaveGame.json");
        // END saving_test
    }

    public void Load() {
        // BEGIN loading_test
        // Try to load the game from a file called "SaveGame.json"
        SavingService.LoadGame("SaveGame.json");
        // END loading_test
    }

    public void LoadExtraScene() {
        SceneManager.LoadScene("SavingDemoExtraScene", LoadSceneMode.Additive);
    }
}
