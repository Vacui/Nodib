using System.Collections.Generic;
using UnityEngine;

public enum Sound {
    Jump,
    EnterPoint
}

[System.Serializable]
public class SoundAudioClip {
    public Sound sound;
    public AudioClip audioClip;
}

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        CreateAudioClipDictionary();
    }

    [Header("Components")]
    [SerializeField] private AudioSource EffectsSource = null;
    [SerializeField] private AudioSource MusicSource = null;

    [SerializeField] private List<SoundAudioClip> soundAudioClipArray = new List<SoundAudioClip>();
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();

    private void CreateAudioClipDictionary() {
        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach(SoundAudioClip soundAudioClip in soundAudioClipArray) {
            soundAudioClipDictionary.Add(soundAudioClip.sound, soundAudioClip.audioClip);
        }
    }

    public void PlaySound(Sound sound) {
        Debug.Log("Attempt to play sound " + sound);
        if (soundAudioClipDictionary.ContainsKey(sound)) {
            Debug.Log("Playing sound " + sound);
            EffectsSource.clip = soundAudioClipDictionary[sound];
            EffectsSource.Play();
        }
    }

    public void PlayMusic(AudioClip clip) {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

}