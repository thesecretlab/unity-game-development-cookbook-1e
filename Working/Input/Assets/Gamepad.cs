using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamepad : MonoBehaviour
{
    private void Start()
    {
        // BEGIN input_joystick_list
        var names = Input.GetJoystickNames();

        Debug.LogFormat("Joysticks: {0}", names);
        // END input_joystick_list


    }

    private void Update()
    {
        // BEGIN input_joystick_buttons
        // The list of all 20 buttons on the first joystick that Unity can
        // access. (This list repeats for joystick 2, 3, and so on.)
        var buttons = new[] {
            KeyCode.Joystick1Button0,
            KeyCode.Joystick1Button1,
            KeyCode.Joystick1Button2,
            KeyCode.Joystick1Button3,
            KeyCode.Joystick1Button4,
            KeyCode.Joystick1Button5,
            KeyCode.Joystick1Button6,
            KeyCode.Joystick1Button7,
            KeyCode.Joystick1Button8,
            KeyCode.Joystick1Button9,
            KeyCode.Joystick1Button10,
            KeyCode.Joystick1Button11,
            KeyCode.Joystick1Button12,
            KeyCode.Joystick1Button13,
            KeyCode.Joystick1Button14,
            KeyCode.Joystick1Button15,
            KeyCode.Joystick1Button16,
            KeyCode.Joystick1Button17,
            KeyCode.Joystick1Button18,
            KeyCode.Joystick1Button19
        };

        // Loop over every button and report on its state.
        foreach (var button in buttons) {
            if (Input.GetKeyDown(button)) {
                Debug.LogFormat("Button {0} pressed", button);
            }

            if (Input.GetKeyUp(button))
            {
                Debug.LogFormat("Button {0} released", button);
            }
        }
        // END input_joystick_buttons

        // BEGIN input_joystick_axes
        Debug.LogFormat(
            "Primary Joystick: X: {0}; Y:{1}", 
            Input.GetAxis("Horizontal"), 
            Input.GetAxis("Vertical")
        );
        // END input_joystick_axes
    }
}
