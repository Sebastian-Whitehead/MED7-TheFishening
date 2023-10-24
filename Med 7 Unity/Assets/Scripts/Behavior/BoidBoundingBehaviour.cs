using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boid))]
public class BoidBoundingBehaviour : MonoBehaviour
{
    private Boid boid;
    public float radius;
    public float boundingForce; // How hard should fish be pushed back when they reach the bounding sphere edge
    public Vector3 centerPoint; // New center point

    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If position is out of bounds move boids towards centerPoint
        if((boid.transform.position - centerPoint).magnitude > radius){
            boid.velocity += (centerPoint - this.transform.position).normalized * ((radius - (boid.transform.position - centerPoint).magnitude) * boundingForce * Time.deltaTime);
        }
    }
}

