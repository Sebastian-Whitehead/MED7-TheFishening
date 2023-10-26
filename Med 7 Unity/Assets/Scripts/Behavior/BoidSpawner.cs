using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("The prefab to be spawned")]
    public GameObject prefab;

    [Tooltip("The radius within which the boids will be spawned")]
    public float radius;

    [Tooltip("The number of boids to spawn")]
    public int number;

    [Header("Passed Variables")]
    [Tooltip("The GameObject representing the bounding box")]
    public GameObject boundingBoxObject;

    [Tooltip("The id for all boids from this spawner")]
    public int id;

    [Header("Random Scaling Settings")]
    [Tooltip("The minimum scale factor for the boids")]
    [Range(0.1f, 2f)]
    public float minScale = 0.8f;

    [Tooltip("The maximum scale factor for the boids")]
    [Range(0.1f, 2f)]
    public float maxScale = 1.2f;

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

            // Scale the boid randomly within the specified range while maintaining original proportions
            float scale = Random.Range(minScale, maxScale);
            Vector3 originalScale = boid.transform.localScale;
            boid.transform.localScale = new Vector3(originalScale.x * scale, originalScale.y * scale, originalScale.z * scale);
        }
    }
}