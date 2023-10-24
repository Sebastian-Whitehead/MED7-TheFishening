using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float radius;
    public int number;
    public Vector3 centerPoint; // New center point
    public int id; // New id for all boids from this spawner

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < number; ++i) {
            GameObject boid = Instantiate(prefab, this.transform.position + Random.insideUnitSphere * radius, Random.rotation);
            BoidBoundingBehaviour boidBehaviour = boid.GetComponent<BoidBoundingBehaviour>();
            Boid boidComponent = boid.GetComponent<Boid>();
            if(boidBehaviour != null) {
                boidBehaviour.centerPoint = centerPoint;
            }
            if(boidComponent != null) {
                boidComponent.id = id;
            }
        }
        
    }
}


