using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] Sound[] array_sfx = null;
    [SerializeField] Sound[] array_bgm = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource sfxPlayer = null;
    [SerializeField] AudioSource typeWritePlayer = null;

    Dictionary<string, AudioClip> dic_BGM;
    Dictionary<string, AudioClip> dic_SFX;

    [SerializeField] float bgmVolume;
    [SerializeField] float sfxVolume;


    private void Awake()
    {
        dic_BGM = new Dictionary<string, AudioClip>();
        dic_SFX = new Dictionary<string, AudioClip>();

        foreach (Sound sound in array_bgm)
        {
            dic_BGM.Add(sound.name, sound.clip);
        }

        foreach (Sound sound in array_sfx)
        {
            dic_SFX.Add(sound.name, sound.clip);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayTypeWriteSFX(string sfxName)
    {
        if (!dic_SFX.ContainsKey(sfxName))
        {
            Debug.LogWarning("SoundManager - Sound not found: " + sfxName);
            return;
        }

        typeWritePlayer.clip = dic_SFX[sfxName];
        typeWritePlayer.volume = sfxVolume;

        typeWritePlayer.Play();
    }

    /// <summary>
    /// sfxName �̸��� SFX ���
    /// </summary>
    /// <param name="sfxName"></param>
    public void PlaySFX(string sfxName, bool newPlayer = false)
    {
        if (newPlayer)
        {
            AudioSource newSfxPlayer = gameObject.AddComponent<AudioSource>();
            newSfxPlayer.volume = sfxVolume;
            newSfxPlayer.clip = dic_SFX[sfxName];
            newSfxPlayer.loop = false;
            newSfxPlayer.Play();
        }
        else
        {


            if (!dic_SFX.ContainsKey(sfxName))
            {
                Debug.LogWarning("SoundManager - Sound not found: " + sfxName);
                return;
            }

            sfxPlayer.clip = dic_SFX[sfxName];
            sfxPlayer.volume = sfxVolume;

            sfxPlayer.Play();

        }
    }

    /// <summary>
    /// bgmName �̸��� BGM ���
    /// </summary>
    /// <param name="bgmName"></param>
    public void PlayBGM(string bgmName)
    {
        if (!dic_BGM.ContainsKey(bgmName))
        {
            Debug.LogWarning("SoundManager - Sound not found: " + bgmName);
            return;
        }



        if (bgmPlayer.clip == dic_BGM["Ending"])
        {
            if (dic_BGM[bgmName] == dic_BGM["City"] || dic_BGM[bgmName] == dic_BGM["Forest"])
            {
                return;
            }
        }
        
        if (bgmPlayer.clip == dic_BGM[bgmName])
        {
            return;
        }


        bgmPlayer.clip = dic_BGM[bgmName];
        bgmPlayer.volume = bgmVolume;

        bgmPlayer.Play();
    }

    /// <summary>
    /// BGM ����
    /// </summary>
    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    /// <summary>
    /// SFX ����
    /// </summary>
    public void StopSFX()
    {
        sfxPlayer.Stop();
    }

    /// <summary>
    /// BGM ���� ���� (0 ~ 1)
    /// </summary>
    /// <param name="volume"></param>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);

        bgmPlayer.volume = bgmVolume;
    }

    /// <summary>
    /// SFX ���� ���� (0 ~ 1)
    /// </summary>
    /// <param name="volume"></param>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume * 0.5f);

        sfxPlayer.volume = sfxVolume;
        typeWritePlayer.volume = sfxVolume;
    }

    public float SetBGMVolumeTweening(float _duration)
    {
        float volume = bgmPlayer.volume;

        var bgmTween = DOTween.To(() => bgmVolume, x => bgmVolume = x, 0f, _duration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                bgmPlayer.volume = bgmVolume;
            });

        return volume;
    }

    /// <summary>
    /// SFX ��Ͽ� �ش� SFX �ִ��� Ȯ��
    /// </summary>
    /// <param name="sfxName"></param>
    /// <returns></returns>
    public bool CheckSFXExist(string sfxName)
    {
        if (dic_SFX.ContainsKey(sfxName)) return true;
        else return false;
    }

    public bool CheckSFXPlayNow()
    {
        return sfxPlayer.isPlaying;
    }

    public bool CheckTypeWriteSFXPlayNow()
    {
        return typeWritePlayer.isPlaying;
    }

    public AudioClip GetSFXClip(string _clipName)
    {
        if (dic_SFX.ContainsKey(_clipName))
            return dic_SFX[_clipName];
        return null;
    }
}