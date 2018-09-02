using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public AudioClip MenuSong;
    public AudioClip GameSong;
    public AudioClip EndSong;


    public AudioSource source;
    private float _targetVolume = 1f;

    private void OnEnable()
    {
        ObjectiveManager.OnGameStarted += OnGameStarted;
        ObjectiveManager.OnGameStarted += OnGameEnded;
    }
    private void OnDisable()
    {
        ObjectiveManager.OnGameStarted -= OnGameStarted;
        ObjectiveManager.OnGameStarted -= OnGameEnded;

    }

    private void OnGameStarted() {
        PlayGame();
    }
    private void OnGameEnded() { 

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

    private void PlayMenu() {
        source.clip = MenuSong;
        source.Play();
    }
    private void PlayGame() {
        source.clip = GameSong;
        source.Play();
    }
    private void EndGame() {
        source.clip = EndSong;
        source.Play();
    }
}