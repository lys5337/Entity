using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogScreen;  // DialogScreen ������Ʈ ����
    private GameObject text1;
    private GameObject text2;
    private GameObject text3;
    private GameObject text4;

    void Start()
    {
        // DialogScreen ������ �ؽ�Ʈ ������Ʈ ã��
        text1 = DialogScreen.transform.Find("text1").gameObject;
        text2 = DialogScreen.transform.Find("text2").gameObject;
        text3 = DialogScreen.transform.Find("text3").gameObject;
        text4 = DialogScreen.transform.Find("text4").gameObject;

        // �ʱ� ���¿��� ��� �ؽ�Ʈ ��Ȱ��ȭ
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

    // ���ÿ� ������Ʈ ���� (�����δ� Map�� ��� ���¿� ���� ȣ��Ǿ�� ��)
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