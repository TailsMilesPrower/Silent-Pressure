using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float crouchSpeed = 2.5f;
    public float acceleration = 10f;
    public float airControl = 0.3f;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public float jumpCooldown = 0.25f;
    private bool readyToJump = true;    

    [Header("Physics Setttings")]
    public float groundDrag = 6f;
    public float airDrag = 0.4f;
    public float playerHeight = 1.8f;

    public LayerMask groundMask;
    private bool grounded;

    [Header("Crouch Settings")]
    public float crouchHeight = 0.8f;
    private float originalHeight;
    private bool isCrouching;


    private Rigidbody rb;
    private CapsuleCollider col;
    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;
    private float currentSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<CapsuleCollider>();
        
        rb.freezeRotation = true;
        originalHeight = col.height;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        HandleInput();
        HandleDrag();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Running
        bool running = Input.GetKey(KeyCode.LeftShift);

        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            StartCrouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
        {
            StopCrouch();
        }

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if (running && grounded)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // Jumping
        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }


    private void MovePlayer()
    {
        moveDirection = new Vector3 (horizontalInput, 0f, verticalInput).normalized;

        Vector3 targetVelocity = moveDirection * currentSpeed;
        Vector3 currentVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);
        
        Vector3 velocityChange = (targetVelocity - currentVelocity);

        if (!grounded)
        {
            velocityChange *= airControl;
        }
            
        rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);
    }

    private void HandleDrag()
    {
        rb.linearDamping = grounded ? groundDrag : 0f;
    }

    private void Jump()
    {
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void StartCrouch()
    {
        if (isCrouching) return;
        col.height = crouchHeight;
        transform.position = new Vector3(transform.position.x, transform.position.y - (originalHeight - crouchHeight) / 2f, transform.position.z);
        isCrouching = true;
    }

    private void StopCrouch()
    {
        if (!isCrouching) return;
        col.height = originalHeight;
        transform.position = new Vector3(transform.position.x, transform.position.y + (originalHeight - crouchHeight) / 2f, transform.position.z);
        isCrouching = false;
    }

}
