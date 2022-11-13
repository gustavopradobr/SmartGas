using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using static NotificationObject;
using System;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }


    [Header("References")]
    [SerializeField] private GameObject notificationCanvas;
    [SerializeField] private NotificationObject notificationObject;
    [SerializeField] private Image background;

    [Header("Properties")]
    [SerializeField] private bool enableLog = true;

    private Color transparentColor = new Color(0,0,0,0);
    private Color backgroundOriginalColor;
    private bool activeNotification = false;
    private Coroutine actualCoroutine = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        backgroundOriginalColor = background.color;
        notificationCanvas.SetActive(false);
    }

    public void ShowNotification(string? headerTitle, string content, NotificationButtonType buttonType, bool blockBackground, float duration, Action? closeAction)
    {
        if (actualCoroutine != null)
        {
            StopCoroutine(actualCoroutine);
            actualCoroutine = null;
        }

        if (activeNotification)
            EndNotification();

        activeNotification = true;
        notificationCanvas.SetActive(true);

        background.raycastTarget = blockBackground;
        background.color = transparentColor;

        if (blockBackground)
        {
            background.DOKill();
            background.DOColor(backgroundOriginalColor, 0.5f).SetUpdate(true);
        }

        if (duration > 0)
            actualCoroutine = StartCoroutine(CloseNotificationCoroutine(duration));

        notificationObject.TriggerNotification(headerTitle, content, buttonType, closeAction, actualCoroutine);

        if(enableLog)
            Debug.Log($"Notification: {content}");
    }

    public void EndNotification()
    {
        if (!activeNotification) return;

        if (enableLog)
            Debug.Log("EndNotification");

        background.DOKill();
        background.DOColor(transparentColor, 0.5f).OnComplete(() => {
            background.raycastTarget = false;
        }).SetUpdate(true);

        notificationObject.CloseNotification();

        activeNotification = false;
    }

    private IEnumerator CloseNotificationCoroutine(float secondsDelay)
    {
        yield return new WaitForSecondsRealtime(secondsDelay);
        EndNotification();
    }
}
