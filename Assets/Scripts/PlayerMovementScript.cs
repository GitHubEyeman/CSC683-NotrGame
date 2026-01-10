using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;

    [Header("Movement Settings")]
    public float moveSpeed = 15f;
    public float maxXPosition = 10f; // Maximum allowed X position (right boundary)
    public float minXPosition = -10f; // Minimum allowed X position (left boundary)

    [Header("Jump Settings")]
    public float jumpForce = 30f;
    public float gravityScale = 5f;
    public float fallingGravityScale = 10f;
    public AudioSource jumpSound;

    private bool isGrounded;
    private float horizontalInput;

    void Start()
    {
        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleJump();
        HandleHorizontalInput();
        ApplyMovement();
    }

    void FixedUpdate()
    {
        ApplyCustomGravity();
    }

    // -------------------- HORIZONTAL MOVEMENT --------------------
    void HandleHorizontalInput()
    {
        // Get horizontal input from both arrow keys and A/D keys
        horizontalInput = 0f;
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            horizontalInput = 1f;
    }

    void ApplyMovement()
    {
        if (horizontalInput != 0f)
        {
            // Calculate new position
            Vector3 newPosition = transform.position + 
                                  new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0f, 0f);
            
            // Clamp the X position to stay within boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, minXPosition, maxXPosition);
            
            // Apply the movement
            transform.position = newPosition;
        }
    }

    // -------------------- JUMP --------------------
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        jumpSound.Play();
    }

    // -------------------- GRAVITY --------------------
    void ApplyCustomGravity()
    {
        float gravityMultiplier =
            rb.linearVelocity.y > 0 ? gravityScale : fallingGravityScale;

        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    // -------------------- GROUND CHECK --------------------
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Optional: Draw boundaries in the editor for visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 leftBoundary = new Vector3(minXPosition, transform.position.y, transform.position.z);
        Vector3 rightBoundary = new Vector3(maxXPosition, transform.position.y, transform.position.z);
        
        Gizmos.DrawLine(leftBoundary + Vector3.up * 2f, leftBoundary + Vector3.down * 2f);
        Gizmos.DrawLine(rightBoundary + Vector3.up * 2f, rightBoundary + Vector3.down * 2f);
    }
}