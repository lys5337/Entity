using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ
{
[CreateAssetMenu]
public class Relic : ScriptableObject
{
	public string relicName;
    public string relicDescription;
    public Sprite relicIcon;
}
}
//배틀씬 메니져에 소속된 스크립트