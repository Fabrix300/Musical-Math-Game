using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    public Sound[] CNote;
    public Sound[] DNote;
    public Sound[] ENote;
    public Sound[] FNote;
    public Sound[] GNote;
    public Sound[] ANote;
    public Sound[] BNote;
    public Sound[] COctaveNote;
    public List<Sound[]> musicalNotes = new();

    private GameManager gameManager;
    
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
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        musicalNotes.Add(CNote);
        musicalNotes.Add(DNote);
        musicalNotes.Add(ENote);
        musicalNotes.Add(FNote);
        musicalNotes.Add(GNote);
        musicalNotes.Add(ANote);
        musicalNotes.Add(BNote);
        musicalNotes.Add(COctaveNote);
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        Play(gameManager.savedSceneName);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public IEnumerator Crossfade(string song1, string song2)
    {
        Sound s = Array.Find(sounds, sound => sound.name == song1);
        Sound s2 = Array.Find(sounds, sound => sound.name == song2);
        float currentTime = 0;
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            s.source.volume = Mathf.Lerp(0.3f, 0f, currentTime / 1f);
            yield return null;
        }
        s.source.Stop();
        s.source.volume = 0.3f;
        s2.source.Play();
    }
}
