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


    public AudioSource sourceMenu;
    public AudioSource sourceGame;
    public AudioSource sourceEnd;

    public float _targetVolumeMenu = 1f;
    public float _targetVolumeGame = 1f;
    public float _targetVolumeEnd = 1f;

    private bool gameEnded = false;

    private void OnEnable()
    {
        ObjectiveManager.OnGameStarted += OnGameStarted;
        ObjectiveManager.OnGameEnded += OnGameEnded;
        RadioActivation.OnRadioActivated += OnRadioActivated;
    }
    private void OnDisable()
    {
        ObjectiveManager.OnGameStarted -= OnGameStarted;
        ObjectiveManager.OnGameEnded -= OnGameEnded;
        RadioActivation.OnRadioActivated -= OnRadioActivated;
    }

    private void OnGameStarted() {
        PlayGame();
    }
    private void OnGameEnded() {
        gameEnded = true;
        EndGame();
    }
    private void OnRadioActivated(bool enabled) {
        if (gameEnded) return;

        if(enabled)
            sourceGame.DOFade(_targetVolumeGame * 0.25f, 0.5f);
        else
            sourceGame.DOFade(_targetVolumeGame * 1f, 0.5f);
    }


    void Start()
    {
        sourceMenu.volume = 0;
        sourceGame.volume = 0;
        sourceEnd.volume = 0;

        PlayMenu();
    }

    private void PlayMenu() {
        sourceMenu.clip = MenuSong;
        sourceMenu.Play();
        sourceMenu.DOFade(1f, 0.5f);

    }
    private void PlayGame() {
        sourceGame.clip = GameSong;
        sourceGame.DOFade(_targetVolumeGame, 2.5f);
        sourceGame.Play();

        sourceMenu.DOFade(0f, 2f);
    }
    private void EndGame() {
        sourceEnd.clip = EndSong;
        sourceEnd.Play();
        sourceEnd.DOFade(_targetVolumeEnd, 2.5f);

        sourceGame.DOFade(0f, 2f);
    }
}