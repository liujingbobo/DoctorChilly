using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Util;

public class Pharmacy : MonoBehaviour
{
    public Dictionary<Herb, Sprite> HerbSpritesDic => GameManager.Instance.Config.HerbSpritesDic;

    public Plate plate;

    public List<Drawer> Drawers = new List<Drawer>();

    private List<Herb> _requiredHerb;

    public bool paused;

    private bool _result;

    public void Init(List<Herb> requiredHerb)
    {
        Clear();

        _requiredHerb = requiredHerb;

        Debug.Log($"Pharmacy Init:");
        foreach (var h in requiredHerb)
        {
            Debug.Log($"    requiredHerb:{h}");
        }
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

        Debug.Log($"Pharmacy FetchResult:");
        foreach (var h in collectedHerbs)
        {
            Debug.Log($"    collectedHerbs:{h}");
        }

        foreach (var h in _requiredHerb)
        {
            Debug.Log($"    _requiredHerb:{h}");
        }

        //collectedHerbs 基本上是以病症的顺序放的，直接分开检查是否达标
        var collectedHerbsModifiable = new List<Herb>(collectedHerbs);

        var symptom1 = _requiredHerb.GetRange(0, 2);
        var symptom2 = new List<Herb>();
        if (_requiredHerb.Count >= 4) symptom2 = _requiredHerb.GetRange(2, 2);

        var symptom1Cured = true;
        foreach (var herb in symptom1)
        {
            if (!collectedHerbsModifiable.Contains(herb))
            {
                symptom1Cured = false;
                break;
            }

            collectedHerbsModifiable.Remove(herb);
        }

        var symptom2Cured = true;
        if (_requiredHerb.Count >= 4)
        {
            foreach (var herb in symptom2)
            {
                if (!collectedHerbsModifiable.Contains(herb))
                {
                    symptom2Cured = false;
                    break;
                }

                collectedHerbsModifiable.Remove(herb);
            }
        }

        _result = symptom1Cured && symptom2Cured;

        /*foreach (var herb in collectedHerbs)
        {
            if (!_requiredHerb.Contains(herb))
            {
                _result = false;
                break;
            }
            _requiredHerb.Remove(herb);
        }*/

        Debug.Log($"Pharmacy FetchResult, symptom1Cured:{symptom1}, symptom2Cured:{symptom2Cured}, result:{_result}");

        var emoji = symptom1Cured && symptom2Cured ? EmojiType.Happy :
            !symptom1Cured && !symptom2Cured ? EmojiType.Ill :
            EmojiType.Sad;

        BubbleManager.singleton.ShowEmojiBubble(emoji, _result, 0.8f);
        BubbleManager.singleton.ShowEndSpeechBubble(_result
            ? GameManager.Instance.CurrentHandPack.WinDialog
            : GameManager.Instance.CurrentHandPack.LoseDialog);
    }

    public void ForceQuit()
    {
        // TODO: 
    }

    public void PlaySE(SEManager.SEType t)
    {
        GameManager.Instance.SeManager.PlaySE(t);
    }

}
