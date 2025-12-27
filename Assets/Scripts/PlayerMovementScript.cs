using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 30f;
    public float gravityScale = 10f;
    public float fallingGravityScale = 40f;
    
    // Lane settings
    public int numberOfLanes = 5;
    public float laneWidth = 3f;
    public float laneChangeSpeed = 15f;
    
    private int currentLane = 2; // Start in middle lane (0-4)
    private float targetXPosition;
    private bool isChangingLane = false;

    void Start()
    {
        // Calculate initial lane position
        targetXPosition = CalculateLanePosition(currentLane);
    }

    void Update()
    {
        // Jumping - can jump anytime (infinite jump)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        HandleLaneChange();
        UpdatePosition();
    }

    void HandleLaneChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }
    }

    void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;
        
        // Check if new lane is valid (within 0 to numberOfLanes-1)
        if (newLane >= 0 && newLane < numberOfLanes)
        {
            currentLane = newLane;
            targetXPosition = CalculateLanePosition(currentLane);
            isChangingLane = true;
        }
    }

    float CalculateLanePosition(int laneIndex)
    {
        // Center lanes around 0
        float leftmostLane = -((numberOfLanes - 1) * laneWidth) / 2f;
        return leftmostLane + (laneIndex * laneWidth);
    }

    void UpdatePosition()
    {
        // Get current position
        Vector3 currentPos = transform.position;
        
        // Only update X position for lane changes
        if (isChangingLane)
        {
            float newX = Mathf.MoveTowards(currentPos.x, targetXPosition, 
                laneChangeSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, currentPos.y, currentPos.z);
            
            // Check if we've reached the target lane
            if (Mathf.Abs(currentPos.x - targetXPosition) < 0.01f)
            {
                isChangingLane = false;
            }
        }
    }
    
    void FixedUpdate()
    {
        // Apply custom gravity
        float currentGravity = rb.linearVelocity.y >= 0 ? gravityScale : fallingGravityScale;
        rb.AddForce(Physics.gravity * currentGravity, ForceMode.Acceleration);
    }
}