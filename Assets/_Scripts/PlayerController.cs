using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float crouchSpeed = 2.5f;
    //public float rotationSpeed = 10f;
    public float accelerationTime = 0.1f;
    public float decelerationTime = 0.1f;
    public float airControl = 0.5f;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public float jumpCooldown = 0.25f;
    private bool readyToJump = true;

    [Header("Physics Setttings")]
    //public float groundDrag = 6f;
    //public float airDrag = 0.4f;
    public float playerHeight = 1.8f;
    public LayerMask groundMask;
    private bool grounded;

    [Header("Crouch Settings")]
    public float crouchHeight = 0.8f;
    private float originalHeight;
    private bool isCrouching;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float lookSensitivity = 2f;
    public float lookXLimit = 45f;
    private float rotationX = 0; 

    private Rigidbody rb;
    private CapsuleCollider col;
    //private Vector3 moveDirection;
    
    private float horizontalInput;
    private float verticalInput;
    private float currentSpeed;
    private Vector3 currentVelocity;
    private Vector3 currentMoveVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.angularDamping = 0f;

        originalHeight = col.height;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        HandleInput();
        //HandleDrag();
        HandleCamera();
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
        Vector3 targetDir = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        Vector3 targetVelocity = targetDir * currentSpeed;

        float smoothTime = targetDir.magnitude > 0.1f ? accelerationTime : decelerationTime;
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity,ref currentMoveVelocity, smoothTime);

        Vector3 velocity = rb.linearVelocity;
        Vector3 moveVelocity = currentVelocity;
        //Vector3 horizontalVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);
        //Vector3 currentVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);
        //Vector3 velocityChange = (targetVelocity - currentVelocity);

        if (!grounded)
        {
            moveVelocity *= airControl;
        }

        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

        /*
        if (!grounded)
        {
            //velocityChange *= airControl;
            moveVelocity *= airControl; 

            
            if (moveDirection.magnitude > 0.1f)
            {
                rb.angularVelocity = new Vector3(targetVelocity.x, rb.angularVelocity.y, targetVelocity.z);
            }
            else
            {
                rb.angularVelocity = new Vector3(0f, rb.angularVelocity.y, 0f);
            }
            
        //rb.AddForce(targetVelocity * acceleration);
        */

        /* 
        // This is if you want to move the camera with A and D
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * verticalInput + camRight * horizontalInput).normalized;


        if(moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        //This is the old movement
        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        Vector3 targetVelocity = moveDirection * currentSpeed;
        Vector3 currentVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);

        Vector3 velocityChange = (targetVelocity - currentVelocity);

        if (!grounded)
        {
            velocityChange *= airControl;
        }

        rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);
        */
    }

    /*  //This is for momentum
    private void HandleDrag()
    {
        rb.linearDamping = grounded ? groundDrag : 0f;
    }

    */

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
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

    private void HandleCamera()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        //transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSensitivity, 0);
    }

}
