using UnityEngine;

public class RandomDeformer : MonoBehaviour
{
    public float deformAmount = 0.5f;
    public float randomSeed = 0f;

    private MeshFilter meshFilter;
    private Vector3[] originalVertices;
    private Vector3[] deformedVertices;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        originalVertices = meshFilter.mesh.vertices;
        deformedVertices = new Vector3[originalVertices.Length];
    }

    private void Start()
    {
        Random.InitState((int)randomSeed);
        DeformPlane();
        UpdateMeshCollider();
    }

    private void DeformPlane()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 originalVertex = originalVertices[i];
            Vector3 deformedVertex = originalVertex + Random.insideUnitSphere * deformAmount;
            deformedVertices[i] = deformedVertex;
        }

        meshFilter.mesh.vertices = deformedVertices;
        meshFilter.mesh.RecalculateNormals();
    }

    private void UpdateMeshCollider()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = meshFilter.mesh;
        }
    }
}