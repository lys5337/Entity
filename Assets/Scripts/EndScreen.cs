using System.Collections.Generic;
using UnityEngine;

namespace TJ
{
    public class EndScreen : MonoBehaviour
    {
        public RelicRewardUI relicReward;
        public RelicRewardUI goldReward;
        public RelicRewardUI cardRewardButton;
        public GameObject dontclick;
        public List<GameObject> cards;
        public List<RelicRewardUI> cardRewards; // 카드 보상 UI 리스트
        private List<Card> displayedCards = new List<Card>(); // UI에 표시된 카드 정보를 저장할 리스트

        // 자체적으로 구성된 카드 라이브러리
        public List<Card> commonCardLibrary;
        public List<Card> uncommonCardLibrary;
        public List<Card> rareCardLibrary;
        public List<Card> epicCardLibrary;
        public List<Card> legendCardLibrary;

        public GameManager gameManager; // GameManager 인스턴스

        // 레어도별 확률 설정
        private float commonProbability = 0.6f; // 회색
        private float uncommonProbability = 0.27f; // 초록색
        private float rareProbability = 0.10f; // 파란색
        private float epicProbability = 0.025f; // 보라색
        private float legendProbability = 0.005f; // 금색

        private void Awake()
        {
            // GameData 오브젝트에서 GameManager 컴포넌트를 자동으로 찾음
            if (gameManager == null)
            {
                GameObject gameDataObject = GameObject.Find("GameData");
                if (gameDataObject != null)
                {
                    gameManager = gameDataObject.GetComponent<GameManager>();
                }

                if (gameManager == null)
                {
                    Debug.LogError("GameData 오브젝트에서 GameManager를 찾을 수 없습니다. GameManager 인스턴스를 확인하세요.");
                }
            }

            dontclick.SetActive(false);
        }

        public void DontClick()
        {
            dontclick.SetActive(true);
        }

        public void DoClick()
        {
            dontclick.SetActive(false);
        }

        public void HandleCards()
        {
            // 표시된 카드 리스트 초기화
            displayedCards.Clear();

            // 4장의 카드를 확률에 따라 중복 없이 선택하여 저장
            for (int i = 0; i < 4; i++)
            {
                Card selectedCard = SelectCardWithNoDuplicates();
                if (selectedCard != null)
                {
                    displayedCards.Add(selectedCard); // 표시된 카드 리스트에 추가
                }
            }

            // 5번째 카드는 항상 언커먼 이상의 등급으로 설정하고 중복 방지
            Card uncommonOrHigherCard = SelectUncommonOrHigherCard();
            if (uncommonOrHigherCard != null)
            {
                displayedCards.Add(uncommonOrHigherCard);
            }

            // 선택된 카드를 UI에 표시하고, 레어도에 따라 오브젝트 활성화/비활성화
            for (int i = 0; i < displayedCards.Count; i++)
            {
                cardRewards[i].gameObject.SetActive(true);
                cardRewards[i].DisplayCard(displayedCards[i]); // UI에 카드 표시

                // 카드의 레어도에 따라 RelicRewardUI 오브젝트 활성화/비활성화
                SetCardRarityUI(cardRewards[i], displayedCards[i].cardRarity);
            }
        }

        public void SelectedCard(int cardIndex)
        {
            // `displayedCards` 리스트에서 선택된 카드 가져오기
            if (cardIndex >= 0 && cardIndex < displayedCards.Count)
            {
                Card selectedCard = displayedCards[cardIndex];
                Debug.Log($"{selectedCard.cardTitle} has been selected.");

                // 선택된 카드를 GameManager의 playerDeck에 추가하는 로직
                if (gameManager != null)
                {
                    gameManager.playerDeck.Add(selectedCard);
                    Debug.Log($"{selectedCard.cardTitle} has been added to the player's deck.");
                }
                else
                {
                    Debug.LogError("GameManager is not assigned!");
                }
            }

            // 카드 선택 후 카드 UI 비활성화
            for (int i = 0; i < 5; i++)
            {
                cardRewards[i].gameObject.SetActive(false);
            }
        }

        // 중복 없는 카드 선택 로직
        private Card SelectCardWithNoDuplicates()
        {
            Card newCard = null;
            int attempts = 0;

            // 중복 방지를 위해 10번까지 새로운 카드를 시도해봄
            while (attempts < 10)
            {
                newCard = SelectCardBasedOnRarity();
                if (newCard != null && !displayedCards.Contains(newCard))
                {
                    break; // 중복되지 않는 카드를 찾으면 탈출
                }
                attempts++;
            }

            return newCard;
        }

