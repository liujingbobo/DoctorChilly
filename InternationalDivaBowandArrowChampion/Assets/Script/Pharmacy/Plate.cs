using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Pharmacy pharmacy;
    
    public List<GameObject> pivots = new List<GameObject>();

    private int targetAmount;

    private List<Herb> CollectedHerb = new List<Herb>();

    public bool IsFull => CollectedHerb.Count >= targetAmount;
    
    public void Init(int amount)
    {
        Clear();
        targetAmount = amount;
    }
    public void TakeHerbImmediately(Herb herbType)
    {
        CollectedHerb.Add(herbType);
    }
    public void Present(GameObject obj, Herb herbType)
    {
        Destroy(obj);
        var pivot = pivots.FirstOrDefault(_ => !_.activeSelf);
        if (pivot != null)
        {
            pivot.SetActive(true);
            if (pivot.gameObject.GetComponentInChildren<SpriteRenderer>() is { } sr)
            {
                sr.sprite = pharmacy.HerbSpritesDic[herbType];
            }
        }
    }
    public void Clear()
    {
        pivots.ForEach(_ =>
        {
            _.SetActive(false);
        });
        CollectedHerb = new List<Herb>();
    }
    private void OnMouseDown()
    {
        if (IsFull)
        {
            // Tell Pharmacy that collection is over. 
            pharmacy.Finish(CollectedHerb);
        }
    }
}
