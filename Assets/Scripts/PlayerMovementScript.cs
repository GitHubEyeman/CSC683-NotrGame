using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;

    [Header("Jump Settings")]
    public float jumpForce = 30f;
    public float gravityScale = 5f;
    public float fallingGravityScale = 10f;

    [Header("Lane Settings")]
    public int numberOfLanes = 5;
    public float laneWidth = 3f;
    public float laneChangeSpeed = 15f;

    private int currentLane = 2; // middle lane
    private float targetXPosition;
    private bool isChangingLane;

    private bool isGrounded;

    void Start()
    {
        rb.freezeRotation = true;
        targetXPosition = CalculateLanePosition(currentLane);
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameRunning || GameManager.Instance.isGamePaused)
        return;
        HandleJump();
        HandleLaneInput();
        MoveToLane();
    }

    void FixedUpdate()
    {
        ApplyCustomGravity();
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
    }

    // -------------------- GRAVITY --------------------

    void ApplyCustomGravity()
    {
        float gravityMultiplier =
            rb.linearVelocity.y > 0 ? gravityScale : fallingGravityScale;

        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    // -------------------- LANES --------------------

    void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            ChangeLane(1);
    }

    void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;

        if (newLane >= 0 && newLane < numberOfLanes)
        {
            currentLane = newLane;
            targetXPosition = CalculateLanePosition(currentLane);
            isChangingLane = true;
        }
    }

    void MoveToLane()
    {
        if (!isChangingLane) return;

        Vector3 pos = transform.position;
        float newX = Mathf.MoveTowards(pos.x, targetXPosition, laneChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, pos.y, pos.z);

        if (Mathf.Abs(newX - targetXPosition) < 0.01f)
            isChangingLane = false;
    }

    float CalculateLanePosition(int laneIndex)
    {
        float leftMost = -((numberOfLanes - 1) * laneWidth) / 2f;
        return leftMost + laneIndex * laneWidth;
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
}
