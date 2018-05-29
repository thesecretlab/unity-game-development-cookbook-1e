using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN quest_manager
// Represents the player's current progress through a quest.
public class QuestStatus {

    // The underlying data object that describes the quest.
    public Quest questData;

    // The map of objective identifiers.
    public Dictionary<int, Quest.Status> objectiveStatuses;

    // The constructor. Pass a Quest to this to set it up.
    public QuestStatus(Quest questData)
    {
        // Store the quest info
        this.questData = questData;

        // Create the map of objective numbers to their status
        objectiveStatuses = new Dictionary<int, Quest.Status>();

        for (int i = 0; i < questData.objectives.Count; i += 1)
        {
            var objectiveData = questData.objectives[i];

            objectiveStatuses[i] = objectiveData.initalStatus;
        }
    }

    // Returns the state of the entire quest.
    // If all non-optional objectives are complete, the quest is complete.
    // If any non-optional objective is failed, the quest is failed.
    // Otherwise, the quest is not yet complete.
    public Quest.Status questStatus {
        get {
            
            for (int i = 0; i < questData.objectives.Count; i += 1) {
                
                var objectiveData = questData.objectives[i];

                // Optional objectives do not matter to the overall quest status
                if (objectiveData.optional)
                    continue;

                var objectiveStatus = objectiveStatuses[i];

                // this is a mandatory objective
                if (objectiveStatus == Quest.Status.Failed)
                {
                    // if a mandatory objective is failed, the whole 
                    // quest is failed
                    return Quest.Status.Failed;
                }
                else if (objectiveStatus != Quest.Status.Complete)
                {
                    // if a mandatory objective is not yet complete,
                    // the whole quest is not yet complete
                    return Quest.Status.NotYetComplete;
                }
            }

            // All mandatory objectives are complete, so this quest is complete
            return Quest.Status.Complete;

        }
    }

    // Returns a string containing the list of objectives, their statuses, and
    // the status of the quest.
    public override string ToString()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (int i = 0; i < questData.objectives.Count; i += 1)
        {
            // Get the objective and its status
            var objectiveData = questData.objectives[i];
            var objectiveStatus = objectiveStatuses[i];

            // Don't show hidden objectives that haven't been finished
            if (objectiveData.visible == false &&
                objectiveStatus == Quest.Status.NotYetComplete)
            {
                continue;
            }

            // If this objective is optional, display "(Optional)" after its name
            if (objectiveData.optional)
            {
                stringBuilder.AppendFormat("{0} (Optional) - {1}\n",
                                           objectiveData.name, 
                                           objectiveStatus.ToString());
            }
            else
            {
                stringBuilder.AppendFormat("{0} - {1}\n", 
                                           objectiveData.name,
                                           objectiveStatus.ToString());
            }

        }

        // Add a blank line followed by the quest status
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat("Status: {0}", this.questStatus.ToString());

        return stringBuilder.ToString();
    }
}

// Manages a quest.
public class QuestManager : MonoBehaviour {

    // The quest that starts when the game starts.
    [SerializeField] Quest startingQuest = null;

    // A label to show the state of the quest in.
    [SerializeField] UnityEngine.UI.Text objectiveSummary = null;

    // Tracks the state of the current quest.
    QuestStatus activeQuest;

    // Start a new quest when the game starts
    void Start () {

        if (startingQuest != null)
        {
            StartQuest(startingQuest);
        }
    }   

    // Begins tracking a new quest
    public void StartQuest(Quest quest) {

        activeQuest = new QuestStatus(quest);

        UpdateObjectiveSummaryText();

        Debug.LogFormat("Started quest {0}", activeQuest.questData.name);
    }

    // Updates the label that displays the status of the quest and 
    // its objectives
    void UpdateObjectiveSummaryText() {


        string label;

        if (activeQuest == null) {
            label = "No active quest.";
        } else {
            label = activeQuest.ToString();
        }

        objectiveSummary.text = label;
    }

    // Called by other objects to indicate that an objective has changed status
    public void UpdateObjectiveStatus(Quest quest, int objectiveNumber, Quest.Status status) {

        if (activeQuest == null) {
            Debug.LogError("Tried to set an objective status, but no quest is active");
            return;
        }

        if (activeQuest.questData != quest) {
            Debug.LogWarningFormat("Tried to set an objective status for " +
                                   "quest {0}, but this is not the active " +
                                   "quest. Ignoring.", quest.questName);
            return;
        }

        // Update the objective status
        activeQuest.objectiveStatuses[objectiveNumber] = status;

        // Update the display label
        UpdateObjectiveSummaryText();
    }


}
// END quest_manager