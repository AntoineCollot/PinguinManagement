using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPenguinsCount : MonoBehaviour
{
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        text.text = PenguinsManager.Instance.penguinsCount.ToString();
    }
}
