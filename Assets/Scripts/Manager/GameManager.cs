using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] private AppUI appUiManager;
    [SerializeField] private MapManager mapManager;

    [Header("Car Controller")]
    [SerializeField] private PrometeoHelper carHelper;

    [Header("Phone")]
    [SerializeField] private GameObject phoneRoot;
    [SerializeField] private Transform phoneSupportTransform;
    [SerializeField] private Transform phoneHandTransform;
    [SerializeField] private Transform phoneModelTransform;
    [SerializeField] private bool rotateByMousePosition = false;

    [HideInInspector] public CameraSelector cameraSelector;
    [HideInInspector] public bool phoneIsOpen { get; private set; }
    private bool canOpenPhone = true;
    Vector3 phoneOriginalAngles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        cameraSelector = GetComponent<CameraSelector>();
        phoneOriginalAngles = phoneModelTransform.localRotation.eulerAngles;

        phoneRoot.transform.localPosition = phoneSupportTransform.localPosition;
        phoneRoot.transform.localRotation = phoneSupportTransform.localRotation;
        phoneIsOpen = false;
        cameraSelector.SetCamera(1);
        mapManager.EnableWorldRoute(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            OpenPhone(!phoneIsOpen);
        }

        if (phoneIsOpen && rotateByMousePosition)
        {
            RotatePhoneByMousePosition();
        }
    }

    private void OpenPhone(bool open)
    {
        if (!canOpenPhone) return;
        canOpenPhone = false;
        float duration = 1.0f;

        Transform targetTransform = open ? phoneHandTransform : phoneSupportTransform;

        if (open)
        {
            carHelper.ActivateHandbrake();
            carHelper.ActivateInput(false);
            cameraSelector.SetPhoneCamera();
        }
        else
        {
            carHelper.ActivateInput(true);
            cameraSelector.SetCamera(1);
            appUiManager.SetTouchActive(false);
        }

        phoneRoot.transform.DOLocalRotate(targetTransform.localRotation.eulerAngles, duration).SetEase(Ease.OutQuart);
        phoneRoot.transform.DOLocalMove(targetTransform.localPosition, duration).OnComplete(() =>
        {
            phoneIsOpen = open;
            canOpenPhone = true;
            appUiManager.SetTouchActive(open);
            if (open)
                appUiManager.PhoneWake();
            phoneModelTransform.localRotation = Quaternion.Euler(phoneOriginalAngles);
        }).SetEase(Ease.OutQuart);
    }

    public void MapScreenChanged(bool mapOpen)
    {
        mapManager.EnableWorldRoute(mapOpen);
    }

    private void RotatePhoneByMousePosition()
    {
        float mouseRatioX = Mathf.Clamp01(Input.mousePosition.x / Screen.width);
        float mouseRatioY = Mathf.Clamp01(Input.mousePosition.y / Screen.height);
        mouseRatioX = (mouseRatioX - 0.5f) * 2;
        mouseRatioY = (mouseRatioY - 0.5f) * 2;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(mouseRatioY * 5, mouseRatioX * -15, 0) + phoneOriginalAngles);
        Quaternion deriv = Quaternion.identity;
        phoneModelTransform.localRotation = QuaternionUtil.SmoothDamp(phoneModelTransform.localRotation, targetRotation, ref deriv, Time.deltaTime * 4f);
    }
}
