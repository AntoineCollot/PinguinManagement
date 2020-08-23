using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnKey : MonoBehaviour
{
    [SerializeField] KeyCode keycode = KeyCode.Escape;
    public UnityEvent onKeyPressed = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keycode))
            onKeyPressed.Invoke();
    }
}
