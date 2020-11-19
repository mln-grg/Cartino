using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs playerInputs;
    [SerializeField]private Rigidbody rb;
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxTurn = 25f;
    [SerializeField] Transform leftFrontWheel;
    [SerializeField] Transform rightFrontWheel;

    private float turnInput,speedInput;

    private void Awake()
    {
        playerInputs = GetComponent<PlayerInputs>();
 
    }
    private void Start()
    {
        rb.transform.parent = null;
    }
    private void Update()
    {
      transform.localPosition = rb.transform.position;
        speedInput = 0;

        turnInput = playerInputs.horizontalinput;
        
        speedInput = forwardAcceleration * 1000f;

        rb.AddForce(transform.forward * speedInput);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnSpeed * Time.deltaTime * 1, 0f));

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxTurn), leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxTurn), rightFrontWheel.localRotation.eulerAngles.z);
    }

    
}
