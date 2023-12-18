using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boid))]
public class BoidBoundingBehaviour : MonoBehaviour
{
    private Boid boid;

    [Header("Bounding Settings")]
    [Tooltip("The GameObject representing the bounding box")]
    public GameObject boundingBoxObject;

    private Vector3 halfExtents;

    [Header("Avoid Floor Settings")]
    [Tooltip("Factor by which the boid will avoid the floor")]
    public float avoidFloorStrength;

    [Tooltip("Distance of the raycast used to detect the floor")]
    public float raycastDistance;
    
    void Start()
    {
        boid = GetComponent<Boid>();
        halfExtents = boundingBoxObject.transform.localScale / 2;
    }

    void FixedUpdate()
    {
        Vector3 localPos = boid.transform.position - boundingBoxObject.transform.position;

        if (Mathf.Abs(localPos.x) > halfExtents.x || Mathf.Abs(localPos.y) > halfExtents.y || Mathf.Abs(localPos.z) > halfExtents.z)
        {
            boid.velocity += (boundingBoxObject.transform.position - boid.transform.position) * Time.deltaTime;
        }

        AvoidFloor();
    }

    void AvoidFloor()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.CompareTag("Avoid"))
            {
                Vector3 awayFromFloor = transform.position - hit.point;
                awayFromFloor = awayFromFloor.normalized;

                float distanceToFloor = hit.distance;

                // Use a function of distanceToFloor to adjust avoidFloorStrength
                float adjustedAvoidFloorStrength = avoidFloorStrength * Mathf.Pow(distanceToFloor, 2);

                float deltaTimeStrength = adjustedAvoidFloorStrength * Time.deltaTime;
                boid.velocity = boid.velocity + deltaTimeStrength * awayFromFloor / (deltaTimeStrength + 1);

                boid.velocity = boid.velocity.normalized;
            }
        }
    }

}
