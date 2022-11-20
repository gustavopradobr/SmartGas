using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DynamicRenderScale : MonoBehaviour
{
    public static DynamicRenderScale Instance { get; private set; }

    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float scaleIncrement = 0.05f;
    [SerializeField] private int checkIntervalSeconds = 2;

    [Space]
    [SerializeField] private bool logging = false;

    private int minFps = 45;
    private int desiredFps = 55;
    private int frameCount = 0;
    private float totalDeltaTime = 0;

    private UniversalRenderPipelineAsset urp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        urp = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        urp.renderScale = 1.0f;
    }

    public void UpdateRenderer(UniversalRenderPipelineAsset newAsset)
    {
        urp = newAsset;
    }

    private void Update()
    {
        DetermineResolution();
    }

    public void ChangeResolution(int increaseStep)
    {
        urp.renderScale = Mathf.Clamp(urp.renderScale + (scaleIncrement * increaseStep), minScale, maxScale);
    }

    private void DetermineResolution()
    {
        if (totalDeltaTime <= checkIntervalSeconds)
        {
            frameCount++;
            totalDeltaTime += Time.deltaTime;

            return;
        }
        else
        {
            float fps = frameCount / totalDeltaTime;

            totalDeltaTime = 0;
            frameCount = 0;

            if (fps < minFps)
            {
                ChangeResolution(-Mathf.RoundToInt(minFps / fps));
            }
            else if (fps > desiredFps)
            {
                ChangeResolution(1);
            }

            if(logging)
                Debug.Log(string.Format("FPS: {0:F1} Scale: {1}", fps, urp.renderScale));
        }
    }

}
