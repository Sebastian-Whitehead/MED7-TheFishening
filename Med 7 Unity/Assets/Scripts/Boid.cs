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
    // Start is called before the first frame update
    void Start()
    {
        velocity = this.transform.forward * maxVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        // Clamp maximum velocity to a set cap to prevent infinite acceleration
        if (velocity.magnitude > maxVelocity) {
            velocity = velocity.normalized * maxVelocity;
        }

        this.transform.position += velocity * Time.deltaTime; // Move X (velocity) units every second
        this.transform.rotation = Quaternion.LookRotation(velocity); //Move the way it's looking
    }
}
