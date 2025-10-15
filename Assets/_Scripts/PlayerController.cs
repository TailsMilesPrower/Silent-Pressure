using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float groundDrag = 4f;
    public float airMultiplier = 0.4f;

    [Header("Jump Settings")]
    public float jumpForce = 4f;
    public float jumpCooldown = 0.25f;
    private bool readyToJump = true;    

    [Header("Ground Check")]
    public float playerHeight = 1.8f;
    public LayerMask Ground;
    private bool grounded;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        
        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        rb.linearDamping = grounded ? groundDrag : 0f;
    }


    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = new Vector3 (horizontalInput, 0f, verticalInput).normalized;


        if (grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);

        else
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
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

}
