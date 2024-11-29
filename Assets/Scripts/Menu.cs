using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 필요
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
    private Resolution[] resolutions = new Resolution[4];

    public GameObject settingMenu;
    public GameObject saveBtnMenu;
    public GameObject loadBtnMenu;

    public Slider volumeSlider; // 볼륨 조절 슬라이더
    public Slider bgmVolumeSlider; // BGM 볼륨 조절 슬라이더
    private AudioSource audioSource; // 일반 볼륨 조절 AudioSource
    public AudioSource bgmAudioSource; // BGMManager의 AudioSource

    public GameObject titleBTN;

    void Start()
    {
        // AudioManager 싱글톤 인스턴스의 AudioSource를 찾음
        AudioManager audioManager = AudioManager.Instance;
        if (audioManager != null)
        {
            audioSource = audioManager.GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogWarning("AudioManager에 AudioSource가 없습니다.");
            }
            else
            {
                // 볼륨 슬라이더 초기화
                if (volumeSlider != null)
                {
                    volumeSlider.value = audioSource.volume; // 초기 볼륨 설정
                    volumeSlider.onValueChanged.AddListener(SetVolume); // 슬라이더 값 변경 시 SetVolume 호출
                }
            }
        }
        else
        {
            Debug.LogError("AudioManager 싱글톤 인스턴스를 찾을 수 없습니다.");
        }

        // BGMManager의 AudioSource를 찾음
        BGMManager bgmManager = BGMManager.Instance;
        if (bgmManager != null)
        {
            bgmAudioSource = bgmManager.audioSource;

            if (bgmAudioSource == null)
            {
                Debug.LogWarning("BGMManager에 AudioSource가 없습니다.");
            }
            else
            {
                // BGM 볼륨 슬라이더 초기화
                if (bgmVolumeSlider != null)
                {
                    bgmVolumeSlider.value = bgmAudioSource.volume; // 초기 BGM 볼륨 설정
                    bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume); // 슬라이더 값 변경 시 SetBGMVolume 호출
                }
            }
        }
        else
        {
            Debug.LogError("BGMManager 싱글톤 인스턴스를 찾을 수 없습니다.");
        }

        // 지정한 네 가지 해상도를 설정
        resolutions[0] = new Resolution { width = 720, height = 480 };
        resolutions[1] = new Resolution { width = 1280, height = 720 };
        resolutions[2] = new Resolution { width = 1920, height = 1080 };
        resolutions[3] = new Resolution { width = 2560, height = 1440 };

        // titleBTN 클릭 이벤트 연결
        if (titleBTN != null)
        {
            Button titleButtonComponent = titleBTN.GetComponent<Button>();
            if (titleButtonComponent != null)
            {
                titleButtonComponent.onClick.AddListener(GoToMainScene);
            }
            else
            {
                Debug.LogWarning("titleBTN에 Button 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("titleBTN이 할당되지 않았습니다.");
        }
    }

    // "Main" 씬을 호출하는 함수
    private void GoToMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    // 해상도를 인덱스로 설정하는 함수
    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            Debug.Log($"해상도가 {resolution.width} x {resolution.height}로 설정되었습니다.");
        }
        else
        {
            Debug.LogWarning("잘못된 해상도 인덱스입니다.");
        }
    }

    // 전체 화면 모드를 설정하는 함수
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("전체 화면 모드: " + (isFullscreen ? "켜짐" : "꺼짐"));
    }

    // 일반 볼륨 설정 함수
    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume; // AudioSource의 볼륨 조절
            Debug.Log($"볼륨이 {volume * 100}%로 설정되었습니다.");
        }
        else
        {
            Debug.LogWarning("AudioSource가 설정되지 않았습니다.");
        }
    }

    // BGM 볼륨 설정 함수
    public void SetBGMVolume(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume; // BGMManager의 AudioSource 볼륨 조절
            Debug.Log($"BGM 볼륨이 {volume * 100}%로 설정되었습니다.");
        }
        else
        {
            Debug.LogWarning("BGM AudioSource가 설정되지 않았습니다.");
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenSaveBtn()
    {
        saveBtnMenu.SetActive(true);
        loadBtnMenu.SetActive(false);
        settingMenu.SetActive(false);
    }

    public void OpenLoadBtn()
    {
        saveBtnMenu.SetActive(false);
        loadBtnMenu.SetActive(true);
        settingMenu.SetActive(false);
    }

    public void Back()
    {
        saveBtnMenu.SetActive(false);
        loadBtnMenu.SetActive(false);
        settingMenu.SetActive(true);
    }
}
