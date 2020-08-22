using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinsManager : MonoBehaviour
{
    [SerializeField] Penguin penguinPrefab;
    [SerializeField] int spawnPenguinCount = 10;
    [SerializeField] float spawnAltitude = 0.5f;
    [SerializeField] Vector2 spawnArea = Vector2.one;
    [SerializeField] float spawnSurfaceLevelMargins = 0.1f;
    [SerializeField] LayerMask iceLayer = 1 << 8;
    IceGrid grid;
    MarchingCubes marchingCubes;
    Camera cam;

    public static PenguinsManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        grid = FindObjectOfType<IceGrid>();
        marchingCubes = FindObjectOfType<MarchingCubes>();
        cam = Camera.main;
    }

    private void Start()
    {
        grid.UpdateGridValues();
        SpawnPenguins(spawnPenguinCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SpawnPenguins(int count)
    {
        Vector3 randomSpawnPos;
        for (int i = 0; i < count; i++)
        {
            int tryCount = 0;
            bool locationFound = false;
            do
            {
               //try a location
                randomSpawnPos = transform.position + new Vector3(Random.Range(-spawnArea.x, spawnArea.x) * 0.5f, spawnAltitude, Random.Range(-spawnArea.y, spawnArea.y) * 0.5f);
                locationFound = (grid.GetGridValueAtPosition(randomSpawnPos) + spawnSurfaceLevelMargins) < marchingCubes.surfaceLevel;
                tryCount++;
            } while (!locationFound && tryCount<100);

            if (!locationFound)
                return false;

            Instantiate(penguinPrefab, randomSpawnPos, Quaternion.identity, transform);
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, spawnAltitude, spawnArea.y));
    }
}
