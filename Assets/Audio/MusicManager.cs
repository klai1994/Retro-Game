﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        float maxVolume = 0.5f;

        static MusicManager musicManager;
        AudioClip[] musicList;
        AudioSource audioSource;

        public static MusicManager Instance()
        {
            if (!musicManager)
            {
                musicManager = FindObjectOfType<MusicManager>();
                if (!musicManager)
                {
                    Debug.Log("No music manager found.");
                }
            }
            DontDestroyOnLoad(musicManager);
            return musicManager;
        }

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            musicList = Resources.LoadAll<AudioClip>("Music");
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = Mathf.Clamp(volume, 0, maxVolume);
        }

        // To start music without fade simply set fadeSpeed to maxVolume
        public void PlayMusic(MusicName musicName, float fadeSpeed)
        {
            audioSource.clip = musicList[(int)musicName];
            SetVolume(0);
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
            }
            yield return new WaitForSeconds(0);
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
                yield return new WaitForSeconds(0);
            }
        }

    }
}

public enum MusicName
{
    LightIntro = 0
}