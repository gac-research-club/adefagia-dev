using UnityEngine;

namespace Playground
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class GenerateQuads : MonoBehaviour
    {
        public int xSize, ySize;
        public float tileSize;

        private Vector3[] vertices;
        private Mesh mesh;

        private void Awake()
        {
            // StartCoroutine(Generate());
            Generate();
        }

        void Generate()
        {
            // WaitForSeconds wait = new WaitForSeconds(0.1f);

            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Mesh";
        
            // Membuat vertex
            vertices = new Vector3[((xSize + 1) * (ySize + 1))];

            for (int i = 0, y = 0; y <= ySize; y++)
            {
                for (int x = 0; x <= xSize; x++, i++)
                {
                    var half = tileSize * 0.5f;
                    vertices[i] = new Vector3(x * tileSize - half, 0, y * tileSize - half);


                    // yield return wait;
                }
            }
        
            mesh.vertices = vertices;
        
            // Membuat Triangle
            int[] triangles = new int[xSize * ySize * 6];
        
            // ti = triangle index
            // vi = vertex index
            for (int y = 0, vi = 0, ti = 0; y < ySize; y++, vi++)
            {
                for (int x = 0; x < xSize; x++, vi++, ti += 6)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;

                    mesh.triangles = triangles;
                    // yield return wait;
                }
            }
        
            mesh.RecalculateNormals();

            GetComponent<MeshCollider>().sharedMesh = mesh;

        }

        // private void OnDrawGizmos()
        // {
        //     if (vertices.IsUnityNull()) return;
        //
        //     Gizmos.color = Color.black;
        //     foreach (var vertex in vertices)
        //     {
        //         Gizmos.DrawSphere(vertex, 0.1f);
        //     }
        // }
    }
}
