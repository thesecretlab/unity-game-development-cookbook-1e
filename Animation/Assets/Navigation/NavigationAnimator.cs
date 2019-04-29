using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN navigation_animator
using UnityEngine.AI;

// Uses the NavMeshAgent to drive parameters on the Animator.
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NavigationAnimator : MonoBehaviour
{

    // Selects what the final position of the object is controlled by.
    enum Mode {
        // The agent directly controls the object's position. More
        // accurate, but causes foot sliding (because the animator isn't
        // moving at the exact speed of the agent.)
        AgentControlsPosition,

        // The animator's root motion controls the object's position. Looks
        // better, but less accurate because the motion that results won't
        // precisely match what the agent wants to do.
        AnimatorControlsPosition
    }

    // The mode that this script is operating in.
    [SerializeField] Mode mode = Mode.AnimatorControlsPosition;

    // The names of the parameters that this script will control.
    [SerializeField] string isMovingParameterName = "Moving";
    [SerializeField] string sidewaysSpeedParameterName = "X Speed";
    [SerializeField] string forwardSpeedParameterName = "Z Speed";

    // Cached references to components we'll be accessing every frame.
    Animator animator;
    NavMeshAgent agent;

    // Stores the movement we did last frame, so that we can smooth out
    // our movement over time.
    Vector2 smoothDeltaPosition = Vector2.zero;

    void Start()
    {
        // Cache references to the animator and the nav mesh agent.
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    
        // The animator will potentially be in charge of our position, not
        // the agent. Disable the agent's ability to directly set this
        // object's position. (The agent will retain the ability to rotate
        // the object.)

        agent.updatePosition = false;

    }

    void Update()
    {
        // The agent stores where it wants to move next in
        // agent.nextPosition.

        // Calculate how far we currently are from where the agent wants to
        // be, in world space
        Vector3 worldDeltaPosition = 
            agent.nextPosition - transform.position;

        // Convert this to local space - we need to know how much of a
        // movement in the X and Z planes this would be
        float xMovement = 
            Vector3.Dot(transform.right, worldDeltaPosition);
        float zMovement = 
            Vector3.Dot(transform.forward, worldDeltaPosition);

        Vector2 localDeltaPosition = new Vector2(xMovement, zMovement);

        // Smooth out this movement by interpolating from the last frame's 
        // movement to this one
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, 
                                           localDeltaPosition, smooth);

        // Figure out our velocity
        var velocity = smoothDeltaPosition / Time.deltaTime;

        // We need to tell the animator that we're moving when our velocity
        // exceeds a threshold, _and_ we're not too close to the
        // destination.
        bool shouldMove = velocity.magnitude > 0.5f &&
                                  agent.remainingDistance > agent.radius;

        // We now have all the information we need to tell the animator
        // what to do. Update its parameters; the animation controller will
        // play the right animation. This will also update the animation's
        // root position as well, which we can optionally use to control or
        // influence the object's position.

        // We're providing three parameters here. 
        // - 'Moving' is a bool that unambiguously indicates whether we
        //   want to be idle or moving. 
        // - X and Z speed are intended to control a 2D blend tree; 
        // - Z speed is forward and backward, while X speed is left and
        //   right.

        // This can also work with a 1D blend tree, where Z speed is the
        // only parameter, but if you do this, you should use this script
        // in AgentControlsPosition mode. 
        //
        // This is because if your animation controller doesn't have any
        // animations that move the root position sideways, the animator
        // will find it difficult to make the kinds of turns that the agent
        // may try to do, and the result will be the visible object jumping
        // around on screen as a result of repeatedly moving too far away
        // from the agent. Play with your blend trees and animations in
        // order to get good feeling movement.

        animator.SetBool(isMovingParameterName, shouldMove);
        animator.SetFloat(sidewaysSpeedParameterName, velocity.x);
        animator.SetFloat(forwardSpeedParameterName, velocity.y);

        // Is the animator controlling our position, instead of the agent?
        if (mode == Mode.AnimatorControlsPosition) {

            // If the animator is controlling our position, the agent will
            // start drifting away from the object. If this happens, you'll
            // start seeing visual glitches caused by the navigation logic
            // not matching the visible object on screen.

            // To fix this, we'll detect if the object is significantly far
            // from where the agent is. 'Significantly' means it's more
            // than one agent-radius away from the agent's position (that
            // is, the object is outside the agent's cylinder.)

            // When this happens, we'll start bringing the object closer to
            // the agent's position. This reduces animation realism
            // slightly, because it's movement that's not reflected in the
            // motion of the character, but it prevents larger errors from
            // accumulating over time.

            // Is the object's position far from where the agent wants to
            // be?
            if (worldDeltaPosition.magnitude > agent.radius)
            {
                // Bring us closer to where the agent wants to be
                transform.position = Vector3.Lerp(transform.position, 
                                                  agent.nextPosition, 
                                                  Time.deltaTime / 0.15f);
            }
        }
    }

    void OnAnimatorMove()
    {
        // Which mode is this script in?
        switch (mode)
        {
            case Mode.AgentControlsPosition:
                // Move the object directly to where the agent wants to be.
                transform.position = agent.nextPosition;
                break;
            case Mode.AnimatorControlsPosition:        
                // Update our position to where the animation system has
                // placed us, following the animation's root movement.

                // Override the movement in the Y axis to where the agent
                // wants to be (otherwise, we'll just pass through hills,
                // stairs, and other changes in ground height.)

                Vector3 position = animator.rootPosition;
                position.y = agent.nextPosition.y;
                transform.position = position;
                break;
        }
    }
}
// END navigation_animator