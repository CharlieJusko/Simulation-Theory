using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateSimulationSides : MonoBehaviour
{
    public enum SideDirection {
        X,
        Z
    }

    public LayerMask layersForCrossSection;
    public int edgeCount = 150;
    public SideDirection sideDirection;

    public MeshFilter meshFilter;

    private Mesh mesh;
    [SerializeField]
    private Vector3[] vertices;
    [SerializeField]
    private int[] triangles;
    private int triangleIndex = 0;

    [SerializeField]
    private Vector3[] generatedTopVertecies;
    [SerializeField]
    private Vector3[] generatedBottomVertices;


    private void Awake() {
        generatedTopVertecies = new Vector3[edgeCount];
        generatedBottomVertices = new Vector3[edgeCount];
    }

    private void Update() {
        RaycastVertexGeneration();
        GenerateSideMesh();
    }

    void RaycastVertexGeneration() {
        for(int i = 0; i < edgeCount; ++i) {
            generatedBottomVertices[i] = GetRaycastPosition(i);
            generatedBottomVertices[i].y = transform.position.y + (transform.localScale.y / 2);

            Vector3 raycastPos = GetRaycastPosition(i);
            if(Physics.Raycast(raycastPos, Vector3.down, out RaycastHit hit, 10000f, layersForCrossSection)) {
                generatedTopVertecies[i] = hit.point;

            } else {
                Vector3 defaultVertexPos = raycastPos;
                defaultVertexPos.y = (transform.position.y + (transform.localScale.y / 2)) + 1f;
                generatedTopVertecies[i] = defaultVertexPos;
            }
        }
    }

    Vector3 GetRaycastPosition(int currentVertex) {
        float sideScale;
        float sidePosition;
        float offsideConstant;
        if(sideDirection == SideDirection.X) {
            sideScale = transform.localScale.x;
            sidePosition = transform.position.x;
            offsideConstant = (transform.position.z - (transform.localScale.z / 2));
        } else {
            sideScale = transform.localScale.z;
            sidePosition = transform.position.z;
            offsideConstant = (transform.position.x + (transform.localScale.x / 2));
        }

        float lowestSideAmount = sidePosition - (sideScale / 2);

        float offset = lowestSideAmount + (currentVertex * (sideScale / edgeCount));
        float height = transform.position.y + (transform.localScale.y / 2) + 500;

        if(sideDirection == SideDirection.X) {
            return new Vector3(offset, height, offsideConstant);
        } else {
            return new Vector3(offsideConstant, height, offset);
        }
    }

    void GenerateSideMesh() {
        vertices = new Vector3[edgeCount * 2];
        triangles = new int[(edgeCount - 1) * 6];
        triangleIndex = 0;

        int vertIndex = 0;
        for(int i = 0; i < edgeCount; i++) {
            vertices[vertIndex] = generatedBottomVertices[i];
            vertices[vertIndex + 1] = generatedTopVertecies[i];
            vertIndex += 2;
        }

        PopulateTriangles(ref triangles, ref triangleIndex);
        meshFilter.sharedMesh = GenerateMesh(ref mesh, ref vertices, ref triangles);
    }

    public void PopulateTriangles(ref int[] tris, ref int index) {
        for(int i = 0; i < (edgeCount - 1) * 2; ++i) {
            AddTriangle(i, i + 1, i + 2, ref tris, ref index);
            AddTriangle(i + 2, i + 1, i + 3, ref tris, ref index);
            ++i;
        }
    }

    public void AddTriangle(int a, int b, int c, ref int[] triArr, ref int index) {
        triArr[index] = a;
        triArr[index + 1] = b;
        triArr[index + 2] = c;

        index += 3;
    }

    public Mesh GenerateMesh(ref Mesh m, ref Vector3[] verts, ref int[] tris) {
        if(m == null) {
            m = new Mesh();
        }
        m.vertices = verts;
        m.triangles = tris;
        m.name = "Map Side";
        m.RecalculateNormals();
        m.RecalculateBounds();
        return m;
    }
}
