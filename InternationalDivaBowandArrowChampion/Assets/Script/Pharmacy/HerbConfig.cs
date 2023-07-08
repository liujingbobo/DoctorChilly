using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "HerbConfig", menuName = "Configs", order = 1)]
public class HerbConfig : SerializedScriptableObject
{
    public Dictionary<Herb, Sprite> HerbSpritesDic = new Dictionary<Herb, Sprite>();
}
