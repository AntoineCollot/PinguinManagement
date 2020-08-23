using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGrid : MonoBehaviour
{
    public Vector2Int gridDimensions;
    public Vector3 cellSize = Vector3.one;

    public float[,] grid;
    public float noiseFrequency;
    public float noiseSpeed;
    [HideInInspector] public float noiseSpeedMutliplier = 1;

    Vector2 noiseOffset;
    bool noiseInitialized = false;

    public static IceGrid Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        grid = new float[gridDimensions.x, gridDimensions.y];
    }

    public void UpdateGridValues()
    {
        if (!noiseInitialized)
        {
            noiseInitialized = true;
            noiseOffset = new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
        }

        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                grid[x, y] = Mathf.PerlinNoise((float)x / gridDimensions.x * noiseFrequency + Mathf.Cos(noiseOffset.x) * 5, (float)y / gridDimensions.y * noiseFrequency + noiseOffset.y);
            }
        }
    }

    private void Update()
    {
        if (Time.time > 3)
        {
            noiseOffset.x += Time.deltaTime * noiseSpeed;
            noiseOffset.y += Time.deltaTime * noiseSpeed * noiseSpeedMutliplier;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
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
