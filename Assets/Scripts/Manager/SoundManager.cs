using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource playerSource;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField]
    private List<Sound> sounds;

    private Dictionary<string, AudioClip> soundDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (Sound sound in sounds)
        {
            soundDictionary[sound.name] = sound.clip;
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundDictionary[soundName]);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    public void PlayMusic(string musicName)
    {
        if (soundDictionary.ContainsKey(musicName))
        {
            musicSource.clip = soundDictionary[musicName];
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music '{musicName}' not found!");
        }
    }

    public void PlayPlayerSound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundDictionary[soundName]);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }
}
