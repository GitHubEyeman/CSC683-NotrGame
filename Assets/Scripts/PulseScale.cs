using UnityEngine;

public class PulseScale : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;   // How fast the object pulses
    public float pulseAmount = 0.2f; // How much it scales up/down

    private Vector3 originalScale;

    void Start()
    {
        // Store the original scale of the object
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Calculate the new scale using a sine wave
        float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        // Apply the pulsing scale
        transform.localScale = originalScale * scaleFactor;
    }
}
