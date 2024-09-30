using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScriptOnOff : MonoBehaviour
{
    public List<GameObject> panels; // Panel 오브젝트들을 담을 리스트
    public Button disableButton; // 버튼
    private int currentPanelIndex = 0; // 현재 비활성화할 패널의 인덱스

    private void Start()
    {
        // 버튼 클릭 시 DisableNextPanel 함수 실행
        if (disableButton != null)
        {
            disableButton.onClick.AddListener(DisableNextPanel);
        }
    }

    // 패널을 하나씩 끄는 함수
    public void DisableNextPanel()
    {
        if (currentPanelIndex < panels.Count)
        {
            panels[currentPanelIndex].SetActive(false); // 현재 패널 비활성화
            currentPanelIndex++; // 다음 패널로 이동

            // 모든 패널을 비활성화한 후 버튼도 비활성화
            if (currentPanelIndex >= panels.Count)
            {
                disableButton.gameObject.SetActive(false); // 버튼 비활성화
            }
        }
        else
        {
            Debug.Log("모든 패널이 이미 비활성화되었습니다.");
        }
    }
}
