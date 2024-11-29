/*
using UnityEngine;

namespace TJ
{
    public static class RelicAction
    {
        public enum ActionType { BattleStart, BattleEnd }

        public static void ApplyEffect(string relicName, Player player, ActionType actionType)
        {
            switch (relicName)
            {
                case "BurningBlood":
                    if (actionType == ActionType.BattleEnd)
                    {
                        HealPlayerAfterBattle(player, 6);
                    }
                    break;

                // 추가적으로 다른 유물의 효과도 이곳에 정의할 수 있습니다.
            }
        }

        private static void HealPlayerAfterBattle(Player player, int healAmount)
        {
            player.currentHealth += healAmount;
            if (player.currentHealth > player.maxHealth)
            {
                player.currentHealth = player.maxHealth;
            }
            player.UpdateHealthUI(player.currentHealth);
            Debug.Log($"{player.name} has been healed by {healAmount} due to Burning Blood effect.");
        }
    }
}

*/