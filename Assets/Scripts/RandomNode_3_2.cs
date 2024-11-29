using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class RandomNode_3_2 : MonoBehaviour
    {
        public Fighter player;
        public Button optionButton1;
        public Button optionButton2;
        public Text optionText1;
        public Text optionText2;

        public Button resultButton; // 결과를 표시할 버튼
        public Text resultText; // 결과 텍스트

        public int hpLossAmount = 15;
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
            optionDescriptions.Add("부서진 방패를 뒤져본다 (50% 확률로 아이템을 얻거나 체력을 잃음)");
            optionDescriptions.Add("전장의 피묻은 검을 살펴본다 (50% 확률로 카드를 얻거나 저주를 받음)");
            optionDescriptions.Add("불타는 전장 한가운데로 뛰어든다 (50% 확률로 보상을 얻거나 큰 피해를 입음)");
            optionDescriptions.Add("전사들의 시체를 조사한다 (50% 확률로 장비를 얻거나 저주를 받음)");
            optionDescriptions.Add("적군의 깃발 아래서 전투의 흔적을 살핀다");
            optionDescriptions.Add("전장에서 버려진 물자 더미를 뒤진다 (50% 확률로 보물을 얻거나 체력을 잃음)");
            optionDescriptions.Add("피에 젖은 전투 기록을 읽어본다 (50% 확률로 단서를 얻거나 HP를 잃음)");
            optionDescriptions.Add("칼날이 부러진 검을 만져본다 (50% 확률로 장비를 얻거나 큰 피해를 입음)");
            optionDescriptions.Add("적의 진영 근처를 탐색한다 (50% 확률로 골드를 얻거나 적에게 발각됨)");
            optionDescriptions.Add("불타는 나무 뒤에 숨겨진 무언가를 찾아본다 (50% 확률로 아이템을 얻거나 저주를 받음)");
        }

        private void SetRandomOptions()
        {
            List<string> selectedOptions = new List<string>(optionDescriptions);
            CustomListExtensions_3_2.ShuffleList_3_2(selectedOptions);

            optionText1.text = selectedOptions[0];
            optionText2.text = selectedOptions[1];
        }

        private void ExecuteRandomEvent(string selectedOption)
        {
            int randomOutcome = Random.Range(0, 2);
            string resultMessage = "";

            // 5~8 사이의 랜덤 값을 생성
            int randomTendencyValue = Random.Range(5, 9);

            if (selectedOption.Contains("부서진 방패"))
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
            else if (selectedOption.Contains("피묻은 검"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "새로운 카드를 얻었습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    resultMessage = "저주를 받았습니다!";
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("불타는 전장"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "귀중한 보상을 발견했습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    player.currentHealth -= hpLossAmount * 2;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount * 2} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("전사들의 시체"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "장비를 발견했습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    resultMessage = "저주를 받았습니다!";
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("물자 더미"))
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
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("칼날이 부러진 검"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "장비를 발견했습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    player.currentHealth -= hpLossAmount * 2;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount * 2} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("적의 진영"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "골드를 발견했습니다!";
                    gameManager.goldAmount += 100;
                    gameManager.UpdateGoldNumber(0);
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    resultMessage = "적에게 발각되었습니다! 체력을 잃습니다.";
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("불타는 나무"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "아이템을 발견했습니다!";
                    AdjustTendencyValue(randomTendencyValue);
                }
                else
                {
                    resultMessage = "저주를 받았습니다!";
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

    public static class CustomListExtensions_3_2
    {
        public static void ShuffleList_3_2<T>(this IList<T> list)
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
