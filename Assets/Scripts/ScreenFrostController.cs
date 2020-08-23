using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFrostController : MonoBehaviour
{
    PostProcessFrost postProcess;
    [SerializeField] float maxIntensity = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        postProcess = GetComponent<PostProcessFrost>();
    }

    // Update is called once per frame
    void Update()
    {
        postProcess.intensity = Mathf.Lerp(0, maxIntensity, (TemperatureManager.Instance.baseTemperature - 0.5f)*2);
    }
}
