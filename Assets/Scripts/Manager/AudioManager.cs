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

    [Header("Audio Clips")]
    [SerializeField] private AudioClip lowFuel;
    [SerializeField] private AudioClip gpsRouteFound;


    private float wooshVolume, wooshPitch;

    private void Start()
    {
        wooshVolume = wooshSource.volume;
        wooshPitch = wooshSource.pitch;
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
}
