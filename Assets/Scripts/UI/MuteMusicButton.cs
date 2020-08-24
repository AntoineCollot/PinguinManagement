using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteMusicButton : MonoBehaviour
{
    Button button;
    static bool isMusicOn = true;

    [SerializeField] Image image = null;
    [SerializeField] Sprite musicPlayingSprite = null;
    [SerializeField] Sprite musicMutedSprite = null;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleOnOff);

        AudioManager.Instance.PlayMusic(isMusicOn);
        UpdateButtonState();
    }

    public void ToggleOnOff()
    {
        isMusicOn = !isMusicOn;

        UpdateButtonState();

        AudioManager.Instance.PlayMusic(isMusicOn);
    }

    void UpdateButtonState()
    {
        if (isMusicOn)
            image.sprite = musicPlayingSprite;
        else
            image.sprite = musicMutedSprite;
    }
}
