using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class Map : MonoBehaviour
    {
        public List<Floor> floors;
        //public List<EncounterHeiarchy> allEncounters;
        public GameObject encounterPrefab;
        public Image enemyIcon, EliteIcon, bossIcon;
        public int elite1Floors;
        public int elite2Floors;
        public int elite3Floors;

        public int chest1Floors;
        public int chest2Floors;
        public int chest3Floors;

        public int rest1Floors;
        public int rest2Floors;
        public int rest3Floors;

        public int shop1Floors;
        public int shop2Floors;
        public int shop3Floors;

        public int bossFloors;
        

        public Encounter enemyEncounter;

        public Encounter elite1Encounter;
        public Encounter elite2Encounter;
        public Encounter elite3Encounter;

        public Encounter chest1Encounter;
        public Encounter chest2Encounter;
        public Encounter chest3Encounter;

        public Encounter rest1Encounter;
        public Encounter rest2Encounter;
        public Encounter rest3Encounter;

        public Encounter shop1Encounter;
        public Encounter shop2Encounter;
        public Encounter shop3Encounter;

        public Encounter bossEncounter;
        
        public int currentFloorNumber;

        private void Awake()
        {
            for (int i = 0; i < floors.Count; i++)
            {
                if (i == elite1Floors)
                    floors[i].SetNodesActive(elite1Encounter);
                else if (i == elite2Floors)
                    floors[i].SetNodesActive(elite2Encounter);
                else if (i == elite3Floors)
                    floors[i].SetNodesActive(elite3Encounter);

                else if (i == chest1Floors)
                    floors[i].SetNodesActive(chest1Encounter);
                else if (i == chest2Floors)
                    floors[i].SetNodesActive(chest2Encounter);
                else if (i == chest3Floors)
                    floors[i].SetNodesActive(chest3Encounter);

                else if (i == rest1Floors)
                    floors[i].SetNodesActive(rest1Encounter);
                else if (i == rest2Floors)
                    floors[i].SetNodesActive(rest2Encounter);
                else if (i == rest3Floors)
                    floors[i].SetNodesActive(rest3Encounter);

                else if (i == shop1Floors)
                    floors[i].SetNodesActive(shop1Encounter);
                else if (i == shop2Floors)
                    floors[i].SetNodesActive(shop2Encounter);
                else if (i == shop3Floors)
                    floors[i].SetNodesActive(shop3Encounter);

                else if (i == bossFloors)
                    floors[i].SetNodesActive(bossEncounter);
                
                else
                    floors[i].SetNodesActive(enemyEncounter);
            }
        }

        public void ShowOptions()
        {
            Debug.Log("showing options");
            if (currentFloorNumber == floors.Count)
            {
                currentFloorNumber = 0;
                GenerateMap();
                return;
            }

            for (int i = 0; i < floors.Count; i++)
            {
                if (i == currentFloorNumber)
                    floors[i].SetNodesActiveClickable();
            }
            currentFloorNumber++;
        }

        public void GenerateMap()
        {
            for (int i = 0; i < floors.Count; i++)
            {
                if (i == elite1Floors)
                    floors[i].SetNodesActive(elite1Encounter);
                else if (i == elite2Floors)
                    floors[i].SetNodesActive(elite2Encounter);
                else if (i == elite3Floors)
                    floors[i].SetNodesActive(elite3Encounter);

                else if (i == chest1Floors)
                    floors[i].SetNodesActive(chest1Encounter);
                else if (i == chest2Floors)
                    floors[i].SetNodesActive(chest2Encounter);
                else if (i == chest3Floors)
                    floors[i].SetNodesActive(chest3Encounter);

                else if (i == rest1Floors)
                    floors[i].SetNodesActive(rest1Encounter);
                else if (i == rest2Floors)
                    floors[i].SetNodesActive(rest2Encounter);
                else if (i == rest3Floors)
                    floors[i].SetNodesActive(rest3Encounter);

                else if (i == shop1Floors)
                    floors[i].SetNodesActive(shop1Encounter);
                else if (i == shop2Floors)
                    floors[i].SetNodesActive(shop2Encounter);
                else if (i == shop3Floors)
                    floors[i].SetNodesActive(shop3Encounter);

                else if (i == bossFloors)
                    floors[i].SetNodesActive(bossEncounter);

                else
                    floors[i].SetNodesActive(enemyEncounter);
            }
            ShowOptions();
        }

        public void ConnectFloors(Floor parentNodes, Floor childNodes)
        {
            //Debug.Log("we need to connect them");
            for (int i = 0; i < parentNodes.activeNodes.Count; i++)
            {
                if (childNodes.activeNodes.Contains(childNodes.nodes[0]))
                {

                }
            }
        }

        // public IEnumerator Delayedclicky()
        // {
        //     yield return new WaitForSeconds(1);
        //     ShowOptions();
        // }
        private void OnEnable()
        {
            ShowOptions();
        }
    }

    // public class EncounterHeiarchy:MonoBehaviour
    // {
    //     public List<Encounter> encounterParents;
    //     public Encounter encounterSelf;
    //     public List<Encounter> encounterChildren;
    // }
    [System.Serializable]
    public struct Encounter
    {
        public Type encounterType;
        public enum Type { enemy, 
            elite1, elite2, elite3, 
            chest1, chest2, chest3, 
            rest1, rest2, rest3,
            shop1, shop2, shop3,
            boss};
        public Sprite encounterSprite;
    }
}
