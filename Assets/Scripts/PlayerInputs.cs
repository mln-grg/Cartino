using System;
using UnityEngine;
public class PlayerInputs : MonoBehaviour
{
    private float VerticalInput;
    public float verticalinput { get { return VerticalInput; } }
    
    private float HorizontalInput;
    public float horizontalinput { get { return HorizontalInput; } }
    
    public float accelerometerSensitivity;

    public static PlayerInputs instance;
    private void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        //VerticalInput = CrossPlatformInputManager.GetAxis("Vertical");//Input.GetAxis("Vertical");//joystick.Vertical;
        if (Input.GetAxis("Horizontal")!=0)
            HorizontalInput = Input.GetAxis("Horizontal");//Input.acceleration.x * accelerometerSensitivity; //Input.GetAxis("Horizontal");//Input.acceleration.x * accelerometerSensitivity; //joystick.Horizontal;
        else
            HorizontalInput = Input.acceleration.x * accelerometerSensitivity;

    }
}
