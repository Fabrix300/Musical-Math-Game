using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float musicVolume;
    public float sfxVolume;

    public Sound[] songs;
    public Sound[] sounds;

    [Header("PlayerSingingNotes")]
    public Sound[] CNote;
    public Sound[] DNote;
    public Sound[] ENote;
    public Sound[] FNote;
    public Sound[] GNote;
    public Sound[] ANote;
    public Sound[] BNote;
    public Sound[] COctaveNote;
    public List<Sound[]> musicalNotes = new();

    [Header("EnemySingingNotes")]
    public Sound[] ECNote;
    public Sound[] EDNote;
    public Sound[] EENote;
    public Sound[] EFNote;
    public Sound[] EGNote;
    public Sound[] EANote;
    public Sound[] EBNote;
    public Sound[] ECOctaveNote;
    public List<Sound[]> enemyMusicalNotes = new();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            /*En este if poner los nombres de los sonidos a los cuales no se les asignará nada*/
            if (s.name != "Jump" || s.name != "CherryEat" || s.name != "CharacterExplosion")
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = sfxVolume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
            }
        }

        foreach (Sound s in songs)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = musicVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }

        musicalNotes.Add(CNote);
        musicalNotes.Add(DNote);
        musicalNotes.Add(ENote);
        musicalNotes.Add(FNote);
        musicalNotes.Add(GNote);
        musicalNotes.Add(ANote);
        musicalNotes.Add(BNote);
        musicalNotes.Add(COctaveNote);

        enemyMusicalNotes.Add(ECNote);
        enemyMusicalNotes.Add(EDNote);
        enemyMusicalNotes.Add(EENote);
        enemyMusicalNotes.Add(EFNote);
        enemyMusicalNotes.Add(EGNote);
        enemyMusicalNotes.Add(EANote);
        enemyMusicalNotes.Add(EBNote);
        enemyMusicalNotes.Add(ECOctaveNote);
    }

    /*private void Start()
    {
        //gameManager = GameManager.instance;
        //Play(gameManager.savedSceneName);
    }*/

    public void PlaySong(string name)
    {
        Sound s = Array.Find(songs, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Song: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Song: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public AudioClip GetAudioClip(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.clip;
    }

    public IEnumerator Crossfade(string song1, string song2)
    {
        Sound s = Array.Find(songs, sound => sound.name == song1);
        Sound s2 = Array.Find(songs, sound => sound.name == song2);
        float currentTime = 0;
        float sourceVolume = s.source.volume;
        while (currentTime < 1f)
        {
            s.source.volume = Mathf.Lerp(sourceVolume, 0f, currentTime / 1f);
            currentTime += Time.deltaTime;
            yield return null;
        }
        s.source.Stop();
        s.source.volume = sourceVolume;
        s2.source.Play();
    }

    public IEnumerator FadeVolumeDown(string song)
    {
        Sound s = Array.Find(songs, sound => sound.name == song);
        float currentTime = 0;
        float sourceVolume = s.source.volume;
        while (currentTime < 0.5f)
        {
            s.source.volume = Mathf.Lerp(sourceVolume, sourceVolume * 0.3f, currentTime / 0.5f);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeVolumeUp(string song)
    {
        Sound s = Array.Find(songs, sound => sound.name == song);
        float currentTime = 0;
        float sourceVolume = s.source.volume;
        float targetVolume = musicVolume;
        while (currentTime < 0.5f)
        {
            s.source.volume = Mathf.Lerp(sourceVolume, targetVolume, currentTime / 0.5f);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    // To be used in pause menu
    public void AdjustMusicVolume(float _musicVolume)
    {
        musicVolume = _musicVolume;
        foreach (Sound s in songs)
        {
            if (s.source) s.source.volume = musicVolume;
        }
    }

    public void AdjustSfxVolume(float _sfxVolume)
    {
        sfxVolume = _sfxVolume;
        foreach (Sound s in sounds)
        {
            if (s.source) s.source.volume = sfxVolume;
        }
    }
}
