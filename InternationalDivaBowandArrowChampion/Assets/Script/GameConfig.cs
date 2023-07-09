using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs", order = 1)]
public class GameConfig : SerializedScriptableObject
{
    public Dictionary<Herb, Sprite> HerbSpritesDic = new Dictionary<Herb, Sprite>();

    public Dictionary<Symptom, SymptomPack> SymptomPacks = new Dictionary<Symptom, SymptomPack>();

    public HandPack normalHandPack;
    public List<HandPack> HandConfigs = new List<HandPack>();

    public Dictionary<PharmacyResult, Sprite> ResultDic = new Dictionary<PharmacyResult, Sprite>();
    

    public struct SymptomPack
    {
        public List<Herb> Herbs;
        public DiagnosisPattern diagnosisPattern;
    }

    public class HandPack
    {
        public Sprite Hand;

        public string StartDialog;

        public string EndDialog;

        public string WinDialog;

        public string LoseDialog;

        // TODO: BGM
    }

    // public List<T> RandomlyGetTwo<T>(List<T> items, int count)
    // {
    //     // 生成两个不同的随机索引
    //     int randomIndex1 = UnityEngine.Random.Range(0, items.Count);
    //     int randomIndex2 = UnityEngine.Random.Range(0, items.Count - 1);
    //     if (randomIndex2 >= randomIndex1)
    //         randomIndex2++;
    //
    //     // 获取对应的元素
    //     T item1 = items[randomIndex1];
    //     T item2 = items[randomIndex2];
    //     return new List<T>() { item1, item2 };
    //     
    // }

    private List<T> GetRandomItems<T>(List<T> list, int count)
    {
        List<T> result = new List<T>(list);

        for (int i = 0; i < result.Count - 1; i++)
        {
            int randomIndex = Random.Range(i, result.Count);
            (result[randomIndex], result[i]) = (result[i], result[randomIndex]);
        }

        return result.GetRange(0, count);
    }

    public List<Symptom> RandomlyGetSymtoms(int count)
    {
        return GetRandomItems<Symptom>(SymptomPacks.Keys.ToList(), count);
    }


    public List<Herb> GetHerbs(List<Symptom> symptoms)
    {
        List<Herb> herbs = new List<Herb>();
        foreach (var s in symptoms)
        {
            herbs.AddRange(SymptomPacks[s].Herbs);
        }
        return herbs;
    }

    public HandPack RandomPickHandExcludeGiven(List<HandPack> handPack)
    {
        var excludedList = handPack == null? 
            HandConfigs:
            HandConfigs.Where(x => !handPack.Contains(x)).ToList();
        return GetRandomItems<HandPack>(excludedList, 1)[0];

    }
}