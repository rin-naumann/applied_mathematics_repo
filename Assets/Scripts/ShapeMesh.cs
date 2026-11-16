using System.Collections.Generic;
using UnityEngine;

public enum ShapeType { Cube, Pyramid, Sphere }
public class ShapeMesh : MonoBehaviour
{
    [Header("Shape")]
    [SerializeField] private ShapeType shape = ShapeType.Cube;
    [Range(6, 32)] [SerializeField] private int sphereSegments = 8; // must be > 5

    [Header("Look")]
    [SerializeField] private Material lineMat;
    [SerializeField] private Vector3 position = Vector3.zero;
    [SerializeField] private float scale = 1;
    [Range(1, 500)] [SerializeField] private float focalLength = 20f;

    [Header("Rotation (degrees)")]
    [SerializeField] private float xRotation = 0;
    [SerializeField] private float yRotation = 0;
    [SerializeField] private float xSpeed = 40f;  
    [SerializeField] private float ySpeed = 60f;  

    private Vector3[] verts;
    private int[][] faces;
    private readonly List<Vector3> projected = new();
    private readonly List<int> visibleEdges = new();
    private readonly HashSet<long> drawnEdges = new();

    private Mesh mesh;

    #region Unity Functions

    private void Awake()
    {
        switch (shape)
        {
            case ShapeType.Cube: BuildCube(); break;
            case ShapeType.Pyramid: BuildPyramid(); break;
            case ShapeType.Sphere: BuildSphere(); break;
        }

        OrientFacesOutward();

        mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = lineMat;
    }

    private void LateUpdate()
    {
        xRotation = (xRotation + xSpeed * Time.deltaTime) % 360f;
        yRotation = (yRotation + ySpeed * Time.deltaTime) % 360f;

        float sx = Mathf.Sin(xRotation * Mathf.Deg2Rad), cx = Mathf.Cos(xRotation * Mathf.Deg2Rad);
        float sy = Mathf.Sin(yRotation * Mathf.Deg2Rad), cy = Mathf.Cos(yRotation * Mathf.Deg2Rad);

        projected.Clear();
        foreach (var v in verts)
            projected.Add(Project(v, sx, cx, sy, cy));

        mesh.vertices = projected.ToArray();

        visibleEdges.Clear();
        drawnEdges.Clear();
        foreach (var f in faces)
        {
            if (!FacesCamera(f)) continue;

            for (int i = 0; i < f.Length; i++)
            {
                int a = f[i];
                int b = f[(i + 1) % f.Length];

                long key = ((long)Mathf.Min(a, b) << 32) | (uint)Mathf.Max(a, b);
                if (!drawnEdges.Add(key)) continue;

                visibleEdges.Add(a);
                visibleEdges.Add(b);
            }
        }

        mesh.SetIndices(visibleEdges, MeshTopology.Lines, 0);
        mesh.RecalculateBounds();
    }

    #endregion

    #region Shape builders

    private void BuildCube()
    {
        verts = new Vector3[]
        {
            new(-1, -1, -1), new(1, -1, -1), new(1, 1, -1), new(-1, 1, -1), 
            new(-1, -1, 1),  new(1, -1, 1),  new(1, 1, 1),  new(-1, 1, 1) 
        };

        faces = new[]
        {
            new[] { 0, 1, 2, 3 }, new[] { 5, 4, 7, 6 }, new[] { 4, 0, 3, 7 },
            new[] { 1, 5, 6, 2 }, new[] { 3, 2, 6, 7 }, new[] { 0, 4, 5, 1 }
        };
    }

    private void BuildPyramid()
    {
        verts = new Vector3[]
        {
            new(-1, -1, -1), new(1, -1, -1), new(1, -1, 1), new(-1, -1, 1),
            new(0, 1, 0)
        };

        faces = new[]
        {
            new[] { 0, 1, 2, 3 },
            new[] { 0, 1, 4 }, new[] { 1, 2, 4 },
            new[] { 2, 3, 4 }, new[] { 3, 0, 4 }
        };
    }

    private void BuildSphere()
    {
        int lat = sphereSegments;
        int lon = sphereSegments;

        verts = new Vector3[2 + (lat - 1) * lon];
        faces = new int[2 * lon + (lat - 2) * lon][];

        verts[0] = Vector3.up;
        verts[1] = Vector3.down;

        for (int i = 1; i < lat; i++)
        for (int j = 0; j < lon; j++)
        {
            float phi = Mathf.PI * i / lat;
            float theta = 2f * Mathf.PI * j / lon;

            verts[2 + (i - 1) * lon + j] = new Vector3(
                Mathf.Sin(phi) * Mathf.Cos(theta),
                Mathf.Cos(phi),
                Mathf.Sin(phi) * Mathf.Sin(theta));
        }

        int face = 0;

        int topRing = 2;
        int bottomRing = 2 + (lat - 2) * lon;
        for (int j = 0; j < lon; j++)
        {
            int next = (j + 1) % lon;
            faces[face++] = new[] { 0, topRing + j, topRing + next };
            faces[face++] = new[] { 1, bottomRing + next, bottomRing + j };
        }

        for (int i = 1; i < lat - 1; i++)
        {
            int upper = 2 + (i - 1) * lon;
            int lower = upper + lon;
            for (int j = 0; j < lon; j++)
            {
                int next = (j + 1) % lon;
                faces[face++] = new[] { upper + j, upper + next, lower + next, lower + j };
            }
        }
    }

    private void OrientFacesOutward()
    {
        foreach (var f in faces)
        {
            var normal = Vector3.Cross(verts[f[1]] - verts[f[0]], verts[f[2]] - verts[f[0]]);

            var center = Vector3.zero;
            foreach (var i in f) center += verts[i];
            center /= f.Length;

            if (Vector3.Dot(normal, center) > 0)
                System.Array.Reverse(f);
        }
    }

    #endregion

    #region Helper

    private Vector3 Project(Vector3 point, float sx, float cx, float sy, float cy)
    {
        point *= .5f;

        float y1 = point.y * cx - point.z * sx;
        float z1 = point.y * sx + point.z * cx;
        float x1 = point.x;

        float x2 = x1 * cy - z1 * sy;
        float z2 = x1 * sy + z1 * cy;

        var p3 = new Vector3(x2 + position.x, y1 + position.y, z2 + position.z);

        float p = focalLength / (focalLength + p3.z);

        return new Vector3(p3.x, p3.y, 0) * scale * p;
    }

    private bool FacesCamera(int[] f)
    {
        float area = 0;
        for (int i = 0; i < f.Length; i++)
        {
            var a = projected[f[i]];
            var b = projected[f[(i + 1) % f.Length]];
            area += a.x * b.y - b.x * a.y;
        }

        return area > 0;
    }

    #endregion
}