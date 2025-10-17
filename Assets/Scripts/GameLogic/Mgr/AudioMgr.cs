using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum Bgm {
    NONE,
    main,
    war,
    shop,
    level,
    afterwar,
    boss,
}

/// <summary>
/// 音频管理
/// </summary>
public class AudioMgr : MonoBehaviour
{
    AudioSource bgmSource;
    List<AudioSource> shotSource = new List<AudioSource>();
    List<TimeoutDir> timers = new List<TimeoutDir>();
    float bgmFadeTime = 1f;
    float noVoiceDis = 15f;
    float fullVoiceDis = 9f;
    Dictionary<Bgm, AudioClip> bgmDic = new Dictionary<Bgm, AudioClip>();
    string playerPrefabMusicName = "SetMusic";
    float _musicSetValue;
    private Bgm curBGM = Bgm.NONE;
    public float MusicSetValue
    {
        get{return _musicSetValue;}
        set { _musicSetValue = value; }
    }
    string playerPrefabVoiceName = "SetVoice";
    float _voiceSetValue;
    public float VoiceSetValue
    {
        get { return _voiceSetValue; }
        set { _voiceSetValue = value; }
    }
    private static AudioMgr _instance;
    public static AudioMgr Instance
    {
        get {
            if (_instance == null)
            {
                GameObject AudioObj = new GameObject("AudioObj");
                _instance = AudioObj.AddComponent<AudioMgr>();

            }
            return _instance;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = 0.5f;
        bgmDic.Add(Bgm.main, Resources.Load<AudioClip>("BGM/Battle with Beasts"));
        bgmDic.Add(Bgm.war, Resources.Load<AudioClip>("BGM/05 - Battle 1"));
        bgmDic.Add(Bgm.shop, Resources.Load<AudioClip>("BGM/08 - Shop"));
        bgmDic.Add(Bgm.boss, Resources.Load<AudioClip>("BGM/09 - Battle 2"));
        InitValue();
    }

    private void InitValue()
    {
        MusicSetValue = PlayerPrefs.GetFloat(playerPrefabMusicName, 0.5f);
        VoiceSetValue = PlayerPrefs.GetFloat(playerPrefabVoiceName, 1);
    }

    public void PlayBGM(string name)
    {
        bgmSource.DOFade(0, bgmFadeTime).OnComplete(()=> {
            AudioClip clip = Resources.Load<AudioClip>("BGM/" + name);
            bgmSource.DOFade(MusicSetValue, bgmFadeTime);
            if (clip != null)
            {
                bgmSource.clip = clip;
                if (!bgmSource.isPlaying)
                    bgmSource.Play();
            }
        });
    }
    public void PlayBGM(Bgm bgm, float volume = 0.5f)
    {
        if (curBGM != bgm)
        {
            curBGM = bgm;
            if (!Main.Instance.IsCaptureVedio)//录屏时不放bgm
            {
                bgmSource.DOFade(0, bgmFadeTime).OnComplete(() =>
                {
                    AudioClip clip = bgmDic[bgm];
                    bgmSource.DOFade(volume * MusicSetValue, bgmFadeTime);
                    if (clip != null)
                    {
                        bgmSource.clip = clip;
                        if (!bgmSource.isPlaying)
                            bgmSource.Play();
                    }
                });
            }
        }
    }
    public void StopBgm()
    {
        //curBGM = Bgm.NONE;
        bgmSource.Stop();
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlayClip(string name, Action cb = null, float volume = 1f)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/" + name);
        if(clip!=null)
        {
            AudioSource shotSource = gameObject.AddComponent<AudioSource>();
            shotSource.loop = false;
            shotSource.volume = volume * VoiceSetValue;
            shotSource.PlayOneShot(clip);
            float detal = clip.length;
            TimeoutDir timer = TimeManager.Instance.AddTimeOut(detal, () => {
                ClipOver(shotSource);
                if(cb!=null)
                {
                    cb.Invoke();
                }
            });
            timers.Add(timer);
        }
       
    }
    public void PlayClip(AudioClip clip, Action cb = null, float volume = 0.8f)
    {
        AudioSource shotSource = gameObject.AddComponent<AudioSource>();
        shotSource.loop = false;
        shotSource.volume = volume * VoiceSetValue;
        shotSource.PlayOneShot(clip);
        float detal = clip.length;
        TimeoutDir timer = TimeManager.Instance.AddTimeOut(detal, () => {
            ClipOver(shotSource);
            if (cb != null)
            {
                cb.Invoke();
            }
        });
        timers.Add(timer);
    }
    public void PlayClip(string name, Vector3 pos, Action cb = null, float volume = 0.8f)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/" + name);
        if (clip != null)
        {
            AudioSource shotSource = gameObject.AddComponent<AudioSource>();
            shotSource.loop = false;
            shotSource.volume = volume * VoiceSetValue;
            shotSource.PlayOneShot(clip);
            float detal = clip.length;
            TimeoutDir timer = TimeManager.Instance.AddTimeOut(detal, () => {
                ClipOver(shotSource);
                if (cb != null)
                {
                    cb.Invoke();
                }
            });
            timers.Add(timer);
        }

    }

    private void ClipOver(AudioSource shotSource)
    {
        Destroy(shotSource);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < timers.Count; i++)
        {
            TimeManager.Instance.RemoveTimeout(timers[i]);
        }
        timers.Clear();
    }

    public void SetMusicValue(float value)
    {
        MusicSetValue = value;
        bgmSource.volume = value;
        PlayerPrefs.SetFloat(playerPrefabMusicName, value);
        PlayerPrefs.Save();
    }
    public void SetVoiceValue(float value)
    {
        VoiceSetValue = value;
        PlayerPrefs.SetFloat(playerPrefabVoiceName, value);
        PlayerPrefs.Save();
    }
}
