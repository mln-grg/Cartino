    using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    private PlayerInputs playerInputs;
    [SerializeField]private Rigidbody rb;
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxTurn = 25f;
    [SerializeField] Transform leftFrontWheel;
    [SerializeField] Transform rightFrontWheel;
    [SerializeField] float knockbackforce;
    public float recoilForce;
    private bool knocking = false;
    private Rigidbody playerRb;
    private PlayerHealth playerHealth;

    private float turnInput,speedInput;
    public TrailRenderer[] trailRenderers;

    [SerializeField] private float knockBackTime;
    public float carMaxSpeed = 32;
    public float carCurrentSpeed;
    public bool gameStarted = false;
    private void Awake()
    {
        Instance = this;
        playerInputs = GetComponent<PlayerInputs>();
        playerRb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();
 
    }
    private void Start()
    {
        rb.transform.parent = null;
    }
    private void Update()
    {
        if(playerHealth.IsDead == false)
            carCurrentSpeed = (rb.velocity.magnitude * 3.6f) / carMaxSpeed;
       
        if (playerHealth.IsDead == false)
        {
            transform.localPosition = rb.transform.position;
            speedInput = 0;

            turnInput = playerInputs.horizontalinput;
            if (turnInput != 0)
            {
                foreach(TrailRenderer t in trailRenderers)
                {
                    t.emitting = true;
                }
            }
            else
            {
                foreach (TrailRenderer t in trailRenderers)
                {
                    t.emitting = false;
                }
            }

            speedInput = forwardAcceleration * 1000f;


            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnSpeed * Time.deltaTime * 1, 0f));

            leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxTurn), leftFrontWheel.localRotation.eulerAngles.z);
            rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxTurn), rightFrontWheel.localRotation.eulerAngles.z);
        }
    }
    private void FixedUpdate()
    {
        if(playerHealth.IsDead == false && !knocking && gameStarted)
            rb.AddForce(transform.forward * speedInput);
    }

    public void KnockBack()
    {
        StartCoroutine(Knocking());
    }
    public void ForwardPush()
    {

        rb.AddExplosionForce(recoilForce, transform.position, 2f);
    }
    IEnumerator Knocking()
    {
        knocking = true;
        rb.AddForce(transform.forward * -1 * speedInput * knockbackforce);
        yield return new WaitForSeconds(knockBackTime);
        knocking = false;
    }
    public void freezePlayer()
    {
        //playerRb.constraints = RigidbodyConstraints.FreezeAll;  
        //playerRb.mass = 10000000f;              
        //rb.mass = 10000000f;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        //rb.rotation = Quaternion.identity;
        //rb.isKinematic = true;
        Destroy(rb);
        Destroy(playerRb);
       
    }

    
}
