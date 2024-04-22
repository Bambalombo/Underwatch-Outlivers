using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public float volume;
    }

    public List<Sound> sounds = new List<Sound>();
    private Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSounds();
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void InitializeSounds()
    {
        foreach (Sound sound in sounds)
        {
            soundClips[sound.name] = sound.clip;
        }
    }
    
    public static void PlaySound(string name)
    {
        if (Instance.soundClips.TryGetValue(name, out AudioClip clip))
        {
            float volume = Instance.sounds.Find(sound => sound.name == name).volume;
            Instance.audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogError("SoundManager: Sound not found. Name: " + name);
        }
    }

    
}