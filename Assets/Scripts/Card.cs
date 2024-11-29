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
        public CardAditionalValue aditionalValue;
        public CardAditionalValueFloat aditionalValueFloat;
        public CardAditionalValueFloat_ADD aditionalValueFloat_ADD;
        public CardPrice cardPrice;
        public Sprite cardIcon;
        public CardType cardType;
        public enum CardType { Attack, Skill, Power }
        public CardTargetType cardTargetType;
        public enum CardTargetType { enemy, self };
        public CardRarity cardRarity;
        public enum CardRarity { Common, Uncommon, Rare, Epic, Legendary, Hidden_Card, Ultimate_Skill } // 카드 레어도 설정
        public CardDetailDescription cardDetailDescription;
        

        // 기본 값을 초기화하는 메서드
        public void Initialize()
        {
            isUpgraded = false;
        }

        public bool IsCardUpgraded()
        {
            return isUpgraded;
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

        public int GetAditionalValueAmount()
        {
            if (!isUpgraded)
                return aditionalValue.aditionalValueAmount;
            else
                return aditionalValue.upgradeAditionalValueAmount;
        }

        public float GetAditionalValueAmountFloat()
        {
            if (!isUpgraded)
                return aditionalValueFloat.aditionalValueAmountFloat;
            else
                return aditionalValueFloat.upgradeAditionalValueAmountFloat;
        }

        public float GetAditionalValueAmountFloat_ADD()
        {
            if (!isUpgraded)
                return aditionalValueFloat_ADD.aditionalValueAmountFloat_Add;
            else
                return aditionalValueFloat_ADD.upgradeAditionalValueAmountFloat_Add;
        }


        public int CalculateTotalDamage(Fighter player)
        {
            int baseDamage = GetCardEffectAmount(); // 기본 또는 업그레이드된 대미지
            int strengthMultiplier = isUpgraded ? 3 : 2; // 업그레이드 여부에 따라 힘의 배율 결정

            return baseDamage + (player.strength.buffValue * strengthMultiplier);
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
    public struct CardDescription
    {
        [TextArea(3, 5)] // 텍스트 입력 필드를 3줄에서 최대 5줄까지 확장
        public string baseAmount;

        [TextArea(3, 5)] // 텍스트 입력 필드를 3줄에서 최대 5줄까지 확장
        public string upgradedAmount;
    }

    [System.Serializable]
    public struct CardAmount
    {
        public int baseAmount;
        public int upgradedAmount;
    }

    [System.Serializable]
    public struct CardBuffs
    {
        public Buff.Type buffType;
        public CardAmount buffAmount;
    }

    [System.Serializable]
    public struct CardAditionalValue
    {
        public int aditionalValueAmount;
        public int upgradeAditionalValueAmount;
    }

    [System.Serializable]
    public struct CardAditionalValueFloat
    {
        public float aditionalValueAmountFloat;
        public float upgradeAditionalValueAmountFloat;
    }

    [System.Serializable]
    public struct CardAditionalValueFloat_ADD
    {
        public float aditionalValueAmountFloat_Add;
        public float upgradeAditionalValueAmountFloat_Add;
    }

    [System.Serializable]
    public struct CardPrice
    {
        public int cardShopPrice;
        public int cardEnhancePrice;
    }

    [System.Serializable]
    public struct CardDetailDescription
    {
        [TextArea(5, 5)]
        public string cardDetailDescription;
    }
}