using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGridScroll : IceGrid
{
    [Header("Noise")]
    public float noiseFrequency;
    public float noiseSpeed;
    [HideInInspector] public float noiseSpeedMutliplier = 1;
    protected Vector2 noiseOffset;
    protected bool noiseInitialized = false;

    public override void UpdateGridValues()
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
                grid[x, y] = Mathf.PerlinNoise((float)x / gridDimensions.x * noiseFrequency + noiseOffset.x, (float)y / gridDimensions.y * noiseFrequency + noiseOffset.y);
            }
        }
    }

    protected override void Update()
    {
        if (Time.timeSinceLevelLoad > 3  && !GameManager.Instance.gameIsOver)
        {
            noiseOffset.x += Mathf.Cos(Time.timeSinceLevelLoad / 5) * Time.deltaTime * noiseSpeed;
            noiseOffset.y += Time.deltaTime * noiseSpeed * noiseSpeedMutliplier;
        }
    }
}
