using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM再生用
/// </summary>
public class BgmSpeaker : SingletonMonoBehaviour<BgmSpeaker>
{
    [SerializeField]
    AudioClip defaultBgm = null;

    [SerializeField]
    AudioClip ChaseBgm = null;

    AudioSource audioSource = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void EntryPoint()
    {
        DontDestroyOnLoad(Instantiate(Resources.Load("Prefabs/Audios/BgmSpeaker")));
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        PlayDefaultBgm();
    }

    public void PlayDefaultBgm()
    {
        audioSource.Stop();
        audioSource.clip = defaultBgm;
        audioSource.Play();
    }

    public void PlayChaseBgm()
    {
        Debug.Log("on");

        audioSource.Stop();
        audioSource.clip = ChaseBgm;
        audioSource.Play();
    }
}
