using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class RandomNode_2_1 : MonoBehaviour
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

        public SceneChanger sceneChanger;
        private GameManager gameManager;
        private List<string> optionDescriptions = new List<string>();

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
            optionDescriptions.Add("낙오된 마차를 조사한다 (50% 확률로 카드를 얻거나 HP를 잃음)");
            optionDescriptions.Add("우물에서 물을 마신다 (50% 확률로 체력을 회복하거나 HP를 잃음)");
            optionDescriptions.Add("초원의 풀숲 속에 숨어 상황을 지켜본다");
            optionDescriptions.Add("평화로운 들판에서 휴식을 취한다 (50% 확률로 카드를 얻거나 HP를 잃음)");
            optionDescriptions.Add("야생의 동물 떼를 관찰한다 (50% 확률로 카드를 얻거나 HP를 잃음)");
            optionDescriptions.Add("낡은 비석을 조사한다 (50% 확률로 카드를 얻거나 저주를 받음)");
            optionDescriptions.Add("수상한 상인을 만난다 (상점 노드를 활성화 시킨다.)");
            optionDescriptions.Add("은빛 나비를 쫓아간다 (50% 확률로 카드를 얻거나 길을 잃음)");
            optionDescriptions.Add("작은 불꽃을 발견한다 (50% 확률로 체력을 회복하거나 더 큰 위협을 맞이함)");
            optionDescriptions.Add("잃어버린 보물 상자를 발견한다 (50% 확률로 골드를 얻거나 HP를 잃음)");
        }

        private void SetRandomOptions()
        {
            List<string> selectedOptions = new List<string>(optionDescriptions);
            CustomListExtensions_2_1.ShuffleList_2_1(selectedOptions);

            optionText1.text = selectedOptions[0];
            optionText2.text = selectedOptions[1];
        }

        private void ExecuteRandomEvent(string selectedOption)
        {
            int randomOutcome = Random.Range(0, 2);
            string resultMessage = "";

            int randomTendencyValue = Random.Range(4, 7);

            if (selectedOption.Contains("낙오된 마차"))
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
            else if (selectedOption.Contains("우물에서 물을 마신다"))
            {
                if (randomOutcome == 0)
                {
                    player.currentHealth += hpLossAmount;
                    if (player.currentHealth > player.maxHealth)
                        player.currentHealth = player.maxHealth;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(randomTendencyValue);
                    resultMessage = $"HP가 {hpLossAmount}만큼 회복되었습니다!";
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("풀숲 속에 숨어"))
            {
                resultMessage = "아무 일도 일어나지 않았습니다.";
            }
            else if (selectedOption.Contains("들판에서 휴식"))
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
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("야생의 동물 떼를 관찰"))
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
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue);
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("낡은 비석을 조사한다"))
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
                    resultMessage = "저주를 받았습니다! 일부 스탯이 감소했습니다.";
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("수상한 상인"))
            {
                sceneChanger.SelectScreen("Shop");
                AdjustTendencyValue(randomTendencyValue);
                StartCoroutine(ResetAfterDelay(0.99f));
                return;
            }
            else if (selectedOption.Contains("은빛 나비"))
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
                    resultMessage = "길을 잃었습니다. 시간이 지체됩니다.";
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("작은 불꽃"))
            {
                if (randomOutcome == 0)
                {
                    player.currentHealth += hpLossAmount;
                    if (player.currentHealth > player.maxHealth)
                        player.currentHealth = player.maxHealth;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(randomTendencyValue);
                    resultMessage = $"HP가 {hpLossAmount}만큼 회복되었습니다!";
                }
                else
                {
                    resultMessage = "불꽃이 커지며 더 큰 위협을 맞닥뜨렸습니다!";
                    AdjustTendencyValue(-randomTendencyValue);
                }
            }
            else if (selectedOption.Contains("보물 상자"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "보물 상자에서 골드를 얻었습니다!";
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

    public static class CustomListExtensions_2_1
    {
        public static void ShuffleList_2_1<T>(this IList<T> list)
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
