using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    [Header("Cursor")]
    [SerializeField] private Texture2D cursorTexBig;
    [SerializeField] private Texture2D cursorTexMid;

    [Header("Quality")]
    [SerializeField] private Slider qualitySlider;
    [SerializeField] private TextMeshProUGUI qualityValueText;
    private int qualityMinValue = 0;
    private int qualityMaxValue = 2;
    public readonly static string GraphicQualityPrefKey = "_gfxquality";

    private void Awake()
    {
        SetCursorHand(true);
    }

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);

        LoadGraphicPrefs();

        qualitySlider.minValue = qualityMinValue;
        qualitySlider.maxValue = qualityMaxValue;
        qualitySlider.onValueChanged.AddListener(OnGraphicQualityChange);
    }
    private void OnGraphicQualityChange(float newValue)
    {
        int valueInt = Mathf.RoundToInt(newValue);
        string valueString = "";

        if (valueInt == 0)
            valueString = "Baixa";
        else if (valueInt == 1)
            valueString = "Média";
        else if (valueInt == 2)
            valueString = "Alta";

        SetGraphicQuality(valueInt);

        qualityValueText.SetText(valueString);
    }

    private void SetGraphicQuality(int level)
    {
        level = Mathf.Clamp(level, qualityMinValue, qualityMaxValue);

        QualitySettings.SetQualityLevel(level);

        PlayerPrefs.SetInt(GraphicQualityPrefKey, level);
        PlayerPrefs.Save();
    }

    private void LoadGraphicPrefs()
    {
        int graphicQuality = Mathf.Clamp(PlayerPrefsHelper.GetInt(GraphicQualityPrefKey, qualityMaxValue), qualityMinValue, qualityMaxValue);

        OnGraphicQualityChange(graphicQuality);

        qualitySlider.SetValueWithoutNotify(graphicQuality);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    public void SetCursorHand(bool cursorHand)
    {
        Texture2D cursorTex = Screen.height >= 800 ? cursorTexBig : cursorTexMid;

        if (cursorHand)
            Cursor.SetCursor(cursorTex, new Vector2(40 / 512f, 30 / 512f), CursorMode.ForceSoftware);
        else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
