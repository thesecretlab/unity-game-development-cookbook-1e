using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
// When the player clicks on a part of the world, the NavMeshAgent moves
// to that position.
[RequireComponent(typeof(NavMeshAgent))]
public class MoveToPoint : MonoBehaviour
{

    // The agent we'll be moving around.
    NavMeshAgent agent;

    void Start()
    {
        // Cache a reference to the nav mesh agent on game start
        agent = GetComponent<NavMeshAgent>();
    }

    // When the user clicks, move the agent.
    void Update()
    {

        // Did the user just click the left mouse button?
        if (Input.GetMouseButtonDown(0))
        {

            // Get the position on screen, in screen coordinates (ie pixels.)
            var mousePosition = Input.mousePosition;

            // Convert this position into a ray that starts at the camera
            // and moves towards where the mouse cursor is.
            var ray = Camera.main.ScreenPointToRay(mousePosition);

            // Store information about any raycast hit in this variable.
            RaycastHit hit;

            // Did the ray hit something?
            if (Physics.Raycast(ray, out hit))
            {

                // Figure out where the ray hit an object.
                var selectedPoint = hit.point;

                // Tell the agent to start walking to that point.
                agent.destination = selectedPoint;

            }
        }
    }
}
