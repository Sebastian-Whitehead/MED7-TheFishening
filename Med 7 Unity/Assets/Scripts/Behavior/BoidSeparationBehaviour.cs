using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Boid))]

public class BoidSeparationBehaviour : MonoBehaviour
{
    private Boid boid;

    [Header("Separation Settings")]
    [Tooltip("The radius within which boids will be separated. This should probably be smaller than the radius used by 'Cohesion' and 'Alignment'")]
    public float radius;

    [Tooltip("The force with which boids repel each other")]
    public float repulsionForce;

    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // This finds every boid for every frame for every scene, and will probably be on multiple components. NOT OPTIMAL!
        var boids = FindObjectsOfType<Boid>();
        var average = Vector3.zero;
        var found = 0;

        // Find additional vector (to other objects) add the difference, the normal of difference is direction of movement
        foreach(var boid in boids.Where(b => b != boid)) {
            var diff = boid.transform.position - this.transform.position;
            if (diff.magnitude < radius) {
                average += diff;
                found += 1;
            }
        }

        // If the average is very far away we want to move with more velocity or if objects around are close it moves with less
        if (found > 0) {
            average = average / found;
            boid.velocity -= Vector3.Lerp(Vector3.zero, average, average.magnitude / radius) * repulsionForce; // Same as 'Cohesion' but opposite
        }
    }
}
