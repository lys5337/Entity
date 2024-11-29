using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class RandomNode_2_2 : MonoBehaviour
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
            optionDescriptions.Add("붉게 빛나는 덩굴을 조사한다 (50% 확률로 카드를 얻거나 HP를 잃음)");
            optionDescriptions.Add("신비한 버섯을 먹어본다 (50% 확률로 능력치를 얻거나 저주를 받음)");
            optionDescriptions.Add("까마귀 떼의 경로를 따라가 본다");
            optionDescriptions.Add("고목 아래서 휴식을 취한다 (50% 확률로 체력을 회복하거나 HP를 잃음)");
            optionDescriptions.Add("휘청거리는 그림자를 추적한다 (50% 확률로 카드를 얻거나 저주를 받음)");
            optionDescriptions.Add("나무 껍질에 새겨진 글자를 해독한다 (50% 확률로 비밀을 알게 되거나 HP를 잃음)");
            optionDescriptions.Add("어두운 안개 속에서 반짝이는 빛을 쫓아간다 (50% 확률로 보상을 얻거나 길을 잃음)");
            optionDescriptions.Add("녹슨 갑옷을 조사한다 (50% 확률로 장비를 얻거나 HP를 잃음)");
            optionDescriptions.Add("이끼 낀 돌무더기를 조사한다 (50% 확률로 보물을 얻거나 함정에 걸림)");
            optionDescriptions.Add("마른 나무 가지가 기괴한 소리를 내는 곳을 살펴본다 (50% 확률로 적과 전투하거나 골드를 얻음)");
        }

        private void SetRandomOptions()
        {
            List<string> selectedOptions = new List<string>(optionDescriptions);
            CustomListExtensions_2_2.ShuffleList_2_2(selectedOptions);

            optionText1.text = selectedOptions[0];
            optionText2.text = selectedOptions[1];
        }

        private void ExecuteRandomEvent(string selectedOption)
        {
            int randomOutcome = Random.Range(0, 2);
            string resultMessage = "";

            // 4~6 사이의 랜덤 값을 생성
            int randomTendencyValue = Random.Range(4, 7);

            if (selectedOption.Contains("붉게 빛나는 덩굴"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    AdjustTendencyValue(randomTendencyValue); // 긍정적인 옵션으로 성향 증가
                    resultMessage = $"{cardGained.cardTitle} 카드를 발견했습니다!";
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue); // 부정적인 옵션으로 성향 감소
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("신비한 버섯"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "능력치가 상승했습니다!";
                    AdjustTendencyValue(randomTendencyValue); // 긍정적인 옵션으로 성향 증가
                    // 예: 능력치 상승 코드 추가
                }
                else
                {
                    resultMessage = "버섯의 저주를 받았습니다! 일부 스탯이 감소했습니다.";
                    AdjustTendencyValue(-randomTendencyValue); // 부정적인 옵션으로 성향 감소
                    // 예: 스탯 감소 코드 추가
                }
            }
            else if (selectedOption.Contains("까마귀 떼"))
            {
                resultMessage = "까마귀 떼가 숲속으로 사라졌습니다. 아무 일도 일어나지 않았습니다.";
            }
            else if (selectedOption.Contains("고목 아래서 휴식"))
            {
                if (randomOutcome == 0)
                {
                    player.currentHealth += hpLossAmount;
                    if (player.currentHealth > player.maxHealth)
                        player.currentHealth = player.maxHealth;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(randomTendencyValue); // 긍정적인 옵션으로 성향 증가
                    resultMessage = $"HP가 {hpLossAmount}만큼 회복되었습니다!";
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue); // 부정적인 옵션으로 성향 감소
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("휘청거리는 그림자"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    AdjustTendencyValue(randomTendencyValue); // 긍정적인 옵션으로 성향 증가
                    resultMessage = $"{cardGained.cardTitle} 카드를 얻었습니다!";
                }
                else
                {
                    resultMessage = "저주를 받았습니다! 일부 스탯이 감소했습니다.";
                    AdjustTendencyValue(-randomTendencyValue); // 부정적인 옵션으로 성향 감소
                    // 예: 스탯 감소 코드 추가
                }
            }
            else if (selectedOption.Contains("나무 껍질에 새겨진 글자"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "비밀을 알아냈습니다! 특별한 보상을 얻습니다.";
                    AdjustTendencyValue(randomTendencyValue); // 긍정적인 옵션으로 성향 증가
                    // 예: 보상 코드 추가
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue); // 부정적인 옵션으로 성향 감소
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("어두운 안개 속"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "보상을 얻었습니다!";
                    AdjustTendencyValue(randomTendencyValue); // 긍정적인 옵션으로 성향 증가
                    // 예: 보상 코드 추가
                }
                else
                {
                    resultMessage = "길을 잃었습니다. 시간이 지체됩니다.";
                    AdjustTendencyValue(-randomTendencyValue); // 부정적인 옵션으로 성향 감소
                    // 예: 길을 잃은 이벤트 처리
                }
            }
            else if (selectedOption.Contains("녹슨 갑옷"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "녹슨 갑옷에서 장비를 얻었습니다!";
                    AdjustTendencyValue(randomTendencyValue); // 긍정적인 옵션으로 성향 증가
                    // 예: 장비 획득 코드 추가
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue); // 부정적인 옵션으로 성향 감소
                    resultMessage = $"{hpLossAmount} HP를 잃었습니다!";
                }
            }
            else if (selectedOption.Contains("이끼 낀 돌무더기"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "보물을 발견하여 골드를 얻었습니다!";
                    gameManager.goldAmount += 200;
                    gameManager.UpdateGoldNumber(0);
                    AdjustTendencyValue(randomTendencyValue); // 긍정적인 옵션으로 성향 증가
                }
                else
                {
                    resultMessage = "함정에 걸렸습니다! HP를 잃었습니다.";
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    AdjustTendencyValue(-randomTendencyValue); // 부정적인 옵션으로 성향 감소
                }
            }
            else if (selectedOption.Contains("마른 나무 가지"))
            {
                if (randomOutcome == 0)
                {
                    resultMessage = "골드를 얻었습니다!";
                    gameManager.goldAmount += 100;
                    gameManager.UpdateGoldNumber(0);
                    AdjustTendencyValue(randomTendencyValue); // 긍정적인 옵션으로 성향 증가
                }
                else
                {
                    resultMessage = "적과의 전투가 시작됩니다!";
                    AdjustTendencyValue(-randomTendencyValue); // 부정적인 옵션으로 성향 감소
                    // 예: 적과의 전투 이벤트 추가
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

    public static class CustomListExtensions_2_2
    {
        public static void ShuffleList_2_2<T>(this IList<T> list)
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
