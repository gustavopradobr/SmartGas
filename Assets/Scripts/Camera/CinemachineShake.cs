using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{

    private CinemachineVirtualCamera cineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer;

    private float _amplitudeGain;
    private float _frequencyGain;

    private Coroutine shakeCoroutine;

    private void Awake()
    {
        cineCam = GetComponent<CinemachineVirtualCamera>();
        noise = cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        _amplitudeGain = noise.m_AmplitudeGain;
        _frequencyGain = noise.m_FrequencyGain;
    }

    public void ShakeCamera(float amplitude, float frequency, float duration, bool easeOut)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeCameraCoroutine(amplitude, frequency, duration, easeOut));
    }

    IEnumerator ShakeCameraCoroutine(float amp, float freq, float duration, bool easeOut)
    {
        duration = Mathf.Max(duration, 0.01f);
        if (easeOut)
        {
            float elapsed = 0;

            while (elapsed <= duration)
            {
                noise.m_AmplitudeGain = Mathf.Lerp(amp, _amplitudeGain, elapsed / duration);
                noise.m_FrequencyGain = Mathf.Lerp(freq, _frequencyGain, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            noise.m_AmplitudeGain = _amplitudeGain;
            noise.m_FrequencyGain = _frequencyGain;
        }
        else
        {
            noise.m_AmplitudeGain = amp;
            noise.m_FrequencyGain = freq;

            yield return new WaitForSeconds(duration);

            noise.m_AmplitudeGain = _amplitudeGain;
            noise.m_FrequencyGain = _frequencyGain;
        }
    }
}
