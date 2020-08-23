using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayTimer : MonoBehaviour
{
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.gameIsOver)
        {
            System.TimeSpan time = GameManager.Instance.RemainingTime;
            text.text = time.TotalSeconds + "s";
        }
    }
}
