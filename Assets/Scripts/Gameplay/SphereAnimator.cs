using UnityEngine;

public class SphereAnimator : MonoBehaviour
{
    [SerializeField] private SphereController sphereController;
    [SerializeField] private float amplitude = 0.5f;  // Height of the floating effect
    [SerializeField] private float frequency = 1f;   // Speed of the floating effect

    private Vector3 startPosition;

    void Start()
    {
        // Record the starting position of the GameObject to base the floating effect on
        startPosition = transform.position;
    }

    void Update()
    {
        if (sphereController.IsLiftedOnce) return;
        float newY = startPosition.y + amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}