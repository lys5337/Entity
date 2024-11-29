using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TJ
{
    public class Floor : MonoBehaviour
    {
	    public List<Image> nodes;
	    public List<Image> activeNodes;
        public Encounter encounter;
        SceneChanger SceneChanger;
        BattleSceneManager battleSceneManager;
        Map map;
        VerticalLayoutGroup verticalLayoutGroup;
        private void Start()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
        }
        public void StartEncounter()
        {
            if(encounter.encounterType == Encounter.Type.elite1)
            {
                SceneChanger.SelectBattleType("elite");
                SceneChanger.SelectScreen("eliteevent1");
            }
                
            else if (encounter.encounterType == Encounter.Type.elite2)
            {
                SceneChanger.SelectBattleType("elite");
                SceneChanger.SelectScreen("eliteevent2");
            }
                
            else if (encounter.encounterType == Encounter.Type.elite3)
            {
                SceneChanger.SelectBattleType("elite");
                SceneChanger.SelectScreen("eliteevent3");  
            }

            else if (encounter.encounterType == Encounter.Type.chest1)
                SceneChanger.SelectScreen("Chest");
            else if (encounter.encounterType == Encounter.Type.chest2)
                SceneChanger.SelectScreen("Chest");
            else if (encounter.encounterType == Encounter.Type.chest3)
                SceneChanger.SelectScreen("Chest");

            else if (encounter.encounterType == Encounter.Type.rest1)
                SceneChanger.SelectScreen("Rest");
            else if (encounter.encounterType == Encounter.Type.rest2)
                SceneChanger.SelectScreen("Rest");
            else if (encounter.encounterType == Encounter.Type.rest3)
                SceneChanger.SelectScreen("Rest");

            else if (encounter.encounterType == Encounter.Type.shop1)
            {
                SceneChanger.SelectScreen("Shop");
                SceneChanger.SelectScreen("ShopEvent");
            }
            else if (encounter.encounterType == Encounter.Type.shop2)
                SceneChanger.SelectScreen("Shop");
            else if (encounter.encounterType == Encounter.Type.shop3)
                SceneChanger.SelectScreen("Shop");

            else if (encounter.encounterType == Encounter.Type.boss)
            {
                SceneChanger.SelectBattleType("boss");
                SceneChanger.SelectScreen("bossevent");
            }
            
            else if (encounter.encounterType == Encounter.Type.enemy)
                SceneChanger.SelectBattleType("enemy");

            else if (encounter.encounterType == Encounter.Type.random)
                SceneChanger.SelectScreen("Random");

            else if (encounter.encounterType == Encounter.Type.event1)
                SceneChanger.SelectScreen("event1");
            else if (encounter.encounterType == Encounter.Type.event2)
                SceneChanger.SelectScreen("event2");
            else if (encounter.encounterType == Encounter.Type.event3)
                SceneChanger.SelectScreen("event3");
            else if (encounter.encounterType == Encounter.Type.event4)
                SceneChanger.SelectScreen("event4");
            else if (encounter.encounterType == Encounter.Type.event5)
                SceneChanger.SelectScreen("event5");
            else if (encounter.encounterType == Encounter.Type.event6)
                SceneChanger.SelectScreen("event6");

            else if (encounter.encounterType == Encounter.Type.eventEnemy1)
            {
                SceneChanger.SelectBattleType("enemy");
                SceneChanger.SelectScreen("EventEnemy1");
            }
            else if (encounter.encounterType == Encounter.Type.eventEnemy2)
            {
                SceneChanger.SelectBattleType("enemy");
                SceneChanger.SelectScreen("EventEnemy2");
            }
            else if (encounter.encounterType == Encounter.Type.eventEnemy3)
            {
                SceneChanger.SelectBattleType("enemy");
                SceneChanger.SelectScreen("EventEnemy3");
            }
            else if (encounter.encounterType == Encounter.Type.eventEnemy4)
            {
                SceneChanger.SelectBattleType("enemy");
                SceneChanger.SelectScreen("EventEnemy4");
            }
            else if (encounter.encounterType == Encounter.Type.eventEnemy5)
            {
                SceneChanger.SelectBattleType("enemy");
                SceneChanger.SelectScreen("EventEnemy5");
            }

        }
        public void SetNodesActive(Encounter _encounter)
        {
                verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            SceneChanger = FindObjectOfType<SceneChanger>();
            map = FindObjectOfType<Map>();

            foreach(Transform n in this.gameObject.transform)
                nodes.Add(n.GetComponent<Image>());
            
            encounter = _encounter;
            foreach(Image n in nodes)
            {
                n.enabled=false;
                n.GetComponent<Node>().availableIcon.enabled=false;
                n.GetComponent<Node>().clickedIcon.enabled=false;
                n.GetComponent<Node>().floor=this;
            }
            if (SceneManager.GetActiveScene().name == "Main")
            {
                verticalLayoutGroup.spacing = Random.Range(25f, 125f);
                verticalLayoutGroup.padding.top = Random.Range(0, 125);
            }
                
            //this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (600, Random.Range(80,120));
            activeNodes.Clear();
            nodes.Shuffle();
            EnableNode(nodes[0]);
        
            for (int i = 1; i < nodes.Count; i++)
            {
                if (encounter.encounterType == Encounter.Type.elite1)
                    continue;
                else if (encounter.encounterType == Encounter.Type.elite2)
                    continue;
                else if (encounter.encounterType == Encounter.Type.elite3)
                    continue;

                else if (encounter.encounterType == Encounter.Type.rest1)
                    continue;
                else if (encounter.encounterType == Encounter.Type.rest2)
                    continue;
                else if (encounter.encounterType == Encounter.Type.rest3)
                    continue;

                else if (encounter.encounterType == Encounter.Type.shop1)
                    continue;
                else if (encounter.encounterType == Encounter.Type.shop2)
                    continue;
                else if (encounter.encounterType == Encounter.Type.shop3)
                    continue;

                else if (encounter.encounterType == Encounter.Type.boss)
                    continue;

                else if (encounter.encounterType == Encounter.Type.event1)
                    continue;
                else if (encounter.encounterType == Encounter.Type.event2)
                    continue;
                else if (encounter.encounterType == Encounter.Type.event3)
                    continue;
                else if (encounter.encounterType == Encounter.Type.event4)
                    continue;
                else if (encounter.encounterType == Encounter.Type.event5)
                    continue;
                else if (encounter.encounterType == Encounter.Type.event6)
                    continue;

                else if (encounter.encounterType == Encounter.Type.enemy)
                    continue;
                else if (encounter.encounterType == Encounter.Type.random)
                    continue;

                else if (encounter.encounterType == Encounter.Type.eventEnemy1)
                    continue;
                else if (encounter.encounterType == Encounter.Type.eventEnemy2)
                    continue;
                else if (encounter.encounterType == Encounter.Type.eventEnemy3)
                    continue;
                else if (encounter.encounterType == Encounter.Type.eventEnemy4)
                    continue;
                else if (encounter.encounterType == Encounter.Type.eventEnemy5)
                    continue;

                else if (SceneManager.GetActiveScene().name == "Main")
                {
                    //Debug.Log(nodes.Count);
                    //Debug.Log($"Is {Random.Range(0,1f)} >= {i*(1f/(nodes.Count))}?");
                    if (Random.Range(0, 1f) >= i * (1f / (nodes.Count)))
                        EnableNode(nodes[i]);

                }

                
            }
        }
        public void SetNodesActiveClickable()
        {
            foreach(Image n in activeNodes)
                n.GetComponent<Node>().availableIcon.enabled=true;
        }
        public void EnableNode(Image n)
        {
            n.enabled = true;
            n.sprite = encounter.encounterSprite;
            activeNodes.Add(n);
        }
        public void ClickedOnMe(Node clickedNode)
        {
            if(!clickedNode.availableIcon.enabled)
                return;

            clickedNode.clickedIcon.enabled=true;

            foreach(Image n in activeNodes)
                n.GetComponent<Node>().availableIcon.enabled=false;

            StartEncounter();
            //StartCoroutine(map.Delayedclicky());
        
            // if(encounter.encounterType==Encounter.Type.enemy)
            //     sceneManager.SelectBattleType("enemy");
            // else if(encounter.encounterType==Encounter.Type.elite)
            //     sceneManager.SelectBattleType("elite");
        }
    
    
    }

}
