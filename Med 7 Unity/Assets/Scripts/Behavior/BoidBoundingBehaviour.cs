using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boid))]
public class BoidBoundingBehaviour : MonoBehaviour
{
    private Boid boid;
    public float radius;
    public float boundingForce; // How hard should fish be pushed back when they reach the bounding sphere edge
    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If position is out of bounds move boids towards (0.0.0)
        if(boid.transform.position.magnitude > radius){
            boid.velocity += this.transform.position.normalized * ((radius - boid.transform.position.magnitude) * boundingForce * Time.deltaTime);
        }
    }
}
