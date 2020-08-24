using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Difficulty Increase")]
    [SerializeField] float iceGridNoiseSpeedIncrease = 0.05f;
    [SerializeField] float temperatureNoiseSpeedIncrease = 0.05f;
    IceGridScroll grid;

    [Header("GameOver")]
    public UnityEvent onGameOver = new UnityEvent();
    [HideInInspector] public bool gameIsOver = false;

    [Header("Score")]
    [SerializeField] float scoreIncreasePerPenguinPerSec = 0.1f;
    [HideInInspector] public float score;

    [Header("Timer")]
    public System.TimeSpan fullTime = new System.TimeSpan(0,3,0);
    public UnityEvent onTimesOff = new UnityEvent();

    [Header("WebCursorFix")]
    [SerializeField] Texture2D cursorTex = null;

    public System.TimeSpan RemainingTime
    {
        get
        {
            return fullTime - new System.TimeSpan(0, 0, Mathf.FloorToInt(Time.timeSinceLevelLoad));
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        Cursor.visible = false;
    }

    void Start()
    {
        grid = FindObjectOfType<IceGridScroll>();
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.ForceSoftware);
        gameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
            return;

        score += Time.deltaTime * scoreIncreasePerPenguinPerSec * PenguinsManager.Instance.penguinsCount;

        IncreaseGameDifficulty();

        if(RemainingTime.Ticks<0)
        {
            TimesOff();
        }
    }

    void IncreaseGameDifficulty()
    {
        grid.noiseSpeedMutliplier += iceGridNoiseSpeedIncrease * Time.deltaTime;
        TemperatureManager.Instance.temperatureEvolutionSpeedMultiplier += temperatureNoiseSpeedIncrease * Time.deltaTime;
    }

    public void GameOver()
    {
        if (gameIsOver)
            return;

        gameIsOver = true;
        Cursor.visible = true;
        Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.ForceSoftware);
        onGameOver.Invoke();
    }

    public void TimesOff()
    {
        if (gameIsOver)
            return;

        gameIsOver = true;
        Cursor.visible = true;
        Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.ForceSoftware);
        onTimesOff.Invoke();
    }
}
