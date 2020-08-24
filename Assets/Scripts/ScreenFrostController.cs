using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFrostController : MonoBehaviour
{
    PostProcessFrost postProcess;
    [SerializeField] float maxIntensity = 0.5f;
    [SerializeField] float hotUpperBound = 0.75f;
    [SerializeField] float coldLowerBound = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        postProcess = GetComponent<PostProcessFrost>();
    }

    // Update is called once per frame
    void Update()
    {
        float invUpperBound = 1 - hotUpperBound;
        float invLowerBound = 1 - coldLowerBound;
        postProcess.intensity = Mathf.Lerp(0, maxIntensity, (TemperatureManager.Instance.baseTemperature - invUpperBound)/(invLowerBound-invUpperBound));
    }
}
