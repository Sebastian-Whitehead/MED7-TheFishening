using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script ensures that the boid stays within a specified bounding box and avoids the floor.

[RequireComponent(typeof(Boid))]
public class BoidBoundingBehaviour : MonoBehaviour
{
    private Boid boid; // Reference to the Boid component attached to this GameObject
    public GameObject boundingBoxObject; // The GameObject representing the bounding box
    private Vector3 halfExtents; // Half the dimensions of the bounding box

    // Variables for AvoidFloor
    public float avoidFloorStrength = 5; // Factor by which the boid will avoid the floor
    public float raycastDistance = 5; // Distance of the raycast

    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>(); // Get the Boid component attached to this GameObject
        // Get the dimensions of the bounding box from the GameObject's scale
        halfExtents = boundingBoxObject.transform.localScale / 2;
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
        Vector3 localPos = boid.transform.position - boundingBoxObject.transform.position;

        // Check if the boid is outside the bounding box
        if (Mathf.Abs(localPos.x) > halfExtents.x || Mathf.Abs(localPos.y) > halfExtents.y || Mathf.Abs(localPos.z) > halfExtents.z)
        {
            // If position is out of bounds, move boids towards the center of the bounding box
            boid.velocity += (boundingBoxObject.transform.position - boid.transform.position) * Time.deltaTime;
        }

        // Call the AvoidFloor function to make the boid avoid the floor
        AvoidFloor();
    }

    void AvoidFloor()
    {
        // Cast a ray downwards from the fish
        RaycastHit hit;

        // Draw the ray in the Scene view for debugging purposes
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            // If the ray hits the sea floor
            if (hit.collider.gameObject.CompareTag("Avoid"))
            {
                // Calculate direction away from the floor
                Vector3 awayFromFloor = transform.position - hit.point;

                // Normalize the direction
                awayFromFloor = awayFromFloor.normalized;

                // Calculate distance to the floor
                float distanceToFloor = hit.distance;

                // Make avoidFloorStrength inversely proportional to distanceToFloor
                float adjustedAvoidFloorStrength = avoidFloorStrength / distanceToFloor;

                // Apply the direction to the boid's current direction
                float deltaTimeStrength = adjustedAvoidFloorStrength * Time.deltaTime;
                boid.velocity = boid.velocity + deltaTimeStrength * awayFromFloor / (deltaTimeStrength + 1);

                // Normalize the result
                boid.velocity = boid.velocity.normalized;
            }
        }
    }
}
