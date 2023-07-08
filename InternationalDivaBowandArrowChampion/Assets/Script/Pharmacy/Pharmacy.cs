using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Util;

public class Pharmacy : MonoBehaviour
{
    public Dictionary<Herb, Sprite> HerbSpritesDic =>GameManager.Instance.Config.HerbSpritesDic;
    
    public Plate plate;

    public List<Drawer> Drawers = new List<Drawer>();

    private List<Herb> _requiredHerb;

    public bool paused;

    private bool _result;

    public void Init(List<Herb> requiredHerb)
    {
        Clear();
        
        _requiredHerb = requiredHerb;
        
        plate.Init(requiredHerb.Count);
    }

    public void Finish()
    {
        GameManager.Instance.EndPharmacy(_result);
    }

    public void Clear()
    {
        plate.Clear();
        Drawers.ForEach(_ => _.ClearAndReset());
    }

    public void FetchResult(List<Herb> collectedHerbs)
    {
        _result = true;
        
        foreach (var herb in collectedHerbs)
        {
            if (!_requiredHerb.Contains(herb))
            {
                _result = false;
                break;
            }
            _requiredHerb.Remove(herb);
        }
        
        BubbleManager.singleton.ShowEmojiBubble(_result ? EmojiType.Happy : EmojiType.Sad, _result);
    }
    
    public void ForceQuit()
    {
        // TODO: 
    }
}
