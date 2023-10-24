using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float radius;
    public int number;
    public GameObject boundingBoxObject; // The GameObject representing the bounding box
    public int id; // The id for all boids from this spawner

    void Start()
    {
        for(int i = 0; i < number; ++i) {
            GameObject boid = Instantiate(prefab, this.transform.position + Random.insideUnitSphere * radius, Random.rotation);
            BoidBoundingBehaviour boidBehaviour = boid.GetComponent<BoidBoundingBehaviour>();
            if(boidBehaviour != null) {
                boidBehaviour.boundingBoxObject = boundingBoxObject;
            }
            
            Boid boidComponent = boid.GetComponent<Boid>();
            if(boidComponent != null) {
                boidComponent.id = id;
            }
        }
    }
}



