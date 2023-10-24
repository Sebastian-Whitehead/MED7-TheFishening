using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    [HideInInspector] public Vector3 velocity;        // Current velocity of the boid
    public float minVelocity;       // Minimum velocity magnitude
    public float maxVelocity;       // Maximum velocity magnitude
    public float rotSpeed = 1;     // Rotation speed
    public int id;                 // Unique identifier for the boid
    private Rigidbody rb;          

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component attached to this GameObject
        velocity = new Vector3(Random.Range(minVelocity, maxVelocity), 0, Random.Range(minVelocity, maxVelocity));

    }

    void FixedUpdate()
    {
        // Interpolate velocity magnitude randomly between minVelocity and maxVelocity
        float newSpeed = Mathf.Lerp(minVelocity, maxVelocity, Random.value);
        velocity = velocity.normalized * newSpeed;  // Set the new velocity magnitude

        // Calculate the new position based on the current position, velocity, and time
        Vector3 newPosition = transform.position + velocity * Time.fixedDeltaTime; 
        rb.MovePosition(newPosition);  // Move the boid to the new position

        // Calculate the new rotation by smoothly interpolating between the current rotation and the direction of velocity
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(velocity), Time.fixedDeltaTime * rotSpeed);
        rb.MoveRotation(newRotation);  // Set the new rotation for the boid
    }
}