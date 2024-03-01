using UnityEngine;

public class TrailGrowTowardsMouse : MonoBehaviour
{
    // Speed at which the trail grows towards the mouse click position
    public float growSpeed = 5f;

    // Number of points to use for interpolation between start and target position
    public int interpolationPoints = 10;

    // Maximum deviation from the straight line between start and target position
    public float maxDeviation = 0.1f;

    // Threshold for smoothing the transition between interpolation points
    public float smoothingThreshold = 0.001f;

    // Reference to the Trail Renderer component
    private TrailRenderer trailRenderer;

    // Target position for the trail to grow towards
    private Vector3 targetPosition;

    // List of interpolation points between start and target position
    private Vector3[] interpolationPositions;

    // Index of the current interpolation point
    private int currentInterpolationIndex = 0;

    // Flag to indicate if the trail is currently growing
    private bool isGrowing = false;

    void Start()
    {
        // Get reference to the Trail Renderer component
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        // Check if the trail is currently growing
        if (isGrowing)
        {
            // Move the trail towards the current interpolation point
            Vector3 nextPosition = interpolationPositions[currentInterpolationIndex];
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, growSpeed * Time.deltaTime);

            // Check if the trail has reached the current interpolation point
            if (Vector3.Distance(transform.position, nextPosition) < smoothingThreshold)
            {
                currentInterpolationIndex++;

                // Check if the trail has reached the target position
                if (currentInterpolationIndex >= interpolationPositions.Length)
                {
                    isGrowing = false;
                }
            }
        }

        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Set the target position to the mouse click position
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0f; // Ensure the target position is at the same z-coordinate as the trail

            // Generate interpolation points between start and target position
            GenerateInterpolationPositions(transform.position, targetPosition, interpolationPoints);

            // Reset current interpolation index
            currentInterpolationIndex = 0;

            // Start growing the trail towards the target position
            isGrowing = true;
        }
    }

    // Generate interpolation points between start and target position with random deviations
    void GenerateInterpolationPositions(Vector3 start, Vector3 end, int points)
    {
        interpolationPositions = new Vector3[points];

        // Calculate the direction vector from start to end position
        Vector3 direction = (end - start).normalized;

        // Calculate the perpendicular vector to the direction
        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0f);

        // Generate interpolation points with random deviations
        for (int i = 0; i < points; i++)
        {
            // Calculate the position with linear interpolation
            float t = (float)i / (points - 1);
            Vector3 point = Vector3.Lerp(start, end, t);

            // Add random deviation perpendicular to the line connecting start and end points
            float deviation = Random.Range(-maxDeviation, maxDeviation);
            point += perpendicular * deviation;

            // Assign the interpolated point
            interpolationPositions[i] = point;
        }
    }
}
