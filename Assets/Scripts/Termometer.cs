using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Termometer : MonoBehaviour
{
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Graphic>().material;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mat.SetFloat("_FillThreshold", 1-TemperatureManager.Instance.baseTemperature);
    }
}
