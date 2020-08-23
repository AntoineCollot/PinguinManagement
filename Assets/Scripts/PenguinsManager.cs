using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinsManager : MonoBehaviour
{
    [SerializeField] Penguin penguinPrefab = null;
    [SerializeField] int spawnPenguinCount = 10;
    [SerializeField] float spawnAltitude = 0.5f;
    [SerializeField] Vector2 spawnArea = Vector2.one;
    [SerializeField] float spawnSurfaceLevelMargins = 0.1f;
    [SerializeField] LayerMask iceLayer = 1 << 8;
    IceGridScroll grid;
    MarchingCubes marchingCubes;
    Camera cam;

    [Header("Respawning")]
    [SerializeField] float minRespawnTime = 7;
    [SerializeField] float maxRespawnTime = 15;
    [SerializeField] float respawnAltitude = 0.5f;

    [HideInInspector] public int penguinsCount;
    public static PenguinsManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        grid = FindObjectOfType<IceGridScroll>();
        marchingCubes = FindObjectOfType<MarchingCubes>();
        cam = Camera.main;
    }

    private void Start()
    {
        grid.UpdateGridValues();
        SpawnPenguins(spawnPenguinCount, spawnAltitude);

        StartCoroutine(RespawnPenguins());
    }

    public void PenguinKilled()
    {
        penguinsCount--;

        if (penguinsCount <= 0)
            GameManager.Instance.GameOver();
    }

    IEnumerator RespawnPenguins()
    {
        while(!GameManager.Instance.gameIsOver)
        {
            yield return new WaitForSeconds(Random.Range(minRespawnTime, maxRespawnTime));

            SpawnPenguins(1,respawnAltitude);
        }
    }

    public bool SpawnPenguins(int count, float spawnAltitude =1)
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
                locationFound = (grid.GetGridValueAtPosition(randomSpawnPos) + spawnSurfaceLevelMargins) > marchingCubes.surfaceLevel;
                tryCount++;
            } while (!locationFound && tryCount<100);

            if (!locationFound)
                return false;

            penguinsCount++;
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
