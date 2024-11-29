using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class RandomNode_3_1 : MonoBehaviour
    {
        public Fighter player;
        public Button optionButton1;
        public Button optionButton2;
        public Text optionText1;
        public Text optionText2;

        public Button resultButton; // 결과를 표시할 버튼
        public Text resultText; // 결과 텍스트

        public int hpLossAmount = 10;
        public List<Card> possibleCardsToGain = new List<Card>(); // 얻을 수 있는 카드 목록

        private GameManager gameManager;
        private List<string> optionDescriptions = new List<string>();

        // 딜레이 후 초기화하는 코루틴
        private IEnumerator ResetAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            // 초기화: 버튼과 텍스트를 초기 상태로 복구
            resultButton.gameObject.SetActive(false);
            optionButton1.gameObject.SetActive(true);
            optionButton2.gameObject.SetActive(true);
            SetRandomOptions(); // 새로운 랜덤 옵션 설정
            resultText.text = ""; // 결과 텍스트 초기화
        }

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            InitializeOptions();
            SetRandomOptions();
            optionButton1.onClick.AddListener(() => ExecuteRandomEvent(optionText1.text));
            optionButton2.onClick.AddListener(() => ExecuteRandomEvent(optionText2.text));

            // 결과 버튼을 초기에는 비활성화
            resultButton.gameObject.SetActive(false);
            resultButton.onClick.AddListener(() => StartCoroutine(ResetAfterDelay(0.99f)));
        }

        private void InitializeOptions()
        {
            optionDescriptions.Add("녹슬고 날카로운 쇠창살을 살펴본다 (50% 확률로 아이템을 얻거나 체력을 잃음)");
            optionDescriptions.Add("습기 찬 감방 구석에 놓인 의심스러운 물체를 만져본다 (50% 확률로 능력치를 얻거나 저주를 받음)");
            optionDescriptions.Add("희미한 소리를 따라가본다");
            optionDescriptions.Add("벽에 새겨진 문양을 조사한다 (50% 확률로 단서를 얻거나 체력을 잃음)");
            optionDescriptions.Add("캄캄한 통로 속 그림자를 쫓는다 (50% 확률로 아이템을 얻거나 저주를 받음)");
            optionDescriptions.Add("벽에 남겨진 낙서를 해독한다 (50% 확률로 비밀을 알게 되거나 체력을 잃음)");
            optionDescriptions.Add("감옥 바닥에 떨어진 열쇠를 주워본다 (50% 확률로 탈출 도구를 얻거나 함정에 걸림)");
            optionDescriptions.Add("낡은 철제 상자를 열어본다 (50% 확률로 아이템을 얻거나 체력을 잃음)");
            optionDescriptions.Add("어두운 구석에서 반짝이는 물체를 발견한다 (50% 확률로 보물을 얻거나 함정에 걸림)");
            optionDescriptions.Add("깊은 감옥 구석에서 기묘한 소리를 조사한다 (50% 확률로 적과 싸우거나 보상을 얻음)");
        }

        private void SetRandomOptions()
        {
            List<string> selectedOptions = new List<string>(optionDescriptions);
            CustomListExtensions_3_1.ShuffleList_3_1(selectedOptions);

            optionText1.text = selectedOptions[0];
            optionText2.text = selectedOptions[1];
        }

        private void ExecuteRandomEvent(string selectedOption)
        {
            int randomOutcome = Random.Range(0, 2);
            string resultMessage = "";

            // 4~6 사이의 랜덤 값을 생성
            int randomTendencyValue = Random.Range(4, 7);

            if (selectedOption.Contains("녹슬고 날카로운 쇠창살"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    AdjustTendencyValue(randomTendencyValue);
                    resultMessage = $"{cardGained.cardTitle} 카드를 발견했습니다!";
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("습기 찬 감방 구석"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "능력치가 상승했습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    resultMessage = "저주를 받았습니다! 일부 스탯이 감소했습니다.";
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("희미한 소리"))
            {
                resultMessage = "소리를 따라갔지만 아무 일도 일어나지 않았습니다.";
            }
            else if (selectedOption.Contains("벽에 새겨진 문양"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "단서를 발견했습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("캄캄한 통로 속"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    AdjustTendencyValue(randomTendencyValue);
                    resultMessage = $"{cardGained.cardTitle} 카드를 얻었습니다!";
                }
                else
                {
                    resultMessage = "저주를 받았습니다!";
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("벽에 남겨진 낙서"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "비밀을 알아냈습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("감옥 바닥"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "탈출 도구를 발견했습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    resultMessage = "함정에 걸렸습니다!";
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("낡은 철제 상자"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "아이템을 얻었습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("어두운 구석"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "보물을 발견했습니다!";
                    gameManager.goldAmount += 200;
                    gameManager.UpdateGoldNumber(0);
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    resultMessage = "함정에 걸렸습니다! HP를 잃었습니다.";
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("깊은 감옥 구석"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "보상을 얻었습니다!";
                    gameManager.goldAmount += 100;
                    gameManager.UpdateGoldNumber(0);
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    resultMessage = "적과의 전투가 시작됩니다!";
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }

            // 옵션 버튼을 숨기고 결과 버튼을 활성화하여 결과 메시지를 표시
            optionButton1.gameObject.SetActive(false);
            optionButton2.gameObject.SetActive(false);

            resultText.text = resultMessage;
            resultButton.gameObject.SetActive(true);

            Debug.Log(resultMessage);
        }

        private void AdjustTendencyValue(int amount)
        {
            gameManager.currentTendencyValue = Mathf.Clamp(gameManager.currentTendencyValue + amount, 0, 100);
        }

        private Card GetRandomCardFromList(List<Card> cardList)
        {
            int randomIndex = Random.Range(0, cardList.Count);
            return cardList[randomIndex];
        }
    }

    public static class CustomListExtensions_3_1
    {
        public static void ShuffleList_3_1<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
