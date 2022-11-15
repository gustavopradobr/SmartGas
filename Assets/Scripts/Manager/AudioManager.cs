using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource oneShotSource;
    [SerializeField] private AudioSource button1Source;
    [SerializeField] private AudioSource button2Source;
    [SerializeField] private AudioSource wooshSource;
    [SerializeField] private AudioSource carImpactSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip lowFuel;
    [SerializeField] private AudioClip gpsRouteFound;


    private float wooshVolume, wooshPitch;
    private float impactVolume, impactPitch;

    private void Start()
    {
        wooshVolume = wooshSource.volume;
        wooshPitch = wooshSource.pitch;

        impactVolume = carImpactSource.volume;
        impactPitch = carImpactSource.pitch;
    }

    public void PlayLowFuel()
    {
        oneShotSource.PlayOneShot(lowFuel, 1);
    }
    public void PlayRouteFound()
    {
        oneShotSource.PlayOneShot(gpsRouteFound, 1);
    }

    public void PlayButton1()
    {
        button1Source.Play();
    }

    public void PlayButton2()
    {
        button2Source.Play();
    }   
    
    public void PlayWoosh(float volumeFactor, float pitchFactor)
    {
        wooshSource.volume = wooshVolume * volumeFactor;
        wooshSource.pitch = wooshPitch * pitchFactor;
        wooshSource.Play();
    }

    public void PlayCarImpact(float volumeFactor, float pitchFactor)
    {
        carImpactSource.volume = Mathf.Clamp(impactVolume * volumeFactor, impactVolume * 0.5f, impactVolume);
        carImpactSource.pitch = Mathf.Clamp(impactPitch * pitchFactor, impactPitch * 0.7f, impactPitch);
        carImpactSource.Play();
    }
}
