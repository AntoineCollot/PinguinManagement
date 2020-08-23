using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureManager : MonoBehaviour
{
    [Header("Penguins")]
    [SerializeField] float sameCellMultiplier = 3;
    [SerializeField] float closeNeighboursMultiplier = 1.5f;
    Dictionary<Vector2Int, TemperatureCell> penguinMap = new Dictionary<Vector2Int, TemperatureCell>();
    [Header("Settings")]
    [SerializeField] float temperatureEvolutionSpeed = 0.1f;
    [HideInInspector] public float temperatureEvolutionSpeedMultiplier = 1f;
    [SerializeField] float maxTemperatureNeeded = 20;
    [HideInInspector] public float baseTemperature;
    Vector2 temperatureOffset;

    List<Vector2Int> dirtyCells = new List<Vector2Int>();

    const int NEIGHBOURS_LOOP_START = -2;
    const int NEIGHBOURS_LOOP_END = 3;

    public float TemperatureNeeded
    {
        get
        {
            return maxTemperatureNeeded * baseTemperature;
        }
    }

    public static TemperatureManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        temperatureOffset.x = UnityEngine.Random.Range(-100f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirtyCells();

        temperatureOffset.y += Time.deltaTime * temperatureEvolutionSpeed * temperatureEvolutionSpeedMultiplier;
        baseTemperature = Mathf.PerlinNoise(temperatureOffset.x, temperatureOffset.y);
    }

    public class TemperatureCell
    {
        public List<Penguin> penguins = new List<Penguin>();
        public float temperature;
    }

    internal float GetTemperatureDelta(Vector2Int coords)
    {
        return penguinMap[coords].temperature - TemperatureNeeded - 3;
    }

    internal void RemovePenguin(Penguin penguin, Vector2Int coords)
    {
        if (penguinMap.ContainsKey(coords))
            penguinMap[coords].penguins.Remove(penguin);

        AddDirtyCells(coords);
    }

    internal Vector2Int GetCoordsFromPosition(Vector3 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
    }

    internal void UpdatePenguinPosition(Penguin penguin, Vector2Int oldCoords, Vector2Int newCoords)
    {
        RemovePenguin(penguin, oldCoords);

        if (!penguinMap.ContainsKey(newCoords))
            penguinMap.Add(newCoords, new TemperatureCell());

        penguinMap[newCoords].penguins.Add(penguin);

        AddDirtyCells(newCoords);
    }

    void AddDirtyCells(Vector2Int coords)
    {
        Vector2Int currentCoords;

        for (int x = NEIGHBOURS_LOOP_START; x < NEIGHBOURS_LOOP_END; x++)
        {
            for (int y = NEIGHBOURS_LOOP_START; y < NEIGHBOURS_LOOP_END; y++)
            {
                currentCoords = coords;
                currentCoords.x += x;
                currentCoords.y += y;

                if (!dirtyCells.Contains(currentCoords))
                    dirtyCells.Add(currentCoords);
            }
        }
    }

    void UpdateDirtyCells()
    {
        foreach (Vector2Int coords in dirtyCells)
        {
            if (!penguinMap.ContainsKey(coords))
                continue;

            float temperature = 0;
            Vector2Int neighbourCoords;
            for (int x = NEIGHBOURS_LOOP_START; x < NEIGHBOURS_LOOP_END; x++)
            {
                for (int y = NEIGHBOURS_LOOP_START; y < NEIGHBOURS_LOOP_END; y++)
                {
                    //ourself
                    if (x == 0 && y == 0)
                        temperature += penguinMap[coords].penguins.Count * sameCellMultiplier;
                    //else
                    //{
                    //    neighbourCoords = coords;
                    //    neighbourCoords.x += x;
                    //    neighbourCoords.y += y;

                    //    if (penguinMap.ContainsKey(neighbourCoords))
                    //        temperature += penguinMap[neighbourCoords].penguins.Count;
                    //}
                    //Close Neighbours
                    else if (Mathf.Abs(x) <= 1 && Mathf.Abs(y) <= 1)
                    {
                        neighbourCoords = coords;
                        neighbourCoords.x += x;
                        neighbourCoords.y += y;

                        if (penguinMap.ContainsKey(neighbourCoords))
                            temperature += penguinMap[neighbourCoords].penguins.Count * closeNeighboursMultiplier;
                    }
                    //Far Neighbours
                    else
                    {
                        neighbourCoords = coords;
                        neighbourCoords.x += x;
                        neighbourCoords.y += y;

                        if (penguinMap.ContainsKey(neighbourCoords))
                            temperature += penguinMap[neighbourCoords].penguins.Count;
                    }
                }
            }
            penguinMap[coords].temperature = temperature;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach(KeyValuePair<Vector2Int, TemperatureCell> cell in penguinMap)
        {
            Gizmos.color = Color.Lerp(Color.blue, Color.red, cell.Value.temperature / maxTemperatureNeeded);
            Gizmos.DrawCube(new Vector3(cell.Key.x, 0, cell.Key.y), Vector3.one);
        }
    }
#endif
}
