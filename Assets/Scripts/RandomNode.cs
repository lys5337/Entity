using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class RandomNode : MonoBehaviour
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
            optionDescriptions.Add("보물 상자를 연다 (50% 확률로 카드를 얻거나 HP를 잃음)");
            optionDescriptions.Add("수상한 인물의 제안을 받아들인다 (50% 확률로 카드를 얻거나 카드를 잃음)");
            optionDescriptions.Add("그냥 지나간다");
            optionDescriptions.Add("전투를 준비한다 (50% 확률로 카드를 얻거나 HP를 잃음)");
            optionDescriptions.Add("신비한 장소를 조사한다 (50% 확률로 카드를 얻거나 HP를 잃음)");
        }

        private void SetRandomOptions()
        {
            List<string> selectedOptions = new List<string>(optionDescriptions);
            CustomListExtensions.ShuffleList(selectedOptions);

            optionText1.text = selectedOptions[0];
            optionText2.text = selectedOptions[1];
        }

        private void ExecuteRandomEvent(string selectedOption)
        {
            int randomOutcome = Random.Range(0, 2);
            string resultMessage = "";

            if (selectedOption.Contains("보물 상자"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    resultMessage = $"{cardGained.cardTitle} 카드를 얻었습니다!";
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("수상한 인물"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    resultMessage = $"{cardGained.cardTitle} 카드를 얻었습니다!";
                }
                else
                {
                    Card cardLost = RemoveRandomCardFromDeck();
                    resultMessage = $"{cardLost.cardTitle} 카드를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("그냥 지나간다"))
            {
                resultMessage = "아무 일도 일어나지 않았습니다.";
            }
            else if (selectedOption.Contains("전투를 준비한다"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    resultMessage = $"{cardGained.cardTitle} 카드를 얻었습니다!";
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("신비한 장소를 조사한다"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    resultMessage = $"{cardGained.cardTitle} 카드를 얻었습니다!";
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }

            // 옵션 버튼을 숨기고 결과 버튼을 활성화하여 결과 메시지를 표시
            optionButton1.gameObject.SetActive(false);
            optionButton2.gameObject.SetActive(false);

            resultText.text = resultMessage;
            resultButton.gameObject.SetActive(true);

            Debug.Log(resultMessage);
        }

        private Card GetRandomCardFromList(List<Card> cardList)
        {
            int randomIndex = Random.Range(0, cardList.Count);
            return cardList[randomIndex];
        }

        private Card RemoveRandomCardFromDeck()
        {
            if (gameManager.playerDeck.Count == 0)
            {
                Debug.Log("플레이어 덱에 카드가 없습니다.");
                return null;
            }

            int randomIndex = Random.Range(0, gameManager.playerDeck.Count);
            Card cardToRemove = gameManager.playerDeck[randomIndex];
            gameManager.playerDeck.Remove(cardToRemove);
            return cardToRemove;
        }
    }

    public static class CustomListExtensions
    {
        public static void ShuffleList<T>(this IList<T> list)
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
