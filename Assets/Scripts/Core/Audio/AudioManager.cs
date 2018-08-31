using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class AudioManager : BaseManager<AudioManager>
{

#if UNITY_EDITOR
    public List<AudioClip> ClipToDebug;
#endif

    public int AmtAudioSources = 32;

    [Header("Audio Mixers")] public AudioMixerGroup MasterMixer;
    public AudioMixerGroup MusicMasterMixer;
    public AudioMixerGroup GameplayMusicMasterMixer;

    [Header("Audio Clips")] public AudioClip test; //Use Audio Clips or Scriptable objects

    private List<AudioSource> inactiveAudioSources;
    private List<AudioSource> activeAudioSources;
    private List<AudioSource> activeAudioSourcesToMuteOnDeath;
    private AudioSource[] musicAudioSource;

    private AudioCooldownManager _audioCooldownManager;
    private MusicManager _musicManager;

    public override void OnStartManager()
    {
        _musicManager = GetComponent<MusicManager>();
        musicAudioSource = _musicManager.GetAudioSources();
        inactiveAudioSources = new List<AudioSource>(64);
        activeAudioSources = new List<AudioSource>(64);

        for (int i = 0; i < AmtAudioSources; ++i)
        {
            CreateNewAudioSource();
        }

        _audioCooldownManager = new AudioCooldownManager();
    }

    public override void OnUpdateManager(float deltaTime)
    {
        _audioCooldownManager.UpdateCooldowns();
    }

    //Callbacks
    public override void OnRegisterCallbacks()
    {
        //Register Callbacks
    }

    public override void OnUnregisterCallbacks()
    {
        //Unregister Callbacks
    }

    #region CALLBACKS

    private void Callback_Exemple()
    {
        PlaySoundEffect(test);
    }

    #endregion



    //Audio Management
    private void CreateNewAudioSource()
    {
        inactiveAudioSources.Add(gameObject.AddComponent<AudioSource>());
    }


    private void PlaySoundEffect(AudioClip[] clips, float pitch = 1.0f, float stereoPan = 0f, AudioMixerGroup mixerGroup = null, bool noCooldown = false)
    {
        if (clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];

            if (clip == null)
            {
                Debug.LogWarning("Sound in array is null, no sound will be played !");
            }
            else
            {
                PlaySoundEffect(clip, pitch, stereoPan, mixerGroup, noCooldown);
            }
        }
        else
        {
            Debug.LogWarning("Array is null, no sound will be played !");
        }
    }

    private void PlaySoundEffect(AudioClip clip, float pitch = 1.0f, float stereoPan = 0f, AudioMixerGroup mixerGroup = null, bool noCooldown = false)
    {
        bool valid = true;

        if (clip == null)
        {
            Debug.LogWarning("Audioclip unset !");
            valid = false;
        }
        else if (noCooldown)
        {
            //Debug.Log("No Cooldown : " + clip);
            valid = true;
        }
        else if (!_audioCooldownManager.AddClipCooldown(clip))
        {
            //Debug.Log("Dictionary Cooldown : " + clip);
            valid = false;
        }

        if (valid)
        {
            StartCoroutine(PlaySoundCoroutine(clip, pitch, stereoPan, mixerGroup, noCooldown));
        }
    }


    private IEnumerator PlaySoundCoroutine(AudioClip clip, float pitch = 1.0f, float stereoPan = 0f, AudioMixerGroup mixerGroup = null, bool noCooldown = false)
    {
        if (inactiveAudioSources.Count == 0)
        {
            Debug.LogError("Creating new audio source");
            CreateNewAudioSource();
        }

        AudioSource selectedAudioSource = inactiveAudioSources[0];
        inactiveAudioSources.RemoveAt(0);

        selectedAudioSource.volume = 1;
        selectedAudioSource.pitch = pitch;
        selectedAudioSource.panStereo = stereoPan;

        if (mixerGroup != null) selectedAudioSource.outputAudioMixerGroup = mixerGroup;
        selectedAudioSource.PlayOneShot(clip);

        #if UNITY_EDITOR
        if (ClipToDebug.Contains(clip))
            Debug.Log("Played DEBUG SOUND : " + clip + "(" + selectedAudioSource.GetInstanceID() + ")" + "  IS BEING PLAYED : " + selectedAudioSource.isPlaying);
        #endif

        activeAudioSources.Add(selectedAudioSource);
        yield return new WaitForSeconds(clip.length / pitch);
        activeAudioSources.Remove(selectedAudioSource);
        inactiveAudioSources.Add(selectedAudioSource);
    }

}

