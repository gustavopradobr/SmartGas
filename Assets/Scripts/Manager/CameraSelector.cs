using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelector : MonoBehaviour
{
    [Header("Default Cameras")]
    [SerializeField] private CinemachineVirtualCamera[] cinemachine;
    [SerializeField] private CinemachineVirtualCamera cinemachinePhone;

    private int actualCamera = 0;

    public void NextCamera()
    {
        if (GameManager.Instance.phoneIsOpen) return;

        actualCamera++;
        if (actualCamera >= cinemachine.Length)
            actualCamera = 0;
        foreach (CinemachineVirtualCamera cam in cinemachine)
            cam.Priority = 0;
        cinemachinePhone.Priority = 0;
        cinemachine[actualCamera].Priority = 1;
    }

    public void SetCamera(int cameraIndex)
    {
        actualCamera = Mathf.Clamp(cameraIndex, 0, cinemachine.Length - 1);
        foreach (CinemachineVirtualCamera cam in cinemachine)
            cam.Priority = 0;
        cinemachinePhone.Priority = 0;
        cinemachine[actualCamera].Priority = 1;
    }
    public void SetPhoneCamera()
    {
        foreach (CinemachineVirtualCamera cam in cinemachine)
            cam.Priority = 0;
        cinemachinePhone.Priority = 1;
    }

    public void ShakeCamera(float factor)
    {
        float amplitude = Mathf.Lerp(0, 2, factor);
        float frequency = Mathf.Lerp(0, 6, factor);
        float duration = Mathf.Lerp(0, 2, factor);
        bool easeOut = true;

        foreach (CinemachineVirtualCamera cam in cinemachine)
        {
            CinemachineShake shake = cam.GetComponent<CinemachineShake>();
            shake.ShakeCamera(amplitude, frequency, duration, easeOut);
        }
    }
}
