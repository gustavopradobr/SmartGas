using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppUI : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private GameObject activityWindow;
    [SerializeField] private GameObject statusBar;

    [Header("Screen")]
    [SerializeField] private List<GameObject> phoneScreen = new List<GameObject>();
    [SerializeField] private GameObject navigationArea;
    [SerializeField] private Button startAppButton;
    [SerializeField] private Button backButton;

    [Header("GPS Screen")]
    [SerializeField] private List<GameObject> gpsStep = new List<GameObject>();

    [Header("Function Buttons")]
    [SerializeField] private Button mapButton;
    [SerializeField] private Button pumpButton;

    private bool phoneAwake = false;
    private Coroutine gpsCoroutine;

    public delegate void AppEvent();
    public static AppEvent onEnableGasPump;

    private void Start()
    {
        SetupFunctionButtons();
        activityWindow.SetActive(false);
        statusBar.SetActive(false);

        SetTouchActive(false);
        SetScreen(-1, false);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetScreen(0, false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetScreen(1, false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetScreen(2, false);
        }
#endif
    } 

    public void PhoneWake()
    {
        if (phoneAwake) return;
        phoneAwake = true;

        SetScreen(0, false);
        activityWindow.SetActive(true);
        statusBar.SetActive(true);
    }

    private void ShowLoadingScreen()
    {
        SetScreen(1, false);

        DelayedHelper.InvokeDelayed(ShowHomeScreen, 3f);
    }

    private void ShowHomeScreen()
    {
        SetScreen(2, false);
    }

    private void ShowMapScreen()
    {
        SetScreen(3, true);
        if (gpsCoroutine != null)
            StopCoroutine(gpsCoroutine);
        gpsCoroutine = StartCoroutine(GPSAnimationCoroutine());

        GameManager.Instance.sceneScript.MapButtonClicked();
    }

    private IEnumerator GPSAnimationCoroutine()
    {
        foreach (GameObject scr in gpsStep)
            scr.SetActive(false);
        gpsStep[0].SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        gpsStep[0].SetActive(false);
        gpsStep[1].SetActive(true);
        GameManager.Instance.MapScreenChanged(true);
        GameManager.Instance.audioManager.PlayRouteFound();

        DelayedHelper.InvokeDelayed(GameManager.Instance.sceneScript.MapFound, 1.5f);
    }

    public void ShowGPSArrivedScreen()
    {
        SetScreen(3, true);
        foreach (GameObject scr in gpsStep)
            scr.SetActive(false);
        gpsStep[2].SetActive(true);
    }

    private void SetScreen(int screenIndex, bool navigationEnabled)
    {
        navigationArea.SetActive(navigationEnabled);
        foreach (GameObject scr in phoneScreen)
            scr.SetActive(false);
        if(screenIndex >= 0 && screenIndex < phoneScreen.Count)
            phoneScreen[screenIndex].SetActive(true);
    }

    public void NavigationBackButton()
    {
        ShowHomeScreen();

        GameManager.Instance.MapScreenChanged(false);
        if (gpsCoroutine != null)
            StopCoroutine(gpsCoroutine);
    }

    private void EnableGasPump()
    {
        onEnableGasPump?.Invoke();
    }

    public void EnableMapButton(bool enable)
    {
        mapButton.interactable = enable;
    }

    public void EnablePumpButton(bool enable)
    {
        pumpButton.interactable = enable;
    }

    public void SetTouchActive(bool activate)
    {
        raycaster.enabled = activate;
    }

    private void SetupFunctionButtons()
    {
        startAppButton.onClick.AddListener(delegate { ShowLoadingScreen(); GameManager.Instance.audioManager.PlayButton2(); });
        backButton.onClick.AddListener(delegate { NavigationBackButton(); GameManager.Instance.audioManager.PlayButton1(); });

        mapButton.onClick.AddListener(delegate { ShowMapScreen(); GameManager.Instance.audioManager.PlayButton2(); });
        pumpButton.onClick.AddListener(delegate { EnableGasPump(); GameManager.Instance.audioManager.PlayButton2(); });
    }
}
