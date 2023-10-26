using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boid))]
public class BoidBoundingBehaviour : MonoBehaviour
{
    private Boid boid;
    public GameObject boundingBoxObject; // The GameObject representing the bounding box
    private Vector3 halfExtents; // Half the dimensions of the bounding box

    // Variables for AvoidFloor
    public float avoidFloorStrength = 5; /* factor by which fish will avoid the floor */
    public float raycastDistance = 2; /* distance of the raycast */

    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
        // Get the dimensions of the bounding box from the GameObject's scale
        halfExtents = boundingBoxObject.transform.localScale / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 localPos = boid.transform.position - boundingBoxObject.transform.position;
        if(Mathf.Abs(localPos.x) > halfExtents.x || Mathf.Abs(localPos.y) > halfExtents.y || Mathf.Abs(localPos.z) > halfExtents.z){
            // If position is out of bounds, move boids towards the center of the bounding box
            boid.velocity += (boundingBoxObject.transform.position - boid.transform.position) * Time.deltaTime;
        }

        // Call AvoidFloor
        AvoidFloor();
    }

    void AvoidFloor() {
        // Cast a ray downwards from the fish
        RaycastHit hit;

         // Draw the ray in the Scene view
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance)) {
            // If the ray hits the sea floor
            if (hit.collider.gameObject.CompareTag("Avoid")) {
                // Calculate direction away from the floor
                Vector3 awayFromFloor = transform.position - hit.point;

                // Normalize the direction
                awayFromFloor = awayFromFloor.normalized;

                // Calculate distance to the floor
                float distanceToFloor = hit.distance;

                // Make avoidFloorStrength inversely proportional to distanceToFloor
                float adjustedAvoidFloorStrength = avoidFloorStrength / distanceToFloor;

                // Apply the direction to the fish's current direction
                float deltaTimeStrength = adjustedAvoidFloorStrength * Time.deltaTime;
                boid.velocity = boid.velocity + deltaTimeStrength * awayFromFloor / (deltaTimeStrength + 1);

                // Normalize the result
                boid.velocity = boid.velocity.normalized;
            }
        }
    }
}