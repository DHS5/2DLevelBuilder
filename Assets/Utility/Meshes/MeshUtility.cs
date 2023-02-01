using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dhs5.Utility.Meshes
{
    public static class MeshUtility
    {
        /// <summary>
        /// Creates a mesh from a PolygonCollider2D using raycasts intersecting with 3D colliders
        /// </summary>
        /// <param name="collider2D">The PolygonCollider2D to create the mesh from</param>
        /// <param name="activeCamera">The currently active scene camera</param>
        /// <param name="layerMask">LayerMask to select with which layer of 3D colliders the raycasts will interact</param>
        /// <param name="raycastMaxDist">Max distance of the raycast rays</param>
        /// <returns>The created mesh</returns>
        public static Mesh CreateMeshFrom2DPoly(PolygonCollider2D collider2D,
            Camera activeCamera, LayerMask layerMask, float raycastMaxDist = 100, float meshThickness = 1)
        {
            Vector2[] poly = collider2D.points;
            float zPos = collider2D.transform.position.z;
            Vector3 worldSpacePos;
            Vector3 cameraPos = activeCamera.transform.position;

            Triangulator triangulator = new Triangulator(poly);
            int[] tris = triangulator.Triangulate();
            Mesh m = new Mesh();
            Vector3[] vertices = new Vector3[poly.Length * 2];

            for (int i = 0; i < poly.Length; i++)
            {
                worldSpacePos = collider2D.transform.TransformPoint(poly[i]);
                Ray ray = new Ray(cameraPos, new Vector3(worldSpacePos.x, worldSpacePos.y, zPos) - cameraPos);
                if (Physics.Raycast(ray, out RaycastHit hit, raycastMaxDist, layerMask, QueryTriggerInteraction.Collide))
                {
                    vertices[i] = hit.point;
                    vertices[i + poly.Length] = hit.point + Vector3.down * meshThickness;
                }
                else
                {
                    Debug.LogError("A vertex is missing");
                }
            }
            int[] triangles = new int[tris.Length * 2 + poly.Length * 6];
            int count_tris = 0;
            for (int i = 0; i < tris.Length; i += 3)
            {
                triangles[i] = tris[i];
                triangles[i + 1] = tris[i + 1];
                triangles[i + 2] = tris[i + 2];
            } // front vertices
            count_tris += tris.Length;
            for (int i = 0; i < tris.Length; i += 3)
            {
                triangles[count_tris + i] = tris[i + 2] + poly.Length;
                triangles[count_tris + i + 1] = tris[i + 1] + poly.Length;
                triangles[count_tris + i + 2] = tris[i] + poly.Length;
            } // back vertices
            count_tris += tris.Length;
            for (int i = 0; i < poly.Length; i++)
            {
                // triangles around the perimeter of the object
                int n = (i + 1) % poly.Length;
                triangles[count_tris] = i;
                triangles[count_tris + 1] = n;
                triangles[count_tris + 2] = i + poly.Length;
                triangles[count_tris + 3] = n;
                triangles[count_tris + 4] = n + poly.Length;
                triangles[count_tris + 5] = i + poly.Length;
                count_tris += 6;
            }
            m.vertices = vertices;
            m.triangles = triangles;
            m.RecalculateNormals();
            m.RecalculateBounds();
            m.Optimize();

            return m;
        }
    }
}
