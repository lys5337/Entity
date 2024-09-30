using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TJ
{
    public class SceneChanger : MonoBehaviour
    {
        public GameObject titleScene;
        public GameObject nameScene;
        public GameObject classSelectionScreen;
        public GameObject ModeSelectScreen;
        public GameObject battleScene;
        public GameObject chestScene;
        public GameObject restScene;
        public GameObject idleScene;
        public GameObject mapScene;
        public GameObject nextStageScreen;
        public GameObject shopScene;
        public GameObject randomnodeScene;
        public GameObject menuScene;
        public GameObject eventScene1;
        public GameObject eventScene2;
        public GameObject eventScene3;
        public GameObject eventScene4;
        public GameObject eventScene5;
        public GameObject eventScene6;
        public GameObject eventScene7;
        public GameObject eventScene8;
        public GameObject eventScene9;
        public GameObject eventScene10;

        [Header("UI")]
        public Image splashArt;
        public GameObject classSelectionObjects;

        [Header("Character Select")]
        public List<Character> characters;
        public Character selectedCharacter;
        public GameObject playerIcon;
        GameManager gameManager;
        BattleSceneManager battleSceneManager;
        EndScreen endScreen;
        SceneFader sceneFader;
        private void Awake()
        {
            // "GameData" 오브젝트를 찾아서 GameManager 컴포넌트를 가져옴
            GameObject gameDataObject = GameObject.Find("GameData");
            if (gameDataObject != null)
            {
                gameManager = gameDataObject.GetComponent<GameManager>();
            }
            else
            {
                Debug.LogError("GameData 오브젝트를 찾을 수 없습니다.");
            }

            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            endScreen = FindObjectOfType<EndScreen>();
            sceneFader = FindObjectOfType<SceneFader>();
            menuScene.SetActive(false);
        }
        public void StartScript()
        {
            eventScene10.SetActive(false);
        }

        public void PlayButton()
        {
            titleScene.SetActive(false);
        }
        public void SetnameButton()
        {
            nameScene.SetActive(false);
        }
        public void SelectClass(int i)
        {
            selectedCharacter = characters[i];
            splashArt.sprite = selectedCharacter.splashArt;
        }
        public void Embark()
        {
            //gameManager.character = selectedCharacter;

            StartCoroutine(LoadScene("Map"));
            gameManager.LoadCharacterStats();
        }
        public void Normal()
        {
            ModeSelectScreen.SetActive(false);
        }
        public void Story()
        {
            SceneManager.LoadScene("Stage0");
        }
        public void NextStage()
        {
            if (SceneManager.GetActiveScene().name == "Stage0")
                SceneManager.LoadScene("Stage1");
        }
        public void Menuopen()
        {
            menuScene.SetActive(true);
            Transform Savebtn = menuScene.transform.Find("Savebtn");
            Savebtn.gameObject.SetActive(false);
            Transform Loadbtn = menuScene.transform.Find("Loadbtn");
            Loadbtn.gameObject.SetActive(false);
            Transform Save = menuScene.transform.Find("Save");
            Save.gameObject.SetActive(true);
            Transform Load = menuScene.transform.Find("Load");
            Load.gameObject.SetActive(true);
        }
        public void Menuclose()
        {
            menuScene.SetActive(false);
        }
        public void Save()
        {
            Transform Savebtn = menuScene.transform.Find("Savebtn");
            Savebtn.gameObject.SetActive(true);
            Transform Loadbtn = menuScene.transform.Find("Loadbtn");
            Loadbtn.gameObject.SetActive(false);
            Transform Save = menuScene.transform.Find("Save");
            Save.gameObject.SetActive(false);
            Transform Load = menuScene.transform.Find("Load");
            Load.gameObject.SetActive(false);
        }
        public void Load()
        {
            Transform Savebtn = menuScene.transform.Find("Savebtn");
            Savebtn.gameObject.SetActive(false);
            Transform Loadbtn = menuScene.transform.Find("Loadbtn");
            Loadbtn.gameObject.SetActive(true);
            Transform Save = menuScene.transform.Find("Save");
            Save.gameObject.SetActive(false);
            Transform Load = menuScene.transform.Find("Load");
            Load.gameObject.SetActive(false);
        }
        public void LoadGameBtn()
        {
            menuScene.SetActive(true);
            Transform Savebtn = menuScene.transform.Find("Savebtn");
            Savebtn.gameObject.SetActive(false);
            Transform Loadbtn = menuScene.transform.Find("Loadbtn");
            Loadbtn.gameObject.SetActive(true);
            Transform Save = menuScene.transform.Find("Save");
            Save.gameObject.SetActive(false);
            Transform Load = menuScene.transform.Find("Load");
            Load.gameObject.SetActive(false);
        }
        public void SelectScreen(string sceneName)
        {
            StartCoroutine(LoadScene(sceneName));
        }
        public void SelectBattleType(string e)
        {
            StartCoroutine(LoadBattle(e));
        }
        public IEnumerator LoadBattle(string e)
        {
            Cursor.lockState = CursorLockMode.Locked;
            StartCoroutine(sceneFader.UI_Fade());
            yield return new WaitForSeconds(1);

            shopScene.SetActive(false);
            mapScene.SetActive(false);
            chestScene.SetActive(false);
            restScene.SetActive(false);
            nextStageScreen.SetActive(false);
            randomnodeScene.SetActive(false);
            //playerIcon.SetActive(true);

            if (e == "enemy")
                battleSceneManager.StartHallwayFight();
            else if (e == "elite")
                battleSceneManager.StartEliteFight();
            else if (e == "boss")
            {
                nextStageScreen.SetActive(true);
                battleSceneManager.StartBossFight();
            }
                

                

            //fade from black
            yield return new WaitForSeconds(1);
            Cursor.lockState = CursorLockMode.None;
        }
        public IEnumerator LoadScene(string sceneToLoad)
        {
            //Cursor.lockState=CursorLockMode.Locked;
            StartCoroutine(sceneFader.UI_Fade());
            //fade to black
            yield return new WaitForSeconds(1);
            endScreen.gameObject.SetActive(false);
            //playerIcon.SetActive(true);

            if (sceneToLoad == "Map")
            {
                playerIcon.SetActive(false);
                classSelectionScreen.SetActive(false);
                mapScene.SetActive(true);
                chestScene.SetActive(false);
                restScene.SetActive(false);
                shopScene.SetActive(false);
                randomnodeScene.SetActive(false);
                eventScene1.SetActive(false);
                eventScene2.SetActive(false);
                eventScene3.SetActive(false);
                eventScene4.SetActive(false);
                eventScene5.SetActive(false);
                eventScene6.SetActive(false);
                eventScene7.SetActive(false);
                eventScene8.SetActive(false);
                eventScene9.SetActive(false);
                eventScene10.SetActive(false);
            }
            else if (sceneToLoad == "Battle")
            {
                mapScene.SetActive(false);
                chestScene.SetActive(false);
                restScene.SetActive(false);
                shopScene.SetActive(false);
                randomnodeScene.SetActive(false);
            }
            else if (sceneToLoad == "Chest")
            {
                restScene.SetActive(false);
                mapScene.SetActive(false);
                shopScene.SetActive(false);
                chestScene.SetActive(true);
                randomnodeScene.SetActive(false);
            }
            else if (sceneToLoad == "Rest")
            {
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                restScene.SetActive(true);
                shopScene.SetActive(false);
                randomnodeScene.SetActive(false);
            }
            else if (sceneToLoad == "Shop")
            {
                shopScene.SetActive(true);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
                randomnodeScene.SetActive(false);

                Shop shop = shopScene.GetComponent<Shop>();
                shop.ResetRefreshPrice();
                shop.PopulateShop();
                shop.PopulateRelics();
            }
            else if (sceneToLoad == "Random")
            {
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
                randomnodeScene.SetActive(true);
            }
            else if (sceneToLoad == "event1")
            {
                eventScene1.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "event2")
            {
                eventScene2.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "event3")
            {
                eventScene3.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "event4")
            {
                eventScene4.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "event5")
            {
                eventScene5.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "event6")
            {
                eventScene6.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "event7")
            {
                eventScene7.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "event8")
            {
                eventScene8.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "event9")
            {
                eventScene9.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "event10")
            {
                eventScene10.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }

            //fade from black
            yield return new WaitForSeconds(1);
            //Cursor.lockState=CursorLockMode.None;
        }
    }
}
