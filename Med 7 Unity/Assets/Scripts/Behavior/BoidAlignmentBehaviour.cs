using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Boid))]

public class BoidAlignmentBehaviour : MonoBehaviour
{
    private Boid _boid;
    private List<Boid> _boids;

    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        _boid = GetComponent<Boid>();
        _boids = new List<Boid>(FindObjectsOfType<Boid>());
        
        // Prune boids list of all fish of different id
        _boids.RemoveAll(boid => boid.id != _boid.id);
        
        print(_boid.id + " : ");
        foreach (var boid in _boids)
        {
            print("    " + boid.id);
        }
    }

    // Update is called once per frame
    void Update()
    { 
        var average = Vector3.zero;
        var found = 0;

        // Find additional vector (to other objects) add the difference, the normal of difference is direction of movement
        foreach(var boid in _boids.Where(b => b != _boid)) {
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