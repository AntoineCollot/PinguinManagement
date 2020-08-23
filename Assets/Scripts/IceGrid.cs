using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IceGrid : MonoBehaviour
{
    [Header("Grid")]
    public Vector2Int gridDimensions;
    public Vector3 cellSize = Vector3.one;
    public float[,] grid;

    public static IceGrid Instance;

    // Start is called before the first frame update
    protected void Awake()
    {
        Instance = this;
        grid = new float[gridDimensions.x, gridDimensions.y];
    }

    public abstract void UpdateGridValues();

    protected abstract void Update();

#if UNITY_EDITOR
    protected void OnDrawGizmosSelected()
    {
        if (grid == null)
            return;
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                Vector3 pos = transform.position + new Vector3(x * cellSize.x, 0, y * cellSize.z);
                float gridValue = GetGridValueAtPosition(pos);
                Gizmos.color = new Color(gridValue, gridValue, gridValue, 1);
                Gizmos.DrawSphere(pos, 0.1f);
            }
        }
    }
#endif

    protected Vector2Int WorldPosToGridCoords(Vector3 position)
    {
        position -= transform.position;
        Vector2Int coords = new Vector2Int(
            Mathf.FloorToInt(position.x / cellSize.x),
            Mathf.FloorToInt(position.z / cellSize.z)
            );
        return coords;
    }

    public float GetGridValueAtPosition(Vector3 position)
    {
        position -= transform.position;
        Vector2Int coords = new Vector2Int(
            Mathf.FloorToInt(position.x / cellSize.x),
            Mathf.FloorToInt(position.z / cellSize.z)
            );

        return grid[coords.x, coords.y];
    }
}
