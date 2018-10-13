using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN lap_tracker
// We use LINQ to help figure out the start of the circuit with fewer lines
// of code. Using LINQ allocates memory, which is something we try to
// avoid, but because we only do it once (at scene start), it's less bad.
using System.Linq;

public class LapTracker : MonoBehaviour {

    // The object that we're tracking as it makes laps around the circuit.
    [SerializeField] Transform target = null;
       
    // The number of nodes in the list we're permitted to skip. This
    // prevents the player from just driving a tiny circle from the start
    // of the track to the end ("I crossed the finish line three times!
    // That means I win!") Increase this number to permit longer shortcuts.
    // Set this to zero to forbid any shortcuts.
    [SerializeField] int longestPermittedShortcut = 2;

    // The UI element that appears to let the player know they're going the
    // wrong way.
    [SerializeField] GameObject wrongWayIndicator;

    // A text field that displays the number of laps the player has
    // completed.
    [SerializeField] UnityEngine.UI.Text lapCounter;

    // The number of laps the player has completed.
    int lapsComplete = 0;
    
    // The checkpoint that the player was near most recently.
    Checkpoint lastSeenCheckpoint;

    // The list of all checkpoints on the circuit. We keep a copy of it
    // here, because we need to use this list every frame, and because
    // using FindObjectsOfType to re-generate the list every frame would be
    // slow.
    Checkpoint[] allCheckpoints;

    
    // The start checkpoint is the first (and hopefully only) checkpoint
    // that has isLapStart turned on.
    Checkpoint StartCheckpoint {
        get {
            // Get the checkpoint marked as the start of the lap
            return FindObjectsOfType<Checkpoint>()
                .Where(c => c.isLapStart)
                .FirstOrDefault();           
        }
    }

    void Start () {

        // Ensure that the counter says "lap 1"
        UpdateLapCounter();

        // The player isn't going the wrong way at the start
        wrongWayIndicator.SetActive(false);
        
        // Create the list of all checkpoints, which Update() will make use
        // of
        allCheckpoints = FindObjectsOfType<Checkpoint>();

        // Create the circuit of connected checkpoints
        CreateCircuit();

        // Begin the race at the start of the circuit
        lastSeenCheckpoint = StartCheckpoint;
    }

    private void Update()
    {
        // What's the nearest checkpoint?
        var nearestCheckpoint = NearestCheckpoint();

        if (nearestCheckpoint == null) {
            // No checkpoints! Bail out.
            return;
        }
        
        if (nearestCheckpoint.index == lastSeenCheckpoint.index) {
            // nothing to do; the nearest checkpoint has not changed
        } else if (nearestCheckpoint.index > lastSeenCheckpoint.index) {

            var distance = 
                nearestCheckpoint.index - lastSeenCheckpoint.index;
            
            if (distance > longestPermittedShortcut + 1) {
                // the player has skipped too many checkpoints. 
                // treat this as going the wrong way.
                wrongWayIndicator.SetActive(true);
            } else {
                // We are near the next checkpoint; the player is going the
                // right way.
                lastSeenCheckpoint = nearestCheckpoint;

                wrongWayIndicator.SetActive(false);
            }
            
        } else if (nearestCheckpoint.isLapStart && 
                   lastSeenCheckpoint.next.isLapStart) {
            // If the last checkpoint we saw is the last in the circuit,
            // and our nearest is now the start of the circuit, we just
            // completed a lap!
                     
            lastSeenCheckpoint = nearestCheckpoint;

            lapsComplete += 1;
            UpdateLapCounter();

        } else {
            // This checkpoint is lower than the last one we saw. The
            // player is going the wrong way.
            wrongWayIndicator.SetActive(true);
        }
    }

    // Calculates the nearest checkpoint to the player.
    Checkpoint NearestCheckpoint() {

        // If we don't have a list of checkpoints to use, exit immediately
        if (allCheckpoints == null) {
            return null;
        }

        // Loop through the list of all checkpoints, and find the nearest
        // one to the player's position.
        Checkpoint nearestSoFar = null;
        float nearestDistanceSoFar = float.PositiveInfinity;

        for (int c = 0; c < allCheckpoints.Length; c++) {
            var checkpoint = allCheckpoints[c];
            var distance = 
                (target.position - checkpoint.transform.position)
                .sqrMagnitude;

            if (distance < nearestDistanceSoFar) {
                nearestSoFar = checkpoint;
                nearestDistanceSoFar = distance;
            }
        }

        return nearestSoFar;
    }
    

    // Walks the list of checkpoints, and makes sure that they all have an
    // index that's one higher than the previous one (except for the start
    // checkpoint)
    void CreateCircuit() {

        var index = 0;

        // Start at the start of the checkpoint
        var currentCheckpoint = StartCheckpoint;

        do
        {
            // Update the index for this checkpoint
            currentCheckpoint.index = index;
            index += 1;

            // Move to the checkpoint it's pointing to
            currentCheckpoint = currentCheckpoint.next;

            // We should not reach the end of the list - that means that
            // the circuit does not form a loop
            if (currentCheckpoint == null)
            {
                Debug.LogError("The circuit is not closed!");
                return;
            }

            // loop until we reach the start again
        } while (currentCheckpoint.isLapStart == false); 

    }
    
    // Update the text that's shown to the user
    void UpdateLapCounter()
    {
        lapCounter.text = string.Format("Lap {0}", lapsComplete + 1);
    }

    // Draw a line indicating the nearest checkpoint to the player in the
    // scene view. (Useful for debugging.)
    private void OnDrawGizmos()
    {
        var nearest = NearestCheckpoint();

        if (target != null && nearest != null) {

            Gizmos.color = Color.red;
            Gizmos.DrawLine(target.position, nearest.transform.position);
            
        }
    }
}
// END lap_tracker