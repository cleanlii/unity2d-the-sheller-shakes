using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManeger : MonoBehaviour
{
    static AudioManeger current;

    [Header("Player音效")]
    public AudioClip SlashClips;
    public AudioClip JumpClips;
    public AudioClip ActiveClips;
    public AudioClip DeathClips;
    public AudioClip HurtClips;

    [Header("UI音效")]
    public AudioClip PickClips;
    public AudioClip MenuClips;

    [Header("Scene音效")]
    public AudioClip StartClips;
    public AudioClip GuideClips;
    public AudioClip MainClips;
    public AudioClip BossClips;
    public AudioClip ExtraClips;
    public AudioClip BattleClips;

    [Header("Ending音效")]
    public AudioClip BadClips;

    AudioSource PlayerSource;
    AudioSource MusicSource;

    private void Awake()
    {
        if (current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;

        DontDestroyOnLoad(gameObject);

        PlayerSource = gameObject.AddComponent<AudioSource>();
        MusicSource = gameObject.AddComponent<AudioSource>();
    }
    public static void closeLevelAudio()
{
    current.MusicSource.Stop();
}
    public static void PickAudio()
    {
        current.PlayerSource.clip = current.PickClips;
        current.PlayerSource.Play();
    }

    public static void playerSlashAudio()
    {

        current.PlayerSource.clip = current.SlashClips;
        current.PlayerSource.Play();

    }

    public static void playerJumpAudio()
    {

        current.PlayerSource.clip = current.JumpClips;
        current.PlayerSource.Play();

    }

    public static void playerDeathAudio()
    {
        current.PlayerSource.clip = current.DeathClips;
        current.PlayerSource.Play();
    }

    public static void playerHurtAudio()
    {
        current.PlayerSource.clip = current.HurtClips;
        current.PlayerSource.Play();
    }

    public static void MenuAudio()
    {
        current.PlayerSource.clip = current.MenuClips;
        current.PlayerSource.Play();
    }
    public static void StartAudio()
    {
        current.MusicSource.clip = current.StartClips;
        current.MusicSource.Play();
    }
    public static void GuideAudio()
    {
        current.MusicSource.loop = true;
        current.MusicSource.clip = current.GuideClips;
        current.MusicSource.volume = 0.2f;
        current.MusicSource.Play();
    }

        public static void ExtraAudio()
    {
        current.MusicSource.loop = true;
        current.MusicSource.clip = current.ExtraClips;
        current.MusicSource.volume = 0.5f;
        current.MusicSource.Play();
    }
        public static void MainAudio()
    {
        current.MusicSource.loop = true;
        current.MusicSource.clip = current.MainClips;
        current.MusicSource.Play();
    }
        public static void BossAudio()
    {
        current.MusicSource.loop = true;
        current.MusicSource.clip = current.BossClips;
        current.MusicSource.Play();
    }
        public static void BattleAudio()
    {
        current.MusicSource.loop = true;
        current.MusicSource.clip = current.BattleClips;
        current.MusicSource.Play();
    }
        public static void BadAudio()
    {
        current.MusicSource.clip = current.BadClips;
        current.MusicSource.Play();
    }

    public static void ActiveAudio()
    {

        current.PlayerSource.clip = current.ActiveClips;
        current.PlayerSource.Play();

    }
}
