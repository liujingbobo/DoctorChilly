using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Util;

public class Pharmacy : Singleton<Pharmacy>
{
    [SerializeField] private HerbConfig config;
    
    public Dictionary<Herb, Sprite> HerbSpritesDic =>config.HerbSpritesDic;
    
    public Plate plate;

    public List<Drawer> Drawers = new List<Drawer>();

    private List<Herb> _requiredHerb;

    private bool _finished;

    private bool _result;

    public async UniTask<bool> Init(List<Herb> requiredHerb)
    {
        Clear();
        
        _finished = false;
        
        _requiredHerb = requiredHerb;
        
        plate.Init(requiredHerb.Count);

        await UniTask.WaitUntil(() => _finished);

        return _result;
    }

    public void Finish(List<Herb> collectedHerbs)
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

        _finished = true;
    }

    public void Clear()
    {
        plate.Clear();
        Drawers.ForEach(_ => _.ClearAndReset());
    }
}
