using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN animation_movement
// Allows the control 
public class CharacterMovement : MonoBehaviour {

    // The animator whose parameters we are controlling
    Animator animator;

    [SerializeField] float speed = 1f;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	void Update () {

        animator.SetFloat("Speed", Input.GetAxis("Vertical") * speed);
        // BEGIN animation_movement_sidespeed
        animator.SetFloat("SideSpeed", Input.GetAxis("Horizontal") * speed);
        // END animation_movement_sidespeed
	}
}
// END animation_movement