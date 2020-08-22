using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureManager: MonoBehaviour
{
    Dictionary<Vector2Int, TemperatureCell> penguinMap = new Dictionary<Vector2Int, TemperatureCell>();

    public static TemperatureManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class TemperatureCell
    {
        public List<Penguin> penguins = new List<Penguin>();
        public float temperature;
    }
}
