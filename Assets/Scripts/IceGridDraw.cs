using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGridDraw : IceGrid
{
    [Header("Draw")]
    [SerializeField] LayerMask mapLayer = 0;
    [SerializeField] int drawSize = 5;
    [SerializeField] float drawAmount = 0.1f;
    Camera cam;

    public override void UpdateGridValues()
    {
        if (cam == null)
            cam = Camera.main;
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(camRay,out hit, Mathf.Infinity, mapLayer))
        {
            Vector2Int coords = WorldPosToGridCoords(hit.point);
            DrawAtPosition(coords, drawSize, drawAmount);
        }
    }

    void DrawAtPosition(Vector2Int pos, int size, float amount)
    {
        for (int x = -size; x <= size; x++)
        {
            for (int y = -size; y <= size; y++)
            {
                Vector2Int localPos = pos + new Vector2Int(x, y);
                if (!IsInBound(localPos.x, localPos.y))
                    continue;

                float localAmount = Mathf.Lerp(0, amount * 0.5f, 1 - Mathf.Abs(x) / size) + Mathf.Lerp(0, amount * 0.5f, 1 - Mathf.Abs(y) / size);

                grid[localPos.x, localPos.y] += localAmount;
            }
        }
    }

    bool IsInBound(int x, int y)
    {
        return x >= 0 && y >= 0 && x < gridDimensions.x && y < gridDimensions.y;
    }

    protected override void Update()
    {
       // UpdateGridValues();
    }
}
