using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Termometer : MonoBehaviour
{
    Material mat;

    [SerializeField] Color coldColor = Color.white;
    [SerializeField] Color hotColor = Color.white;
    Graphic graphic;

    // Start is called before the first frame update
    void Start()
    {
        graphic = GetComponent<Graphic>();
        mat = graphic.material;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mat.SetFloat("_FillThreshold", 1-TemperatureManager.Instance.baseTemperature);
        graphic.color = Color.Lerp(coldColor, hotColor, 1 - TemperatureManager.Instance.baseTemperature);
    }
}
