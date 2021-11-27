using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AudioManager>();
            return instance;
        }
    }

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;
    public static GameObject go;

    public static float masterVolumeSFX = 1f;
    public static float masterVolumeBGM = 1f;

    [SerializeField]
    private AudioClip mainBgmAudioClip; //BGM clip
    [SerializeField]
    private AudioClip[] sfxAudioClips; //sfx clips
    Dictionary<string, AudioClip> audioClipsDic = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        bgmPlayer = GameObject.Find("BGMSoundPlayer").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("SFXSoundPlayer").GetComponent<AudioSource>();

        foreach (AudioClip audioclip in sfxAudioClips)
        {
            audioClipsDic.Add(audioclip.name, audioclip);
        }
    }

    public void PlaySFXSound(string name, float volume = 1f)
    {
        if (audioClipsDic.ContainsKey(name) == false)
        {
            Debug.Log(name + " is not Contained audioClipsDic");
            return;
        }
        go = new GameObject("audio");
        sfxPlayer.PlayOneShot(audioClipsDic[name], volume * masterVolumeSFX);
        Destroy(go, audioClipsDic[name].length);
    }

    public void PlayBGMSound(float volume = 1f)
    {
        bgmPlayer.loop = true; //BGM loop
        bgmPlayer.volume = volume * masterVolumeBGM;
        bgmPlayer.clip = mainBgmAudioClip;
        bgmPlayer.Play();

    }

    public void setBGMVolume(float vol)
    {
        masterVolumeBGM = vol;
        bgmPlayer.volume = masterVolumeBGM * 1f;
    }

    public void setSFXVolume(float vol)
    {
        masterVolumeSFX = vol;
        sfxPlayer.volume = masterVolumeBGM * 1f;
    }
}
