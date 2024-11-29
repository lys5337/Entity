using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ
{
    public class Chest : MonoBehaviour
    {
        public EndScreen endScreen;
        GameManager gameManager;
        PlayerStatsUI playerStatsUI;
        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            playerStatsUI = FindObjectOfType<PlayerStatsUI>();
        }
	    public void HandleEndScreen()
        {
            endScreen.gameObject.SetActive(true);
            endScreen.goldReward.gameObject.SetActive(false);
            endScreen.cardRewardButton.gameObject.SetActive(false);
            gameManager.nowStageRelics.chestRelics.Shuffle();
            endScreen.relicReward.gameObject.SetActive(true);
            endScreen.relicReward.DisplayRelic(gameManager.nowStageRelics.chestRelics[0]);
            gameManager.relics.Add(gameManager.nowStageRelics.chestRelics[0]);
            gameManager.nowStageRelics.chestRelics.Remove(gameManager.nowStageRelics.chestRelics[0]);
            
            if(gameManager.PlayerHasRelic("동화")&&gameManager.bronzeCoin==true)
            {
                gameManager.UpdateGoldNumber(100);
                gameManager.bronzeCoin=false;
            }

            if(gameManager.PlayerHasRelic("은화")&&gameManager.silverCoin==true)
            {
                gameManager.UpdateGoldNumber(300);
                gameManager.silverCoin=false;
            }

            if(gameManager.PlayerHasRelic("금화")&&gameManager.goldCoin==true)
            {
                gameManager.UpdateGoldNumber(1000);
                gameManager.goldCoin=false;
            }

            if(gameManager.PlayerHasRelic("딸기")&&gameManager.strawberryUse==true)
            {
                gameManager.playerMaxHealth+=10;
                gameManager.strawberryUse=false;
            }

            if(gameManager.PlayerHasRelic("사과")&&gameManager.appleUse==true)
            {
                gameManager.playerMaxHealth+=7;
                gameManager.appleUse=false;
            }

            if(gameManager.PlayerHasRelic("샤인머스캣")&&gameManager.shinemuscatUse==true)
            {
                gameManager.playerMaxHealth+=15;
                gameManager.shinemuscatUse=false;
            }

            if(gameManager.PlayerHasRelic("생명의 비약")&&gameManager.elixirUse==true)
            {
                gameManager.playerMaxHealth+=25;
                gameManager.elixirUse=false;
            }

            gameManager.TransformRelics();
            playerStatsUI.DisplayRelics();
        }
    }
}
