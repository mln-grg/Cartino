using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerInputs : MonoBehaviour
{
    private float VerticalInput;
    public float verticalinput { get { return VerticalInput; } }
    
    private float HorizontalInput;
    public float horizontalinput { get { return HorizontalInput; } }
    
    [SerializeField] private float accelerometerSensitivity;
    
    void FixedUpdate()
    {
        VerticalInput = CrossPlatformInputManager.GetAxis("Vertical");//Input.GetAxis("Vertical");//joystick.Vertical;
        HorizontalInput = Input.GetAxis("Horizontal");//Input.acceleration.x * accelerometerSensitivity; //joystick.Horizontal;
                                                      //Debug.Log(HorizontalInput);

    }
}
