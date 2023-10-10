using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> objectsToSpawn;
    public List<int> spawnQuantities;
    public float maxSpread = 10f;
    public Transform parentObject; // Reference to the empty parent object
    public int randomSeed = 0; // Seed for random number generation

    private void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        Random.InitState(randomSeed); // Initialize random number generator with the seed

        for (int i = 0; i < objectsToSpawn.Count; i++)
        {
            int quantity = Mathf.Clamp(spawnQuantities[i], 0, int.MaxValue);
            for (int j = 0; j < quantity; j++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-maxSpread, maxSpread), 2f, Random.Range(-maxSpread, maxSpread));
                Quaternion spawnRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                GameObject spawnedObject = Instantiate(objectsToSpawn[i], spawnPosition, spawnRotation);
                spawnedObject.transform.parent = parentObject; // Set the parent of the spawned object
                BoxCollider collider = spawnedObject.AddComponent<BoxCollider>();
                Rigidbody rigidbody = spawnedObject.AddComponent<Rigidbody>();
                StartCoroutine(RemoveColliderAndRigidbodyAfterDelay(collider, rigidbody, 5f));
            }
        }
    }

    private System.Collections.IEnumerator RemoveColliderAndRigidbodyAfterDelay(BoxCollider collider, Rigidbody rigidbody, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(collider);
        Destroy(rigidbody);
    }
}