using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Dictionary를 통해 사운드 이름과 AudioClip을 매핑
    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();

    public AudioSource audioSource; // AudioSource는 여기서 관리
    public List<AudioClip> audioClips; // Unity 에디터에서 추가할 AudioClip 리스트

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

    // AudioClip들을 Dictionary에 저장
    private void InitializeAudioClips()
    {
        foreach (var clip in audioClips)
        {
            soundDictionary.Add(clip.name, clip);
        }
    }

    // 사운드를 재생하는 함수
    public void PlaySound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundDictionary[soundName]);
        }
        else
        {
            Debug.LogWarning($"'{soundName}'이라는 사운드를 찾을 수 없습니다.");
        }
    }
}