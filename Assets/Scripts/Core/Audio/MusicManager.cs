using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{

    public AudioSource source;
    private float _targetVolume = 1f;

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    void Start()
    {       
        source.loop = true;
        source.volume = _targetVolume;
    }

    public AudioSource[] GetAudioSources()
    {
        AudioSource[] sources = new AudioSource[1];
        sources[0] = source;
        return sources;
    }
}