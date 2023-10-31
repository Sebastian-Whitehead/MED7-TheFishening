using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoidAlignmentBehaviour))]
[RequireComponent(typeof(BoidBoundingBehaviour))]
[RequireComponent(typeof(BoidCohesionBehaviour))]
[RequireComponent(typeof(BoidSeparationBehaviour))]
[RequireComponent(typeof(Rigidbody))]

public class Boid : MonoBehaviour
{
    [HideInInspector] public Vector3 velocity; // Current velocity of the boid

    [Header("Boid Settings")]
    [Tooltip("Minimum velocity magnitude")]
    public float minVelocity;

    [Tooltip("Maximum velocity magnitude")]
    public float maxVelocity;
    public float slope = 0.1f;

    [Tooltip("Rotation speed")]
    public float rotSpeed = 1;

    [Tooltip("Unique identifier for the boid")]
    public int id;

    private Rigidbody rb;          

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component attached to this GameObject
        velocity = new Vector3(Random.Range(minVelocity, maxVelocity), 0, Random.Range(minVelocity, maxVelocity));

    }

    void FixedUpdate()
    {
        float randomValue = Random.Range(-0.1f, 0.1f); // Generate a small random value

        // Get the current speed based on the adjusted velocity
        float speed = velocity.magnitude; 

        speed += slope * randomValue; // Adjust the speed

        // Ensure the speed stays within a certain range (e.g., minVelocity to maxVelocity)
        speed = Mathf.Clamp(speed, minVelocity, maxVelocity);

        // Maintain the same direction but adjust the speed
        float lerpSpeed = 10.0f; // Adjust this value to control the lerp speed
        velocity = velocity.normalized * Mathf.Lerp(velocity.magnitude, speed, lerpSpeed);

        Vector3 newPosition = transform.position + velocity * Time.fixedDeltaTime; 
        rb.MovePosition(newPosition);  // Move the boid to the new position

        Quaternion newRotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(velocity), Time.fixedDeltaTime * rotSpeed);
        rb.MoveRotation(newRotation);  // Set the new rotation for the boid
    }


}