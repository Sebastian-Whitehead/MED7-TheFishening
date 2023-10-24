using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boid))]
public class BoidBoundingBehaviour : MonoBehaviour
{
    private Boid boid;
    public GameObject boundingBoxObject; // The GameObject representing the bounding box
    private Vector3 halfExtents; // Half the dimensions of the bounding box

    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
        // Get the dimensions of the bounding box from the GameObject's scale
        halfExtents = boundingBoxObject.transform.localScale / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 localPos = boid.transform.position - boundingBoxObject.transform.position;
        if(Mathf.Abs(localPos.x) > halfExtents.x || Mathf.Abs(localPos.y) > halfExtents.y || Mathf.Abs(localPos.z) > halfExtents.z){
            // If position is out of bounds, move boids towards the center of the bounding box
            boid.velocity += (boundingBoxObject.transform.position - boid.transform.position) * Time.deltaTime;
        }
    }
}


