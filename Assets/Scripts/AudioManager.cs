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
            // 현재 재생 중인 사운드가 같으면 재생 중단 방지
            if (audioSource.isPlaying && audioSource.clip == soundDictionary[soundName])
            {
                Debug.Log($"'{soundName}' 사운드가 이미 재생 중입니다.");
                return;
            }

            audioSource.Stop(); // 이전에 재생 중인 사운드 중지
            audioSource.clip = soundDictionary[soundName];
            audioSource.loop = false; // 필요 시 설정
            audioSource.Play(); // 새로운 사운드 재생
        }
        else
        {
            Debug.LogWarning($"'{soundName}'이라는 사운드를 찾을 수 없습니다.");
        }
    }

    public void PlaySoundFromTime(string soundName, float startTime = 0f)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            audioSource.Stop(); // 이전 사운드 중지
            audioSource.clip = soundDictionary[soundName];

            if (startTime > 0f && startTime < audioSource.clip.length)
            {
                audioSource.time = startTime; // 지정된 시작 지점 설정
            }
            else
            {
                audioSource.time = 0f; // 범위 외일 경우 0초부터 재생
            }

            audioSource.loop = true; // 필요 시 설정
            audioSource.Play(); // 새로운 사운드 재생
        }
        else
        {
            Debug.LogWarning($"'{soundName}'이라는 사운드를 찾을 수 없습니다.");
        }
    }


}