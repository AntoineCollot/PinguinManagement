using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteMusicButton : MonoBehaviour
{
    Button button;
    bool isMusicOn = true;

    [SerializeField] Image image = null;
    [SerializeField] Sprite musicPlayingSprite = null;
    [SerializeField] Sprite musicMutedSprite = null;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleOnOff);
    }

    public void ToggleOnOff()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
            image.sprite = musicPlayingSprite;
        else
            image.sprite = musicMutedSprite;

        AudioManager.Instance.PlayMusic(isMusicOn);
    }
}
