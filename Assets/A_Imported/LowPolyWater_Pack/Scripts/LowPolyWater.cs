using UnityEngine;

namespace LowPolyWater
{
    public class LowPolyWater : MonoBehaviour
    {
        public float waveHeight = 0.5f;
        public float waveFrequency = 0.5f;
        public float waveLength = 0.75f;

        MeshFilter meshFilter;
        Mesh mesh;
        Vector3[] vertices;

        private void Awake()
        {
            // Get the Mesh Filter of the gameobject
            meshFilter = GetComponent<MeshFilter>();
        }

        void Start()
        {
            CreateMeshLowPoly(meshFilter);
        }

        /// <summary>
        /// Rearranges the mesh vertices to create a 'low poly' effect
        /// </summary>
        /// <param name="mf">Mesh filter of gamobject</param>
        /// <returns></returns>
        MeshFilter CreateMeshLowPoly(MeshFilter mf)
        {
            mesh = mf.sharedMesh;

            // Get the original vertices of the gameobject's mesh
            Vector3[] originalVertices = mesh.vertices;

            // Get the list of triangle indices of the gameobject's mesh
            int[] triangles = mesh.triangles;

            // Create a vector array for new vertices 
            vertices = new Vector3[triangles.Length];

            // Assign vertices to create triangles out of the mesh
            for (int i = 0; i < triangles.Length; i++)
            {
                vertices[i] = originalVertices[triangles[i]];
                triangles[i] = i;
            }

            // Update the gameobject's mesh with new vertices
            mesh.vertices = vertices;
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            this.vertices = mesh.vertices;

            return mf;
        }

        void Update()
        {
            // Generate waves using instance-specific wave parameters
            GenerateWaves();
        }

        /// <summary>
        /// Based on the specified wave height and frequency, generate 
        /// wave motion originating from waveOriginPosition
        /// </summary>
        void GenerateWaves()
        {
            // Calculate instance-specific wave phase using local time
            float wavePhase = Time.time * waveFrequency;

            // Get a reference to the original vertices
            Vector3[] originalVertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                // Convert the vertex position to world position
                Vector3 worldPos = transform.TransformPoint(originalVertices[i]);

                // Use Perlin noise to generate wave motion using world positions
                float noiseValue = Mathf.PerlinNoise((worldPos.x + wavePhase) * waveLength, (worldPos.y + wavePhase) * waveLength);
                worldPos.z = waveHeight * noiseValue + .6f;

                // Convert back to local position and update the vertex
                vertices[i] = transform.InverseTransformPoint(worldPos);
            }

            // Update the mesh properties
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.MarkDynamic();
            meshFilter.mesh = mesh;
        }
    }
}
