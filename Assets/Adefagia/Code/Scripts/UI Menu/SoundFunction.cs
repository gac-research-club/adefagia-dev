using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundFunction : MonoBehaviour
{
    public static SoundFunction Instance;
    public Sound0[] musicSound, sfxSound;
    public AudioSource musicSource, sfxSource;

    // Song doesn't stop after change scene
    public void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    // play the sound based on string 
    public void MusicPlay(string name) {
        Sound0 s = Array.Find(musicSound, x => x.name == name);

        // Check if s have sound
        if(s == null) {
            Debug.Log("The music not found :)");
        }else {
            musicSource.clip = s.audio;
            musicSource.Play();
        }
    }

    // same as MusicPlay but sfx
    public void SfxPlay(string name) {
        Sound0 s = Array.Find(sfxSound, x => x.name == name);
        
        // Check if s have sound
        if(s == null) {
            Debug.Log("Sfx not found");
        }else {
            sfxSource.clip = s.audio;
            sfxSource.PlayOneShot(sfxSource.clip);
        }
    }

    // To mute your song
    public void MuteMusic() {
        musicSource.mute =! musicSource.mute;
    }

    // Change volume of your music
    public void MusicVolume(float volume) {
        musicSource.volume = volume;
    }

    // Play your song after enter the game
    public void Start() {
        MusicPlay("main_menu");
    }
}
