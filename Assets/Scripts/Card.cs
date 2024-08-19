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
        public CardDescription cardDescription;
        public CardAmount cardCost;
        public CardAmount cardEffect;
        public CardAmount buffAmount;
        public Sprite cardIcon;
        public CardType cardType;
        public enum CardType { Attack, Skill, Power }
        public CardClass cardClass;
        public enum CardClass { ironChad, silent, colorless, curse, status }
        public CardTargetType cardTargetType;
        public enum CardTargetType { self, enemy };

        // 기본 값을 초기화하는 메서드
        public void Initialize()
        {
            isUpgraded = false;

            // 초기화 시 baseAmount를 기본으로 사용하게 설정
            // 필요한 경우 다른 필드를 초기화할 수도 있습니다.
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
        public string baseAmount;
        public string upgradedAmount;
    }

    [System.Serializable]
    public struct CardBuffs
    {
        public Buff.Type buffType;
        public CardAmount buffAmount;
    }
}
