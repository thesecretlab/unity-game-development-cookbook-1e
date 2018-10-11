using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN statemachine
// Manages a collection of states, which can be transitioned from and to.
public class StateMachine {

    // A single state.
    public class State
    {
        // The state's visible name. Also used to identify the state to the
        // state machine.
        public string name;

        // Called every frame while the state is active.
        public System.Action onFrame;

        // Called when the state is transitioned to from another state.
        public System.Action onEnter;

        // Called when the state is transitioning to another state.
        public System.Action onExit;

        public override string ToString()
        {
            return name;
        }
    }

    // The collection of named states.
    Dictionary<string, State> states = new Dictionary<string, State>();

    // The state that we're currently in.
    public State currentState { get; private set; }

    // The state that we'll start in.
    public State initialState;

    // Creates, registers and returns a new named state.
    public State CreateState(string name) {

        // Create the state
        var newState = new State();

        // Give it a name
        newState.name = name;

        // If this is the first state, it will be our initial state
        if (states.Count == 0)
        {
            initialState = newState;
        }

        // Add it to the dictionary
        states[name] = newState;

        // And return it, so that it can be further configured
        return newState;
    }

    // Updates the current state.
    public void Update() {

        // If we don't have any states to use, log the error.
        if (states.Count == 0 || initialState == null) {
            Debug.LogErrorFormat("State machine has no states!");
            return;
        }

        // If we don't currently have a state, transition to the initial state.
        if (currentState == null) {
            TransitionTo(initialState);
        }

        // If the current state has an onFrame method, call it.
        if (currentState.onFrame != null) {
            currentState.onFrame();
        }
    }

    // Transitions to the specified state.
    public void TransitionTo(State newState) {

        // Ensure we weren't passed null
        if (newState == null)
        {
            Debug.LogErrorFormat("Cannot transition to a null state!");
            return;
        }

        // If we have a current state and that state has an on exit method,
        // call it
        if (currentState != null && currentState.onExit != null)
        {
            currentState.onExit();
        }

        Debug.LogFormat("Transitioning from '{0}' to '{1}'", currentState, newState);

        // This is now our current state
        currentState = newState;

        // If the new state has an on enter method, call it
        if (newState.onEnter != null)
        {
            newState.onEnter();
        }
    }

    // Transitions to a named state.
    public void TransitionTo(string name) {

        if (states.ContainsKey(name) == false) {
            Debug.LogErrorFormat("State machine doesn't contain a state " +
                                 "named {0}!", name);
            return;
        }

        // Find the state in the dictionary
        var newState = states[name];

        // Transition to it
        TransitionTo(newState);

    }

}
// END statemachine

// BEGIN statemachine_demo
// Demonstrates a state machine. This object has two states: 'searching', and
// 'aiming'. When the target is in range, it transitions from 'searching' to
// 'aiming'; when the target leaves range, it transitions back.
public class Turret : MonoBehaviour {
    
    // The object we'll rotate to aim
    [SerializeField] Transform weapon;

    // The object we're trying to aim at
    [SerializeField] Transform target;

    // Aim at the target when it's within this range
    [SerializeField] float range = 5f;

    // The arc that we'll turn in while the target is out of range
    [SerializeField] float arc = 45;

    // The state machine that manages this object
    StateMachine stateMachine;

    // Use this for initialization
    void Start () {

        // Create the state machine
        stateMachine = new StateMachine();

        // The first state we register will be the initial state
        var searching = stateMachine.CreateState("searching");

        // Log when we enter the state
        searching.onEnter = delegate {
            Debug.Log("Now searching for the target...");
        };

        // Each frame, animate the turret, and also check to see if the
        // target is in range
        searching.onFrame = delegate {

            // Sweep from side to side
            var angle = Mathf.Sin(Time.time) * arc / 2f;
            weapon.eulerAngles = Vector3.up * angle;

            // Find the distance to our target
            float distanceToTarget = 
                Vector3.Distance(transform.position, target.position);

            // Are they in range?
            if (distanceToTarget <= range) {
                // Then transition to the aiming state
                stateMachine.TransitionTo("aiming");
            }
        };

        // The aiming state runs when the target is in range.
        var aiming = stateMachine.CreateState("aiming");

        // Every frame, keep the turret aimed at the target. Detect when the
        // target leaves range.
        aiming.onFrame = delegate {

            // Aim the weapon at the target
            weapon.LookAt(target.position);
        
            // Transition back to 'searching' when it's out of range
            float distanceToTarget =
                Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > range)
            {
                stateMachine.TransitionTo("searching");
            }
        };

        // 
        aiming.onEnter = delegate {
            Debug.Log("Target is in range!");
        };

        aiming.onExit = delegate {
            Debug.Log("Target went out of range!");
        };
    }
    
    void Update () {
        // Update the state machine's current state
        stateMachine.Update();
    }
}
// END statemachine_demo
