using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN animation_ik
// Updates the positions and weights used for a specified IK goal.
// The Animator attached to the object this script is on must have a layer
// that has an IK pass enabled. Otherwise, OnAnimatorIK won't be called.
[RequireComponent(typeof(Animator))]
public class IKReach : MonoBehaviour {

    // The object we're reaching towards.
    [SerializeField] Transform target = null;

    // An IK goal is a foot or hand that's reaching to a target.
    [SerializeField] AvatarIKGoal goal = AvatarIKGoal.RightHand;

    // The strength with which we're reaching our goal towards the target.
    // 0 = don't reach at all; 1 = the goal must be at the same point as the
    // target, if it's within range.
    [Range(0, 1)]
    [SerializeField] float weight = 0.5f;

    // The animator that's controlling the positions.
    Animator animator;

    // Cache the reference to the animator on start.
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Called every time the animator is about to apply inverse kinematics, 
    // which will bend the various joints to try to reach the goals towards
    // their targets.
    // This is our opportunity to provide it with updated information.
    private void OnAnimatorIK(int layerIndex)
    {
        // Set the position that the goal is trying to reach.
        animator.SetIKPosition(goal, target.position);

        // Set the weight for the goal.
        animator.SetIKPositionWeight(goal, weight);
    }
}
// END animation_ik