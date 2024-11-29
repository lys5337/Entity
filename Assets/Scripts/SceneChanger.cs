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
        public GameObject classSelectionScreen_Normal;
        public GameObject classSelectionScreen_Story;
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
        public GameObject eliteEvent1;
        public GameObject eliteEvent2;
        public GameObject eliteEvent3;
        public GameObject bossEvent;
        public GameObject shopEvent;
        public GameObject dragPath;
        public GameObject gameOverScreen;

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
            dragPath.SetActive(false);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        public void StartScript()
        {
            eventScene6.SetActive(false);
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

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 씬 이름에 따라 적절한 BGM 설정
            string currentSceneName = scene.name;

            if(currentSceneName == "Main")
            {
                BGMManager.Instance.PlaySound("MainBGM");
            }
            else if (currentSceneName == "Stage0")
            {
                BGMManager.Instance.PlaySound("0_0MapBGM");
            }
            else if (currentSceneName == "Stage1.0")
            {
                BGMManager.Instance.PlaySound("1_0MapBGM");
            }
            else if (currentSceneName == "Stage1.1")
            {
                BGMManager.Instance.PlaySound("1_1MapBGM");
            }
            else if (currentSceneName == "Stage2.1")
            {
                BGMManager.Instance.PlaySound("2_1MapBGM");
            }
            else if (currentSceneName == "Stage2.2")
            {
                BGMManager.Instance.PlaySound("2_2MapBGM");
            }
            else if (currentSceneName == "Stage3.0")
            {
                BGMManager.Instance.PlaySound("3_0MapBGM");
            }
            else if (currentSceneName == "Stage3.1")
            {
                BGMManager.Instance.PlaySound("3_1MapBGM");
            }
            else if (currentSceneName == "Stage3.2")
            {
                BGMManager.Instance.PlaySound("3_2MapBGM");
            }
            else
            {
                Debug.Log($"씬 '{currentSceneName}'에 대한 BGM이 정의되지 않았습니다.");
            }
        }

        public void CheckGameOver()
        {
            if (gameManager.playerCurrentHealth <= 0 || gameManager.playerMaxHealth <= 0)
            {
                gameOverScreen.SetActive(true);
            }
        }

        public void FloorNumberUpdate()
        {
            gameManager.UpdateFloorNumber();
        }
        public void Normal()
        {
            ModeSelectScreen.SetActive(false);
            eventScene6.SetActive(false);
            classSelectionScreen_Story.SetActive(false);
            gameManager.UpdatePlayerStatsUI();
        }
        public void Story()
        {
            SceneManager.LoadScene("Stage0");
            gameManager.UpdatePlayerStatsUI();
            gameManager.NowStageRelicSet();
        }
        public void ModeClose()
        {
            ModeSelectScreen.SetActive(false);
        }
        public void NextStage()
        {

            // 현재 씬 이름에 따라 다음 씬으로 이동
            if (SceneManager.GetActiveScene().name == "Stage0")
            {
                SceneManager.LoadScene("Stage1.0");
                gameManager.stageNumber = 1;
                SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
                if (saveDataManager != null)
                {
                    saveDataManager.SaveGame(4);
                    Debug.Log("4번 슬롯에 데이터 저장 완료.");
                }
                else
                {
                    Debug.LogWarning("SaveDataManager를 찾을 수 없습니다.");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Stage1.0")
            {
                SceneManager.LoadScene("Stage1.1");
                gameManager.stageNumber = 1;
                SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
                if (saveDataManager != null)
                {
                    saveDataManager.SaveGame(4);
                    Debug.Log("4번 슬롯에 데이터 저장 완료.");
                }
                else
                {
                    Debug.LogWarning("SaveDataManager를 찾을 수 없습니다.");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Stage1.1")
            {
                SceneManager.LoadScene("Stage2.1");
                gameManager.stageNumber = 2;
                SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
                if (saveDataManager != null)
                {
                    saveDataManager.SaveGame(4);
                    Debug.Log("4번 슬롯에 데이터 저장 완료.");
                }
                else
                {
                    Debug.LogWarning("SaveDataManager를 찾을 수 없습니다.");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Stage2.1")
            {
                SceneManager.LoadScene("Stage3.0");
                gameManager.stageNumber = 4;
                SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
                if (saveDataManager != null)
                {
                    saveDataManager.SaveGame(4);
                    Debug.Log("4번 슬롯에 데이터 저장 완료.");
                }
                else
                {
                    Debug.LogWarning("SaveDataManager를 찾을 수 없습니다.");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Stage3.0")
            {
                SceneManager.LoadScene("Stage3.1");
                gameManager.stageNumber = 4;
                SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
                if (saveDataManager != null)
                {
                    saveDataManager.SaveGame(4);
                    Debug.Log("4번 슬롯에 데이터 저장 완료.");
                }
                else
                {
                    Debug.LogWarning("SaveDataManager를 찾을 수 없습니다.");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Stage3.1")
            {
                SceneManager.LoadScene("Stage3.2");
                gameManager.stageNumber = 4;
                SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
                if (saveDataManager != null)
                {
                    saveDataManager.SaveGame(4);
                    Debug.Log("4번 슬롯에 데이터 저장 완료.");
                }
                else
                {
                    Debug.LogWarning("SaveDataManager를 찾을 수 없습니다.");
                }
            }

            // 씬 이동 후 데이터 로드
            SceneManager.sceneLoaded += OnSceneLoadedForNextStage;
        }

        public void NextStageSide()
        {
            // 세이브 데이터 4번 슬롯에 저장
            

            // 현재 씬 이름에 따라 다음 사이드 스테이지로 이동
            if (SceneManager.GetActiveScene().name == "Stage1.1")
            {
                SceneManager.LoadScene("Stage2.2");
                gameManager.stageNumber = 3;
                SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
                if (saveDataManager != null)
                {
                    saveDataManager.SaveGame(4);
                    Debug.Log("4번 슬롯에 데이터 저장 완료.");
                }
                else
                {
                    Debug.LogWarning("SaveDataManager를 찾을 수 없습니다.");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Stage2.2")
            {
                SceneManager.LoadScene("Stage3.0");
                gameManager.stageNumber = 4;
                SaveDataManager saveDataManager = FindObjectOfType<SaveDataManager>();
                if (saveDataManager != null)
                {
                    saveDataManager.SaveGame(4);
                    Debug.Log("4번 슬롯에 데이터 저장 완료.");
                }
                else
                {
                    Debug.LogWarning("SaveDataManager를 찾을 수 없습니다.");
                }
            }

            // 씬 이동 후 데이터 로드
            SceneManager.sceneLoaded += OnSceneLoadedForNextStage;
        }

        private void OnSceneLoadedForNextStage(Scene scene, LoadSceneMode mode)
        {
            // 데이터만 로드
            LoadDataManager loadDataManager = FindObjectOfType<LoadDataManager>();
            if (loadDataManager != null)
            {
                loadDataManager.RestoreGameData(4); // 4번 슬롯에서 데이터 복원
                Debug.Log("4번 슬롯에서 데이터 복원 완료.");
            }
            else
            {
                Debug.LogWarning("LoadDataManager를 찾을 수 없습니다.");
            }

            // 씬 로드 이벤트 해제
            SceneManager.sceneLoaded -= OnSceneLoadedForNextStage;
        }

        public void Menuopen()
        {
            menuScene.SetActive(true);
        }

        public void Menuclose()
        {
            menuScene.SetActive(false);
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
        public void Eventend()
        {
            if (eventScene1.activeSelf ||
                eventScene2.activeSelf ||
                eventScene3.activeSelf ||
                eventScene4.activeSelf ||
                eventScene5.activeSelf ||
                eventScene6.activeSelf ||
                eliteEvent1.activeSelf ||
                eliteEvent2.activeSelf ||
                eliteEvent3.activeSelf ||
                bossEvent.activeSelf ||
                shopEvent.activeSelf)
            {
                eventScene1.SetActive(false);
                eventScene2.SetActive(false);
                eventScene3.SetActive(false);
                eventScene4.SetActive(false);
                eventScene5.SetActive(false);
                eventScene6.SetActive(false);
                eliteEvent1.SetActive(false);
                eliteEvent2.SetActive(false);
                eliteEvent3.SetActive(false);
                bossEvent.SetActive(false);
                shopEvent.SetActive(false);
            }
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
            {
                battleSceneManager.StartNormalFight();
                dragPath.SetActive(true);
            }
                
            else if (e == "elite")
            {
                battleSceneManager.StartEliteFight();
                dragPath.SetActive(true);
            }
                
            else if (e == "boss")
            {
                battleSceneManager.StartBossFight();
                dragPath.SetActive(true);
            }
                
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
                classSelectionScreen_Normal.SetActive(false);
                classSelectionScreen_Story.SetActive(false);
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
                eliteEvent1.SetActive(false);
                eliteEvent2.SetActive(false);
                eliteEvent3.SetActive(false);
                bossEvent.SetActive(false);
                dragPath.SetActive(false);
                gameManager.floorNumber -= 1;
                gameManager.UpdateFloorNumber();

                
                if (BGMManager.Instance.GetCurrentBGM() != "MapBGM")
                {
                    BGMManager.Instance.PlaySound("MapBGM");
                }

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
                eventScene1.SetActive(false);
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
            else if (sceneToLoad == "eliteevent1")
            {
                eliteEvent1.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "eliteevent2")
            {
                eliteEvent2.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "eliteevent3")
            {
                eliteEvent3.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "bossevent")
            {
                bossEvent.SetActive(true);
                shopScene.SetActive(false);
                restScene.SetActive(false);
                chestScene.SetActive(false);
                mapScene.SetActive(false);
                playerIcon.SetActive(false);
            }
            else if (sceneToLoad == "NextStage")
                nextStageScreen.SetActive(true);
            else if (sceneToLoad == "ShopEvent")
                shopEvent.SetActive(true);

            //fade from black
            yield return new WaitForSeconds(1);
            //Cursor.lockState=CursorLockMode.None;
        }
    }
}
