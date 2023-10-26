using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("List of game objects to spawn")]
    public List<GameObject> objectsToSpawn;

    [Tooltip("List of quantities for each object to spawn")]
    public List<int> spawnQuantities;

    [Tooltip("Maximum spread for object spawning")]
    public float maxSpread;

    [Tooltip("Reference to the empty parent object")]
    public Transform parentObject;

    [Tooltip("Seed for random number generation")]
    public int randomSeed = 0;

    private void Start()
    {
        SpawnObjects(); // Call the method to spawn objects
    }

    private void SpawnObjects()
    {
        Random.InitState(randomSeed); // Initialize random number generator with the seed

        for (int i = 0; i < objectsToSpawn.Count; i++)
        {
            int quantity = Mathf.Clamp(spawnQuantities[i], 0, int.MaxValue); // Get the quantity to spawn for the current object
            for (int j = 0; j < quantity; j++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-maxSpread, maxSpread), 2f, Random.Range(-maxSpread, maxSpread)); // Calculate a random spawn position
                Quaternion spawnRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)); // Calculate a random spawn rotation
                GameObject spawnedObject = Instantiate(objectsToSpawn[i], spawnPosition, spawnRotation); // Instantiate the object at the spawn position and rotation
                spawnedObject.transform.parent = parentObject; // Set the parent of the spawned object to the specified parent object
                BoxCollider collider = spawnedObject.AddComponent<BoxCollider>(); // Add a box collider to the spawned object
                Rigidbody rigidbody = spawnedObject.AddComponent<Rigidbody>(); // Add a rigidbody to the spawned object
                StartCoroutine(RemoveColliderAndRigidbodyAfterDelay(collider, rigidbody, 5f)); // Start a coroutine to remove the collider and rigidbody after a delay
            }
        }
    }

    private System.Collections.IEnumerator RemoveColliderAndRigidbodyAfterDelay(BoxCollider collider, Rigidbody rigidbody, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        Destroy(collider); // Destroy the collider component
        Destroy(rigidbody); // Destroy the rigidbody component
    }
}