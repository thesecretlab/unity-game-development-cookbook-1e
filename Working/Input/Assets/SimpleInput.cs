using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInput : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {


        // BEGIN input_keyboard
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("The A key was pressed!");
        }

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("The A key is being held down!");
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("The A key was released!");
        }

        if (Input.anyKeyDown) {
            Debug.Log("A key was pressed!");
        }
        // END input_keyboard


        // Mouse button 0 is the left mouse button
        // Mouse button 1 is the right mouse button
        // Mouse button 2 is the middle mouse button (the scroll wheel)

        // BEGIN input_mouse
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button was pressed!");
        }

        if (Input.GetMouseButton(0))
        {
            Debug.Log("Left mouse button is being held down!");
        }

        if (Input.GetMouseButtonUp(0)) {
            Debug.Log("Left mouse button was released!");
        }
        // END input_mouse

        // BEGIN input_mouse_movement
        var mouseX = Input.GetAxis("Mouse X");

        var mouseY = Input.GetAxis("Mouse Y");

        Debug.LogFormat("Mouse movement: {0},{1}", mouseX, mouseY);
        // END input_mouse_movement

        // BEGIN input_mouse_position The mouse position is in screen
        // coordinates, which means it depends on your screen resolution;
        // on a 1920x1080 screen, 0,0 is the top-left, and 1920,1280 is the
        // bottom right.
        var mousePosition = Input.mousePosition;

        // It's often more convenient to convert this to viewport
        // coordinates, in which (0,0) is the top left, and (1,1) is the
        // bottom right, regardless of the screen size
        var screenSpacePosition = 
            Camera.main.ScreenToViewportPoint(mousePosition);
        // END input_mouse_position

        // BEGIN input_axes
        var horizontal = Input.GetAxis("Horizontal");

        var rawHorizontal = Input.GetAxisRaw("Horizontal");

        Debug.LogFormat("Horizontal axis: {0} (raw: {1})", 
                        horizontal, rawHorizontal);
        // END input_axes

        // BEGIN input_screenlock
        // Lock the cursor to the middle of the screen or window.
        Cursor.lockState = CursorLockMode.Locked;

        // Prevent the cursor from leaving the window.
        Cursor.lockState = CursorLockMode.Confined;

        // Don't restrict the mouse cursor at all.
        Cursor.lockState = CursorLockMode.None;

        // Hide the mouse cursor.
        Cursor.visible = false;
        // END input_screenlock


        // BEGIN input_buttons
        if (Input.GetButtonDown("Jump")) {
            Debug.LogFormat("Jump button was pressed!");
        }

        if (Input.GetButton("Jump"))
        {
            Debug.LogFormat("Jump button is being held down!");
        }

        if (Input.GetButtonUp("Jump"))
        {
            Debug.LogFormat("Jump button was released!");
        }
        // END input_buttons

    }
}
