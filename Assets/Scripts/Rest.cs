using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ
{
    public class Rest : MonoBehaviour
    {
	    EndScreen endScreen;
        GameManager gameManager;
        PlayerStatsUI playerStatsUI;
        BattleSceneManager battleSceneManager;
        public GameObject continuebutton;
        public GameObject dontclick;
        private void Awake()
        {
            endScreen = FindObjectOfType<EndScreen>();
            gameManager = FindObjectOfType<GameManager>();
            playerStatsUI = FindObjectOfType<PlayerStatsUI>();
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
        }
        private void OnEnable()
        {
            continuebutton.SetActive(false);
        }
	    public void HandleEndScreen()
        {
            float tempval = 0.3f;
            if (gameManager.PlayerHasRelic("침낭")) tempval = 0.5f;
            battleSceneManager.player.currentHealth+=(int)(battleSceneManager.player.maxHealth*tempval);
            if(battleSceneManager.player.currentHealth>battleSceneManager.player.maxHealth)
                battleSceneManager.player.currentHealth=battleSceneManager.player.maxHealth;

            battleSceneManager.player.UpdateHealthUI(battleSceneManager.player.currentHealth);

            gameManager.DisplayHealth(battleSceneManager.player.currentHealth, battleSceneManager.player.maxHealth);
            continuebutton.SetActive(true);
        }
        public void DontClick()
        {
            dontclick.SetActive(true);
        }
        public void DoClick()
        {
            dontclick.SetActive(false);
        }
    }
}
