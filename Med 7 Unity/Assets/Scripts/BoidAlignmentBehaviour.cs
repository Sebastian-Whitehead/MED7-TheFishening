using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Boid))]

public class BoidAlignmentBehaviour : MonoBehaviour
{
    private Boid boid;

    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
    }

    // Update is called once per frame
    void Update()
    {
        // This finds every boid for every frame for every scene, and will probably be on multiple components. NOT OPTIMAL!
        var boids = FindObjectsOfType<Boid>();
        var average = Vector3.zero;
        var found = 0;

        // Find additional vector (to other objects) add the difference, the normal of difference is direction of movement
        foreach(var boid in boids.Where(b => b != boid)) {
            var diff = boid.transform.position - this.transform.position;
            if (diff.magnitude < radius) {
                average += boid.velocity; // Not diff like in 'Cohesion' and 'Separation'
                found += 1;
            }
        }

        // Try to match average velocity
        if (found > 0) {
            average = average / found;
            boid.velocity += Vector3.Lerp(boid.velocity, average, Time.deltaTime); // Might not be mathematically correct!
        }
    }
}
