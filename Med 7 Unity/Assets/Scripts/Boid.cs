using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Boid : MonoBehaviour
{
    public Vector3 velocity;
    public float maxVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
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
