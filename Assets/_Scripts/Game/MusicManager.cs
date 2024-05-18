using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    float maxVolume = 0.5f;

    static MusicManager musicManager;
    Dictionary<string, AudioClip> musicAtlas;
    AudioSource audioSource;

    public static MusicManager Instance()
    {
        if (!musicManager)
        {
            musicManager = FindObjectOfType<MusicManager>();
            if (!musicManager)
            {
                Debug.LogError("No music manager found.");
            }
        }
        DontDestroyOnLoad(musicManager);
        return musicManager;
    }

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioClip[] musicList = Resources.LoadAll<AudioClip>("Music");
        musicAtlas = new Dictionary<string, AudioClip>();

        for (int i = 0; i < musicList.Length; i++)
        {
            musicAtlas.Add(musicList[i].name, musicList[i]);
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp(volume, 0, maxVolume);
    }

    // To start music without fade simply set fadeSpeed to maxVolume
    public void PlayMusic(MusicName musicName, float fadeSpeed)
    {
        SetVolume(0);
        audioSource.clip = musicAtlas[musicName.ToString()];
        audioSource.Play();
        StartCoroutine(FadeInMusic(fadeSpeed));
    }

    // To stop music immediately simply set fadeSpeed to maxVolume
    public void StopMusic(float fadeSpeed)
    {
        StartCoroutine(FadeMusicOut(fadeSpeed));
    }

    IEnumerator FadeInMusic(float fadeSpeed)
    {
        while (audioSource.volume <= maxVolume)
        {
            audioSource.volume += fadeSpeed * Time.deltaTime;
            if (audioSource.volume > maxVolume)
            {
                audioSource.volume = maxVolume;
                break;
            }
            yield return null;
        }
    }

    IEnumerator FadeMusicOut(float fadeSpeed)
    {
        while (audioSource.volume >= 0)
        {
            audioSource.volume -= fadeSpeed * Time.deltaTime;

            if (audioSource.volume == 0)
            {
                break;
            }
            yield return null;
        }
    }

}

public enum MusicName
{
    LightIntro
}
