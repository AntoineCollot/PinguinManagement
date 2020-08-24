using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenButton : MonoBehaviour
{
    Button button;
    static bool isFullScreen = false;
    const int DEFAULT_WIDTH = 1280;
    const int DEFAULT_HEIGHT = 720;

    [SerializeField] TextMeshProUGUI text = null;
    [SerializeField] string fullScreenText = "Full Screen";
    [SerializeField] string windowedText = "Windowed";

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleOnOff);

        isFullScreen = GetFullScreenState();
        UpdateButtonVisuals();
    }

    public void ToggleOnOff()
    {
        isFullScreen = !isFullScreen;

        ApplyButtonEffect();
        UpdateButtonVisuals();
    }

    bool GetFullScreenState()
    {
        return Screen.width == Screen.currentResolution.width && Screen.height == Screen.currentResolution.height;
    }

    void ApplyButtonEffect()
    {
        if (isFullScreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
        }
        else
        {
            Screen.SetResolution(DEFAULT_WIDTH, DEFAULT_HEIGHT, false);
        }
    }

    void UpdateButtonVisuals()
    {
        if (isFullScreen)
        {
            text.text = fullScreenText;
        }
        else
        {
            text.text = windowedText;
        }
    }
}
