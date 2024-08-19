using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
public class Floor : MonoBehaviour
{
	public List<Image> nodes;
	public List<Image> activeNodes;
    public Encounter encounter;
    SceneChanger SceneChanger;
    Map map;
    VerticalLayoutGroup verticalLayoutGroup;
    private void Awake()
    {
        
    }
    public void StartEncounter()
    {
        if(encounter.encounterType==Encounter.Type.enemy)
            SceneChanger.SelectBattleType("enemy");
        else if(encounter.encounterType==Encounter.Type.elite)
            SceneChanger.SelectBattleType("elite");
        else if(encounter.encounterType==Encounter.Type.boss)
            SceneChanger.SelectBattleType("boss");
        else if(encounter.encounterType==Encounter.Type.rest)
            SceneChanger.SelectScreen("Rest");
        else if(encounter.encounterType==Encounter.Type.chest)
            SceneChanger.SelectScreen("Chest");
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

        verticalLayoutGroup.spacing= Random.Range(25f,125f);
        verticalLayoutGroup.padding.top= Random.Range(0,125);
        //this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (600, Random.Range(80,120));
        activeNodes.Clear();
        nodes.Shuffle();
        EnableNode(nodes[0]);
        
        for (int i = 1; i < nodes.Count; i++)
        {
            //Debug.Log(nodes.Count);
            //Debug.Log($"Is {Random.Range(0,1f)} >= {i*(1f/(nodes.Count))}?");
            if(Random.Range(0,1f)>=i*(1f/(nodes.Count)))
                EnableNode(nodes[i]);
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
