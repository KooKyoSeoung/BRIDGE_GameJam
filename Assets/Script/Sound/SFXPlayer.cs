using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayAudioClip(int _idx, float _volume = 1f)
    {
        int cnt = audioClips.Length;
        if (cnt - 1 < _idx || cnt == 0)
            return;
        audioSource.clip = audioClips[_idx];
        audioSource.volume = _volume;
        audioSource.Play();
    }

    public void StopAudioClip(int _idx)
    {
        int cnt = audioClips.Length;
        if (cnt - 1 < _idx || cnt == 0)
            return;
        audioSource.Stop();
    }
}