        // 5번째 카드 선택 시 언커먼 이상 카드를 선택하고 중복 방지
        private Card SelectUncommonOrHigherCard()
        {
            Card newCard = null;
            int attempts = 0;

            // 중복 방지를 위해 10번까지 새로운 카드를 시도해봄
            while (attempts < 10)
            {
                float randomValue = Random.value;

                if (randomValue < uncommonProbability / (uncommonProbability + rareProbability + epicProbability + legendProbability))
                {
                    newCard = GetRandomCardFromList(uncommonCardLibrary);
                }
                else if (randomValue < (uncommonProbability + rareProbability) / (uncommonProbability + rareProbability + epicProbability + legendProbability))
                {
                    newCard = GetRandomCardFromList(rareCardLibrary);
                }
                else if (randomValue < (uncommonProbability + rareProbability + epicProbability) / (uncommonProbability + rareProbability + epicProbability + legendProbability))
                {
                    newCard = GetRandomCardFromList(epicCardLibrary);
                }
                else
                {
                    newCard = GetRandomCardFromList(legendCardLibrary);
                }

                if (newCard != null && !displayedCards.Contains(newCard))
                {
                    break; // 중복되지 않는 카드를 찾으면 탈출
                }
                attempts++;
            }

            return newCard;
        }

        // 일반 카드 선택 로직
        private Card SelectCardBasedOnRarity()
        {
            // 랜덤 값 생성 (0 ~ 1)
            float randomValue = Random.value;

            // 레어도별 확률에 따라 카드 선택
            if (randomValue < commonProbability)
            {
                return GetRandomCardFromList(commonCardLibrary);
            }
            else if (randomValue < commonProbability + uncommonProbability)
            {
                return GetRandomCardFromList(uncommonCardLibrary);
            }
            else if (randomValue < commonProbability + uncommonProbability + rareProbability)
            {
                return GetRandomCardFromList(rareCardLibrary);
            }
            else if (randomValue < commonProbability + uncommonProbability + rareProbability + epicProbability)
            {
                return GetRandomCardFromList(epicCardLibrary);
            }
            else
            {
                return GetRandomCardFromList(legendCardLibrary);
            }
        }

        // 수정된 카드 선택 함수
        private Card GetRandomCardFromList(List<Card> cardList)
        {
            if (cardList != null && cardList.Count > 0)
            {
                return cardList[Random.Range(0, cardList.Count)];
            }

            // 카드 리스트가 비어 있을 경우, 커먼 등급 카드에서 가져옴
            if (commonCardLibrary != null && commonCardLibrary.Count > 0)
            {
                Debug.LogWarning("요청한 카드 리스트가 비어있어 Common 리스트 에서 불러왔습니다.");
                return commonCardLibrary[Random.Range(0, commonCardLibrary.Count)];
            }

            // 모든 카드 리스트가 비어 있다면 null 반환
            Debug.LogError("All card libraries are empty!");
            return null;
        }

        private void SetCardRarityUI(RelicRewardUI rewardUI, Card.CardRarity rarity)
        {
            // 모든 레어도 오브젝트 비활성화
            rewardUI.common.SetActive(false);
            rewardUI.uncommon.SetActive(false);
            rewardUI.rare.SetActive(false);
            rewardUI.epic.SetActive(false);
            rewardUI.legendary.SetActive(false);
            rewardUI.hidden.SetActive(false);

            // 레어도에 맞는 오브젝트 활성화 (if-else 문 사용)
            if (rarity == Card.CardRarity.Common)
            {
                rewardUI.common.SetActive(true);
            }
            else if (rarity == Card.CardRarity.Uncommon)
            {
                rewardUI.uncommon.SetActive(true);
            }
            else if (rarity == Card.CardRarity.Rare)
            {
                rewardUI.rare.SetActive(true);
            }
            else if (rarity == Card.CardRarity.Epic)
            {
                rewardUI.epic.SetActive(true);
            }
            else if (rarity == Card.CardRarity.Legendary)
            {
                rewardUI.legendary.SetActive(true);
            }
            else if (rarity == Card.CardRarity.Hidden_Card)
            {
                rewardUI.hidden.SetActive(true);
            }
        }

        public void CardListReset()
        {
            for (int i = 0; i < 5; i++)
            {
                cards[i].SetActive(false);
            }
        }
    }
}
