using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class NotificationObject : MonoBehaviour
{
    public enum NotificationButtonType
    {
        None = 0,
        CloseButton = 1,
        ConfirmButton = 2
    }

    [SerializeField] private GameObject headerObject;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI contentTextFull;
    [SerializeField] private TextMeshProUGUI contentTextHalf;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button confirmButton;

    private TextMeshProUGUI desiredContentText;
    private RectTransform rect;
    private Vector2 originalPosition;

    private void Initialize(Action? closeAction, Coroutine closeCoroutine)
    {
        if(rect == null)
        {
            rect = GetComponent<RectTransform>();
            originalPosition = rect.anchoredPosition;
        }

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(delegate { NotificationManager.Instance.EndNotification(); });
        closeButton.onClick.AddListener(delegate { closeAction?.Invoke(); });
        closeButton.onClick.AddListener(delegate { GameManager.Instance.audioManager.PlayButton1(); });

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(delegate { NotificationManager.Instance.EndNotification(); });
        confirmButton.onClick.AddListener(delegate { closeAction?.Invoke(); });
        confirmButton.onClick.AddListener(delegate { GameManager.Instance.audioManager.PlayButton1(); });

        if (closeCoroutine != null)
        {
            closeButton.onClick.AddListener(delegate { StopCoroutine(closeCoroutine); });
            confirmButton.onClick.AddListener(delegate { StopCoroutine(closeCoroutine); });
        }
    }

    public void TriggerNotification(string? headerTitle, string content, NotificationButtonType buttonType, Action? closeAction, Coroutine closeCoroutine)
    {
        Initialize(closeAction, closeCoroutine);

        bool headerEnabled = SetHeader(headerTitle);

        desiredContentText = headerEnabled ? contentTextHalf : contentTextFull;
        desiredContentText.SetText(content);

        closeButton.gameObject.SetActive(buttonType == NotificationButtonType.CloseButton);
        confirmButton.gameObject.SetActive(buttonType == NotificationButtonType.ConfirmButton);

        AdjustWindowSize(content, headerEnabled);

        FadeNotificationWindow(true);

        GameManager.Instance.audioManager.PlayWoosh(0.32f, 1f);
    }

    private void FadeNotificationWindow(bool fadeIn)
    {
        Vector2 outPosition = new Vector2(0, (rect.rect.height + originalPosition.y) * 1);
        Vector2 desiredPosition = fadeIn ? originalPosition : outPosition;

        if (fadeIn) {
            rect.anchoredPosition = outPosition;
            gameObject.SetActive(true);
        }

        rect.DOKill();
        rect.DOAnchorPos(desiredPosition, 0.5f).OnComplete(() =>
        {
            if (!fadeIn)
                gameObject.SetActive(false);
        }).SetUpdate(true);
    }

    private void AdjustWindowSize(string content, bool headerEnabled)
    {
        int charCount = content.Length;
        float multiplier = headerEnabled ? 2.0f : 1.7f;
        float desiredHeight = Mathf.Clamp(charCount * multiplier, headerEnabled ? 320 : 220, 440);
        float desiredWidth = Mathf.Lerp(680, 840, Mathf.InverseLerp(20, 40, charCount));
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, desiredHeight);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, desiredWidth);
    }

    private bool SetHeader(string? headerTitle)
    {
        bool headerEnabled = headerTitle != null;

        headerObject.SetActive(headerEnabled);
        contentTextFull.gameObject.SetActive(!headerEnabled);
        contentTextHalf.gameObject.SetActive(headerEnabled);

        if (headerEnabled)
            headerText.SetText(headerTitle);

        return headerEnabled;
    }
    public void CloseNotification()
    {
        FadeNotificationWindow(false);
    }
}
