using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Boid))]

public class BoidAlignmentBehaviour : MonoBehaviour
{
    private Boid _boid;

    [Header("Alignment Settings")]
    [Tooltip("The radius within which boids will align")]
    public float radius;
    
    // Start is called before the first frame update
    void Start()
    {
        _boid = GetComponent<Boid>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // This finds every boid for every frame for every scene, and will probably be on multiple components. NOT OPTIMAL!
        List<Boid> boids = new List<Boid>(FindObjectsOfType<Boid>());
        var average = Vector3.zero;
        var found = 0;

        // Prune boids list of all fish of different id
        boids.RemoveAll(boid => boid.id != _boid.id);

        // Find additional vector (to other objects) add the difference, the normal of difference is direction of movement
        foreach(var boid in boids.Where(b => b != _boid)) {
            var diff = boid.transform.position - this.transform.position;
            if (diff.magnitude < radius) {
                average += boid.velocity; // Not diff like in 'Cohesion' and 'Separation'
                found += 1;
            }
        }

        // Try to match average velocity
        if (found > 0) {
            average = average / found;
            _boid.velocity += Vector3.Lerp(_boid.velocity, average, Time.deltaTime); // Might not be mathematically correct!
        }
    }
}
