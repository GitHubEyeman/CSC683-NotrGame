using UnityEngine;

public class PlayerJump3D : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 30f;
    public float gravityScale = 10f;
    public float fallingGravityScale = 40f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float currentGravity =
            rb.linearVelocity.y >= 0 ? gravityScale : fallingGravityScale;

        rb.AddForce(Physics.gravity * currentGravity, ForceMode.Acceleration);
    }
}
