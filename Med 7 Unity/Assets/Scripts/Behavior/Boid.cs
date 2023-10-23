using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Boid : MonoBehaviour
{
    // We are using physical definitions for velocity and speed.
    // Velocity is a vector and therefore it has magnitude and direction
    // Speed is a scalar and is equal to the magnitude of the velocity
    public Vector3 velocity;
    public float maxVelocity;
    public float rotSpeed = 1;
    public int id;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocity = this.transform.forward * maxVelocity;
    }

    void FixedUpdate()
    {
        // Clamp maximum velocity to a set cap to prevent infinite acceleration
        if (velocity.magnitude > maxVelocity) {
            velocity = velocity.normalized * maxVelocity;
        }
        
        Vector3 newPosition = transform.position + velocity * Time.fixedDeltaTime; // Move X (velocity) units every second
        rb.MovePosition(newPosition);
        
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(velocity), Time.fixedDeltaTime * rotSpeed); //Move the way it's looking
        rb.MoveRotation(newRotation);
    }
}
