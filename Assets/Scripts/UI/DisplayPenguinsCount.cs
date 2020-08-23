using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPenguinsCount : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] string textBefore = "";
    [SerializeField] string textAfter = "";

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        text.text = textBefore + PenguinsManager.Instance.penguinsCount.ToString() + textAfter;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!GameManager.Instance.gameIsOver)
            text.text = textBefore + PenguinsManager.Instance.penguinsCount.ToString()+ textAfter;
    }
}
