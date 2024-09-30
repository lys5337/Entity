using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ
{
[CreateAssetMenu]
public class Character : ScriptableObject
{
	public CharacterClass characterClass;
    public enum CharacterClass{Swordman, Archer};
    public GameObject characterPrefab;
    public int CharacterStartHealth;
    public Relic startingRelic;
    public Sprite splashArt;
    public List<Card> startingDeck;
}
}