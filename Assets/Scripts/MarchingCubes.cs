using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes : MonoBehaviour
{
    const int CUBE_CORNERS = 8;
    const int CUBE_EDGES = 12;
    [Range(0, 1)] public float surfaceLevel;

    IceGrid grid;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;

    [SerializeField] float generateMeshInterval = 0.1f;

    // Start is called before the first frame update
    void Awake()
    {
        grid = GetComponent<IceGrid>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = new Mesh();
    }

    private void Start()
    {
        StartCoroutine(GenerateMeshLoop());
    }

    IEnumerator GenerateMeshLoop()
    {
        while (true)
        {
            grid.UpdateGridValues();
            //Don't update both on the same frame after the init
            if (Time.timeSinceLevelLoad > 0.1f)
                yield return null;
            GenerateMesh();

            yield return new WaitForSeconds(generateMeshInterval);
        }
    }

    int ComputeCubeIndex(float[] vertexValues)
    {
        int cubeIndex = 0;
        for (int i = 0; i < CUBE_CORNERS; i++)
        {
            if (vertexValues[i] < surfaceLevel)
            {
                cubeIndex += 1 << i;
            }
        }
        return cubeIndex;
    }

    List<Vector3> March()
    {
        List<Vector3> vertices = new List<Vector3>();
        for (int x = 0; x < grid.gridDimensions.x - 1; x++)
        {
            for (int y = 0; y < grid.gridDimensions.y - 1; y++)
            {
                Vector3 cubePositionOffset = new Vector3(x * grid.cellSize.x, 0, y * grid.cellSize.z);

                //Since our grid is 2D, use same values for top and bottom
                float[] cubeValues = new float[8] {
                    grid.grid[x,y+1], grid.grid[x+1, y+1], grid.grid[x+1, y], grid.grid[x, y],
                     0,0, 0, 0
                };

                //Get the configuration of this cube
                //-> Determine the index into the edge table which tells us which vertices are inside of the surface
                int cubeIndex = ComputeCubeIndex(cubeValues);

                //-> Find the vertices where the surface intersects the cube
                //Get the pos of the vertex of id between 0 and 12
                for (int e = 0; e < CUBE_EDGES; e++)
                {
                    //Get the edges part of this configuration
                    //int edgeMask = Triangulation.edgeTable[cubeIndex];

                    //Get the position of the vertices
                    //-1 in the triTable marks the end of the edges of this config
                    //Go 3 by 3 to make triangles
                    for (int i = 0; Triangulation.triTable[cubeIndex, i] != -1; i += 3)
                    {
                        //Get both vertices of each of the 3 edges of this triangle
                        //Get the edge id from the tri table
                        int edge = Triangulation.triTable[cubeIndex, i];
                        int vertA0 = Triangulation.vertexIndexFromEdge[0, edge];
                        int vertB0 = Triangulation.vertexIndexFromEdge[1, edge];
                        edge = Triangulation.triTable[cubeIndex, i + 1];
                        int vertA1 = Triangulation.vertexIndexFromEdge[0, edge];
                        int vertB1 = Triangulation.vertexIndexFromEdge[1, edge];
                        edge = Triangulation.triTable[cubeIndex, i + 2];
                        int vertA2 = Triangulation.vertexIndexFromEdge[0, edge];
                        int vertB2 = Triangulation.vertexIndexFromEdge[1, edge];


                        vertices.Add(InterpolateMiddlePoint(vertA0, vertB0, cubeValues)  + cubePositionOffset);
                        vertices.Add(InterpolateMiddlePoint(vertA1, vertB1, cubeValues) + cubePositionOffset);
                        vertices.Add(InterpolateMiddlePoint(vertA2, vertB2, cubeValues)  + cubePositionOffset);
                    }
                }
            }
        }

        return vertices;
    }

    static Vector3 FindMiddlePoint(int vertexAId, int vertexBId)
    {
        return (GetVertexPosition(vertexAId) + GetVertexPosition(vertexBId)) * 0.5f;
    }

    Vector3 InterpolateMiddlePoint(int vertexAId, int vertexBId, float[] cubeValues)
    {
        Vector3 vertA = GetVertexPosition(vertexAId);
        Vector3 vertB = GetVertexPosition(vertexBId);
        float t = (surfaceLevel - cubeValues[vertexAId]) / (cubeValues[vertexBId] - cubeValues[vertexAId]);
        Vector3 middlePoint;
        middlePoint.x = (vertA.x + t * (vertB.x - vertA.x)) * grid.cellSize.x;
        middlePoint.z = (vertA.z + t * (vertB.z - vertA.z)) *grid.cellSize.z;
        middlePoint.y = (vertA.y + vertB.y) * 0.5f *grid.cellSize.y;
        return middlePoint;
    }

    static Vector3 GetVertexPosition(int vertexId)
    {
        Vector3 vertexPosition;
        switch (vertexId)
        {
            case 0:
                vertexPosition = new Vector3(0, 0, 1);
                break;
            case 1:
                vertexPosition = new Vector3(1, 0, 1);
                break;
            case 2:
                vertexPosition = new Vector3(1, 0, 0);
                break;
            case 3:
                vertexPosition = new Vector3(0, 0, 0);
                break;
            case 4:
                vertexPosition = new Vector3(0, 1, 1);
                break;
            case 5:
                vertexPosition = new Vector3(1, 1, 1);
                break;
            case 6:
                vertexPosition = new Vector3(1, 1, 0);
                break;
            case 7:
                vertexPosition = new Vector3(0, 1, 0);
                break;
            default:
                vertexPosition = Vector3.zero;
                break;
        }

        vertexPosition -= new Vector3(0.5f, 0.5f, 0.5f);

        return vertexPosition;
    }

    void GenerateMesh()
    {
        Vector3[] vertices = March().ToArray();
        int[] triangles = new int[vertices.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = i;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        if(meshCollider!=null)
            meshCollider.sharedMesh = mesh;
    }
}
