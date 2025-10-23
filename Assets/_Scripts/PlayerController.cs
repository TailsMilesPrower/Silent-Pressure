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

    [Header("Sprint FOV Settings")]
    public float normalFOV = 70f;
    public float sprintFOV = 85f;
    public float fovSmoothTime = 0.15f;
    private float fovVelocity;

    private Rigidbody rb;
    private CapsuleCollider col;
    //private Vector3 moveDirection;
    
    private float horizontalInput;
    private float verticalInput;
    private float targetSpeed;
    private float currentSpeed;
    private bool running;

    private Vector3 smoothVelocity;
    private Vector3 smoothRef;

    private PlayerStamina stamina;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<CapsuleCollider>();
        stamina = GetComponent<PlayerStamina>();

        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.angularDamping = 0f;

        originalHeight = col.height;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        HandleInput();
        //HandleDrag();
        HandleCamera();
        HandleFOV();
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
        bool sprintPressed = Input.GetKey(KeyCode.LeftShift);
        bool hasMovementInput = (horizontalInput != 0 || verticalInput != 0);
        bool canSprint = !isCrouching && hasMovementInput;

        //running = Input.GetKey(KeyCode.LeftShift) && !isCrouching && (horizontalInput != 0 || verticalInput != 0);
        bool staminaAllowsSprint = true;
        if (stamina != null) staminaAllowsSprint = !(stamina.IsExhausted());

        running = sprintPressed && canSprint && staminaAllowsSprint;

        if (stamina != null)
        {
            if (running)
            {
                stamina.SetSprinting(true);
                stamina.Drain(Time.deltaTime);
            }
            else
            {
                stamina.SetSprinting(false);
            }
        }

        if (isCrouching)
        {
            targetSpeed = crouchSpeed;
        }
        else if (running)
        {
            targetSpeed = runSpeed;
            stamina.Drain(Time.deltaTime);
        }
        else
        {
            targetSpeed = walkSpeed;
        }

        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            StartCrouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
        {
            StopCrouch();
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
        Vector3 moveDir = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;

        //float speedChangeRate = running ? accelerationTime * 0.5f : accelerationTime;
        float smoothTime = moveDir.magnitude > 0.1f ? accelerationTime : decelerationTime;
        if (smoothTime <= 0f) smoothTime = 0.01f;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime / smoothTime);

        Vector3 targetVelocity = moveDir * currentSpeed;

        smoothVelocity = Vector3.SmoothDamp(smoothVelocity, targetVelocity, ref smoothRef, smoothTime);

        Vector3 velocity = rb.linearVelocity;
        Vector3 moveVelocity = smoothVelocity;
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

        if (stamina != null) stamina.SetSprinting(false);
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

    private void HandleFOV()
    {
        if(!playerCamera) return;

        float targetFOV = (running && !isCrouching) ? sprintFOV : normalFOV;
        float newFOV = Mathf.SmoothDamp(playerCamera.fieldOfView, targetFOV, ref fovVelocity, fovSmoothTime);
        playerCamera.fieldOfView = newFOV;
    }

}
