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

    private void OnEnable()
    {
        ObjectiveManager.OnGameStarted += OnGameStarted;
        ObjectiveManager.OnGameEnded += OnGameEnded;
    }
    private void OnDisable()
    {
        ObjectiveManager.OnGameStarted -= OnGameStarted;
        ObjectiveManager.OnGameEnded -= OnGameEnded;
    }

    private void OnGameStarted() {
        PlayGame();
    }
    private void OnGameEnded() {
        EndGame();
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
        sourceGame.DOFade(_targetVolumeGame, 1.5f);
        sourceGame.Play();

        sourceMenu.DOFade(0f, 1f);
    }
    private void EndGame() {
        sourceEnd.clip = EndSong;
        sourceEnd.Play();
        sourceEnd.DOFade(_targetVolumeEnd, 1.5f);

        sourceGame.DOFade(0f, 1f);
    }
}