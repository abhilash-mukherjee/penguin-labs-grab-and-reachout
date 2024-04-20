using UnityEngine;

public class HaloAnimator : MonoBehaviour
{
    [SerializeField] private float sizeAmplitude = 0.05f; // The amount the size will change
    [SerializeField] private float frequency = 3f;       // How long it takes to complete one cycle of size change in seconds

    private Vector3 originalScale;
    private float timer;

    void Start()
    {
        // Store the original scale of the GameObject
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Update the timer based on the time since last frame
        timer += Time.deltaTime;
        // Calculate scale factor based on a sine wave
        float scaleFactor = 1 + sizeAmplitude * Mathf.Sin(2 * Mathf.PI * timer / frequency);
        // Apply the new scale to the GameObject
        transform.localScale = originalScale * scaleFactor;
    }
}
