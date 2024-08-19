using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogScreen;  // DialogScreen 오브젝트 참조
    private GameObject text1;
    private GameObject text2;
    private GameObject text3;
    private GameObject text4;

    void Start()
    {
        // DialogScreen 내부의 텍스트 오브젝트 찾기
        text1 = DialogScreen.transform.Find("text1").gameObject;
        text2 = DialogScreen.transform.Find("text2").gameObject;
        text3 = DialogScreen.transform.Find("text3").gameObject;
        text4 = DialogScreen.transform.Find("text4").gameObject;

        // 초기 상태에서 모든 텍스트 비활성화
        text1.SetActive(false);
        text2.SetActive(false);
        text3.SetActive(false);
        text4.SetActive(false);
    }

    public void ActivateDialogueSet1()
    {
        text1.SetActive(true);
        text2.SetActive(true);
        text3.SetActive(false);
        text4.SetActive(false);
    }

    public void ActivateDialogueSet2()
    {
        text1.SetActive(false);
        text2.SetActive(false);
        text3.SetActive(true);
        text4.SetActive(true);
    }

    // 예시용 업데이트 로직 (실제로는 Map의 노드 상태에 따라 호출되어야 함)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateDialogueSet1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateDialogueSet2();
        }
    }
}