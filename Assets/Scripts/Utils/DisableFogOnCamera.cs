using UnityEngine;
using UnityEngine.Rendering;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class DisableFogOnCamera : MonoBehaviour
{
    private Camera cameraWithoutFog;

    private void Awake()
    {
        cameraWithoutFog = GetComponent<Camera>();
    }

    private void Start()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }
    void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }
    void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera == cameraWithoutFog)
            RenderSettings.fog = false;
        else
            RenderSettings.fog = true;
    }
}
