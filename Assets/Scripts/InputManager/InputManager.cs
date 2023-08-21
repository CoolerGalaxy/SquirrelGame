using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum InputMode 
    {
        Gamepad,
        KeyboardMouse
    } 
    
    public InputMode inputMode { get; private set; }

    private void Update()
    {
        // Detect any gamepad input
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            inputMode = InputMode.Gamepad;
        }

        // Detect any mouse input
        if (
            Input.GetMouseButton(0) || 
            Input.GetMouseButton(1) || 
            Input.GetMouseButton(2) || 
            Input.GetAxis("Mouse X") != 0 || 
            Input.GetAxis("Mouse Y") != 0
        ){
            inputMode = InputMode.KeyboardMouse;
        }

        // Detect any keyboard input
        if (Input.anyKeyDown)
        {
            inputMode = InputMode.KeyboardMouse;
        }
    }
}
