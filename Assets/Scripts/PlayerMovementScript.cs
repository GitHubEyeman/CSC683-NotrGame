using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;          // Max horizontal speed
    public float horizontalSmooth = 0.1f; // How fast we reach target X velocity
    public float jumpForce = 7f;
    public float airControlFactor = 0.5f; // Reduce horizontal control in air

    [Header("Bike Lean")]
    public float maxLeanAngle = 20f;      // Max tilt angle (degrees)
    public float leanSmooth = 5f;         // Smoothness of leaning

    [Header("Custom Gravity")]
    public float gravityForce = 20f;

    private Rigidbody rb;
    private float horizontalInput;
    private bool isGrounded;

    // Horizontal movement smoothing
    private float targetXVelocity;
    private float currentXVelocity;
    private float velocityXSmooth;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        // Get input (-1 left, 0 none, 1 right)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Jump
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move();
        ApplyCustomGravity();
        HandleLean();
    }

    void Move()
    {
        // Apply reduced control in air
        float controlFactor = isGrounded ? 1f : airControlFactor;

        // Target horizontal velocity
        targetXVelocity = horizontalInput * moveSpeed * controlFactor;

        // Smoothly interpolate current X velocity toward target
        currentXVelocity = Mathf.SmoothDamp(
            rb.linearVelocity.x,
            targetXVelocity,
            ref velocityXSmooth,
            horizontalSmooth
        );

        // Apply velocity
        Vector3 velocity = rb.linearVelocity;
        velocity.x = currentXVelocity;
        rb.linearVelocity = velocity;
    }

    void Jump()
    {
        // Reset vertical velocity for consistent jump
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Prevent double jump
        isGrounded = false;
    }

    void ApplyCustomGravity()
    {
        rb.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
    }

    void HandleLean()
    {
        // Target lean angle based on horizontal input
        float targetAngle = -horizontalInput * maxLeanAngle;

        // Smoothly interpolate current rotation toward target
        Vector3 currentEuler = transform.eulerAngles;
        float smoothZ = Mathf.LerpAngle(currentEuler.z, targetAngle, Time.fixedDeltaTime * leanSmooth);

        transform.rotation = Quaternion.Euler(currentEuler.x, currentEuler.y, smoothZ);
    }

    // -------- Ground Check via Collisions --------

    void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }

        isGrounded = false;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
