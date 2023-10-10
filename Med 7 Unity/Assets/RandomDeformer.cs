using UnityEngine;

public class RandomDeformer : MonoBehaviour
{
    public float deformAmount = 0.5f; // Amount of deformation to apply
    public float randomSeed = 0f; // Seed for random number generation

    private MeshFilter meshFilter; // Reference to the MeshFilter component
    private Vector3[] originalVertices; // Array to store the original vertices of the mesh
    private Vector3[] deformedVertices; // Array to store the deformed vertices of the mesh

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>(); // Get the MeshFilter component attached to the same GameObject
        originalVertices = meshFilter.mesh.vertices; // Store the original vertices of the mesh
        deformedVertices = new Vector3[originalVertices.Length]; // Create a new array to store the deformed vertices
    }

    private void Start()
    {
        Random.InitState((int)randomSeed); // Initialize random number generator with the seed
        DeformPlane(); // Call the method to deform the plane
        UpdateMeshCollider(); // Call the method to update the MeshCollider
    }

    private void DeformPlane()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 originalVertex = originalVertices[i]; // Get the original vertex position
            Vector3 deformedVertex = originalVertex + Random.insideUnitSphere * deformAmount; // Deform the vertex position by adding a random displacement
            deformedVertices[i] = deformedVertex; // Store the deformed vertex position
        }

        meshFilter.mesh.vertices = deformedVertices; // Update the mesh vertices with the deformed vertices
        meshFilter.mesh.RecalculateNormals(); // Recalculate the vertex normals to ensure proper shading
    }

    private void UpdateMeshCollider()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>(); // Get the MeshCollider component attached to the same GameObject
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = meshFilter.mesh; // Update the shared mesh of the MeshCollider with the deformed mesh
        }
    }
}