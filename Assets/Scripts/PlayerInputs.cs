using System;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private float VerticalInput;
    public float verticalinput { get { return VerticalInput; } }
    
    private float HorizontalInput;
    public float horizontalinput { get { return HorizontalInput; } }
    
    private bool isBreaking;
    public bool isbreaking { get { return isBreaking; } }
    
    void FixedUpdate()
    {
        VerticalInput = Input.GetAxis("Vertical");
        HorizontalInput = Input.GetAxis("Horizontal");
        isBreaking = Input.GetKey(KeyCode.Space);
    }
}
