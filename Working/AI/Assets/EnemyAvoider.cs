using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN enemy_avoider
using UnityEngine.AI;

// Detects if the target can see us, and if it can, navigates to somewhere
// they can't.
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAvoider : MonoBehaviour {

    // The object that's looking for us. We'll use it to determine if it
    // can see us, and if it can see the places we're considering hiding.
    [SerializeField] EnemyVisibility visibility = null;

    // The size of the area where we're considering hiding.
    [SerializeField] float searchAreaSize = 10f;

    // The density of the search field. Larger numbers means fewer hiding
    // places are considered, but it's more efficient.
    [SerializeField] float searchCellSize = 1f;

    // If true, lines will be drawn indicating where we're considering
    // hiding.
    [SerializeField] bool visualize = true;

    // The navigation agent, which will navigate to the best hiding place.
    NavMeshAgent agent;

    // The Start method is a coroutine; when the game starts, it will start
    // a continuous cycle of avoiding the target.
    IEnumerator Start()
    {
        // Cache a reference to our navigation agent
        agent = GetComponent<NavMeshAgent>();

        // Do this forever:
        while (true) {

            // Can the target see us?
            if (visibility.targetIsVisible) {
                
                // Find a place to run to where it can't see us anymore.

                Vector3 hidingSpot;

                if (FindHidingSpot(out hidingSpot) == false) {
                    // We didn't find anywhere to hide! wait a second and
                    // try again.
                    yield return new WaitForSeconds(1.0f);
                    continue;
                }

                // Tell the agent to start moving to this location
                agent.destination = hidingSpot;
            }

            // Wait a bit, and then check to see if the target can still
            // see us.
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Attempts to find a nearby place that the target can't see us at.
    // Returns true if one was found; if 
    bool FindHidingSpot(out Vector3 hidingSpot) {

        var distribution = new PoissonDiscSampler(
            searchAreaSize, searchAreaSize, searchCellSize);

        var candidateHidingSpots = new List<Vector3>();

        foreach (var point in distribution.Samples()) {

            var searchPoint = point;

            // Re-position the point so that the middle of the search area
            // is at (0,0)
            searchPoint.x -= searchAreaSize / 2f;
            searchPoint.y -= searchAreaSize / 2f;

            var searchPointLocalSpace = new Vector3(
                searchPoint.x,
                transform.localPosition.y,
                searchPoint.y
            );

            // Can they see us from here?
            var searchPointWorldSpace = 
                transform.TransformPoint(searchPointLocalSpace);

            // Find the nearest point on the navmesh
            NavMeshHit hit;

            bool foundPoint;

            foundPoint = NavMesh.SamplePosition(
                searchPointWorldSpace, 
                out hit, 
                5, 
                NavMesh.AllAreas
            );

            if (foundPoint == false) {
                // We can't get here. Disregard as a place to hide.
                continue;
            }

            searchPointWorldSpace = hit.position;

            var canSee = 
                visibility.CheckVisibilityToPoint(searchPointWorldSpace);


            if (canSee == false) {
                // we can't see the target from this position. return it!
                candidateHidingSpots.Add(searchPointWorldSpace);

            }

            if (visualize) {
                Color debugColor = canSee ? Color.red : Color.green;

                Debug.DrawLine(
                    transform.position, searchPointWorldSpace, 
                    debugColor, 0.1f);
            }


        }

        if (candidateHidingSpots.Count == 0) {
            // We didn't find a hiding spot.

            // Provide a dummy value
            hidingSpot = Vector3.zero;

            // Indicate our failure
            return false;
        }


        // For each of our candidate points, calculate the length of the
        // path needed to reach it.

        // Build a list of candidate points, matched with the length of the
        // path needed to reach it.
        List<KeyValuePair<Vector3, float>> paths;

        // For each point, calculate the length
        paths = candidateHidingSpots.ConvertAll(
            (Vector3 point) => {

            // Create a new path that reaches this point
            var path = new NavMeshPath();
            agent.CalculatePath(point, path);

            // Store the distance needed for this path
            float distance;

            if (path.status != NavMeshPathStatus.PathComplete)
            {
                // If this path doesn't reach the target, consider it
                // infinitely far away
                distance = Mathf.Infinity;
            }
            else
            {

                // Get up to 32 of the points on this path
                var corners = new Vector3[32];
                var cornerCount = path.GetCornersNonAlloc(corners);

                // Start with the first point
                Vector3 current = corners[0];

                distance = 0;

                // Figure out the cumulative distance for each point
                for (int c = 1; c < cornerCount; c++)
                {
                    var next = corners[c];
                    distance += Vector3.Distance(current, next);
                    current = next;
                }
            }

            // Build the pair of point and distance
            return new KeyValuePair<Vector3, float>(point, distance);
        });

        // Sort this list based on distance, so that the shortest path is
        // at the front of the list
        paths.Sort((a, b) =>
        {
            return a.Value.CompareTo(b.Value);
        });

        // Return the point that's the shortest to reach
        hidingSpot = paths[0].Key;
        return true;


    }

}
// END enemy_avoider