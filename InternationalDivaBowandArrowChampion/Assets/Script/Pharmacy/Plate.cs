using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Pharmacy pharmacy;
    [SerializeField] private Sprite FinishedPack;
    [SerializeField] private Sprite StartingPack;

    [SerializeField] private SpriteRenderer sr;
    
    public List<GameObject> pivots = new List<GameObject>();

    private int targetAmount;

    private List<Herb> CollectedHerb = new List<Herb>();

    [SerializeField] private float CloseGap = 1f;

    public bool IsFull => CollectedHerb.Count >= targetAmount;

    private bool locked = false;
    
    public void Init(int amount)
    {
        Clear();
        targetAmount = amount;
    }
    
    public Vector3 TakeHerbImmediately(Herb herbType)
    {
        var position = pivots.FirstOrDefault(_ => !_.activeSelf).transform.position;
        CollectedHerb.Add(herbType);
        return position;
    }
    public void Present(GameObject obj, Herb herbType)
    {
        pharmacy.PlaySE(SEManager.SEType.PutOnPlate);
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
        locked = false;
        pivots.ForEach(_ =>
        {
            _.SetActive(false);
        });
        CollectedHerb = new List<Herb>();
        sr.sprite = StartingPack;
    }
    
    private void OnMouseDown()
    {
        if (IsFull && !locked)
        {
            pharmacy.PlaySE(SEManager.SEType.Bell);
            locked = true;
            StartCoroutine(ClosePack());
        }
    }

    IEnumerator ClosePack()
    {
        pharmacy.PlaySE(SEManager.SEType.Packing);
        pharmacy.FetchResult(CollectedHerb);
        pivots.ForEach(_ =>
        {
            _.SetActive(false);
        });
        sr.sprite = FinishedPack;
        yield return new WaitForSeconds(CloseGap);
        locked = false;
        pharmacy.Finish();
        // Tell Pharmacy that collection is over. 
    }
}
