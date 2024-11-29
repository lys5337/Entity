using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    private string currentBGM; // 현재 재생 중인 BGM 이름 저장

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioClips()
    {
        foreach (var clip in audioClips)
        {
            soundDictionary.Add(clip.name, clip);
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            // 현재 재생 중인 BGM이 동일하면 다시 재생하지 않음
            if (audioSource.isPlaying && audioSource.clip == soundDictionary[soundName] && currentBGM == soundName)
            {
                Debug.Log($"'{soundName}' 사운드가 이미 재생 중입니다.");
                return;
            }

            audioSource.Stop();
            audioSource.clip = soundDictionary[soundName];
            audioSource.loop = true;
            audioSource.Play();
            currentBGM = soundName;
        }
        else
        {
            Debug.LogWarning($"'{soundName}'이라는 사운드를 찾을 수 없습니다.");
        }
    }

    public void StopBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            currentBGM = null;
            Debug.Log("BGM이 중지되었습니다.");
        }
    }

    public string GetCurrentBGM()
    {
        return currentBGM;
    }
}