using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] float iceGridNoiseSpeedIncrease = 0.05f;
    [SerializeField] float temperatureNoiseSpeedIncrease = 0.05f;
    public UnityEvent onGameOver = new UnityEvent();

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseGameDifficulty();
    }

    void IncreaseGameDifficulty()
    {
        IceGrid.Instance.noiseSpeedMutliplier += iceGridNoiseSpeedIncrease * Time.deltaTime;
        TemperatureManager.Instance.temperatureEvolutionSpeedMultiplier += temperatureNoiseSpeedIncrease * Time.deltaTime;
    }

    public void GameOver()
    {
        onGameOver.Invoke();
    }
}
