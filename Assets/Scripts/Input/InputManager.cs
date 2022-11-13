using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool inputEnabled { get; private set; }
    public bool cameraSwitchEnabled { get; private set; }

    public delegate void KeyDown(); 
    public static KeyDown M_KeyDown;
    public static KeyDown C_KeyDown;

    private void Start()
    {
        cameraSwitchEnabled = false;
    }

    void Update()
    {
        if (!inputEnabled) return;

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.OpenPhone(!GameManager.Instance.phoneIsOpen);
            M_KeyDown?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.C) && cameraSwitchEnabled)
        {
            GameManager.Instance.cameraSelector.NextCamera();
            C_KeyDown?.Invoke();
        }
    }

    public void EnableInput(bool enable)
    {
        inputEnabled = enable;
    }

    public void EnableCameraSwitch(bool enable)
    {
        cameraSwitchEnabled = enable;
    }
}
