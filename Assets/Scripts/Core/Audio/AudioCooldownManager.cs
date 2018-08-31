using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCooldownManager
{

    private Dictionary<AudioClip, int> _currentCooldowns;
    private List<AudioClip> clips;

    public AudioCooldownManager()
    {
        _currentCooldowns = new Dictionary<AudioClip, int>();
        clips = new List<AudioClip>(32);
    }

    public void UpdateCooldowns()
    {
        clips.Clear();
        clips = new List<AudioClip>(_currentCooldowns.Keys);
        foreach (AudioClip clip in clips)
        {
            _currentCooldowns[clip] = 0;
        }
    }

    public bool AddClipCooldown(AudioClip clip)
    {
        int amt = 0;
        if (!_currentCooldowns.TryGetValue(clip, out amt))
        {
            _currentCooldowns.Add(clip, 0);
        }



        if (amt > 0)
        {
            ++_currentCooldowns[clip];
            return false;
        }
        else
        {
            ++_currentCooldowns[clip];
            return true;
        }
    }
}