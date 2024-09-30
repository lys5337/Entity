using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ
{
    [CreateAssetMenu]
    public class Card : ScriptableObject
    {
        public string cardTitle;
        public bool isUpgraded;
        public bool expiring;
        public CardDescription cardDescription;
        public CardAmount cardCost;
        public CardAmount cardEffect;
        public CardAmount buffAmount;
        public CardPrice cardPrice;
        public Sprite cardIcon;
        public CardType cardType;
        public enum CardType { Attack, Skill, Power }
        public CardClass cardClass;
        public enum CardClass { Warrior, Archer }
        public CardTargetType cardTargetType;
        public enum CardTargetType { enemy, self };
        public CardRarity cardRarity;
        public enum CardRarity { Common, Uncommon, Rare, Epic, Legendary, Hidden_Card, Ultimate_Skill } // 카드 레어도 설정
        public CardAttribute cardAttribute;
        public enum CardAttribute { Non, Darkest, Divine } // 카드 속성 설정
        public string CardUniqueCode;
        

        // 기본 값을 초기화하는 메서드
        public void Initialize()
        {
            isUpgraded = false;

            // 초기화 시 baseAmount를 기본으로 사용하게 설정
            // 필요한 경우 다른 필드를 초기화할 수도 있습니다.
        }

        public string GetCardUniqueCode()
        {
            return CardUniqueCode;
        }


        public int GetCardCostAmount()
        {
            if (!isUpgraded)
                return cardCost.baseAmount;
            else
                return cardCost.upgradedAmount;
        }

        public int GetCardEffectAmount()
        {
            if (!isUpgraded)
                return cardEffect.baseAmount;
            else
                return cardEffect.upgradedAmount;
        }

        public string GetCardRarity()
        {
            return cardRarity.ToString();
        }
        
        public string GetCardType()
        {
            return cardType.ToString();
        }

        public string GetCardDescriptionAmount()
        {
            if (!isUpgraded)
                return cardDescription.baseAmount;
            else
                return cardDescription.upgradedAmount;
        }

        public int GetBuffAmount()
        {
            if (!isUpgraded)
                return buffAmount.baseAmount;
            else
                return buffAmount.upgradedAmount;
        }

        // 카드 강화 메서드
        public void Upgrade()
        {
            if (!isUpgraded)
            {
                isUpgraded = true;

                // 강화 로직 추가
                // 여기서 카드를 강화할 때 추가적인 변화가 필요하면 코드를 추가할 수 있습니다.
                // 예: cardEffect.upgradedAmount += 10;
            }
        }
    }
    
    [System.Serializable]
    public struct CardAmount
    {
        public int baseAmount;
        public int upgradedAmount;
    }

    [System.Serializable]
    public struct CardDescription
    {
        [TextArea(3, 5)] // 텍스트 입력 필드를 3줄에서 최대 5줄까지 확장
        public string baseAmount;

        [TextArea(3, 5)] // 텍스트 입력 필드를 3줄에서 최대 5줄까지 확장
        public string upgradedAmount;
    }

    [System.Serializable]
    public struct CardBuffs
    {
        public Buff.Type buffType;
        public CardAmount buffAmount;
    }

    [System.Serializable]
    public struct CardPrice
    {
        public int CardShopPrice;
        public int CardEnhancePrice;
    }
}