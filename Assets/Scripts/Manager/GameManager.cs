using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public AppUI appUiManager;
    [SerializeField] private MapManager mapManager;
    public AudioManager audioManager;

    [Header("Car Controller")]
    public PrometeoHelper carHelper;

    [Header("Phone")]
    [SerializeField] private GameObject phoneRoot;
    [SerializeField] private Transform phoneSupportTransform;
    [SerializeField] private Transform phoneHandTransform;
    [SerializeField] private Transform phoneModelTransform;
    [SerializeField] private bool rotateByMousePosition = false;

    [Header("Menu")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button backMenuButton;
    [SerializeField] private GameObject pauseMenuCanvas;

    [HideInInspector] public CameraSelector cameraSelector;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public SceneScript sceneScript;
    [HideInInspector] public bool phoneIsOpen { get; private set; }
    private bool canOpenPhone = true;
    Vector3 phoneOriginalAngles;

    private bool paused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        cameraSelector = GetComponent<CameraSelector>();
        inputManager = GetComponent<InputManager>();
        sceneScript = GetComponent<SceneScript>();
    }

    void Start()
    {
        phoneOriginalAngles = phoneModelTransform.localRotation.eulerAngles;

        phoneRoot.transform.localPosition = phoneSupportTransform.localPosition;
        phoneRoot.transform.localRotation = phoneSupportTransform.localRotation;
        phoneIsOpen = false;
        cameraSelector.SetCamera(1);
        mapManager.EnableWorldRoute(false);

        continueButton.onClick.AddListener(PauseGame);
        backMenuButton.onClick.AddListener(BackToMenu);

        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    private void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        paused = !paused;

        Time.timeScale = paused ? 0.000000001f : 1;
        pauseMenuCanvas.SetActive(paused);
    }

    void Update()
    {
        if (phoneIsOpen && rotateByMousePosition)
        {
            RotatePhoneByMousePosition();
        }
    }

    public void OpenPhone(bool open)
    {
        if (!canOpenPhone) return;
        canOpenPhone = false;
        float duration = 1.0f;

        Transform targetTransform = open ? phoneHandTransform : phoneSupportTransform;

        audioManager.PlayWoosh(0.25f, 0.7f);

        if (open)
        {
            carHelper.ActivateHandbrake();
            //carHelper.ActivateInput(false);
            cameraSelector.SetPhoneCamera();            
        }
        else
        {
            //carHelper.ActivateInput(true);
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

    public void EnableVehicleInput(bool enable)
    {
        carHelper.ActivateInput(enable);
    }
}
