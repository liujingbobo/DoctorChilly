using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drawer : MonoBehaviour
{
    [SerializeField] private Plate plate;
    
    [SerializeField] private Pharmacy pharmacy;
    
    [SerializeField] private Herb herbContained;
    
    [SerializeField] private GameObject herbPrefab;

    [SerializeField] private List<GameObject> OpenedDrawerSprites;
    [SerializeField] private List<GameObject> ClosedDrawerSprites;

    private Coroutine closeDrawer; 
    
    private void OnMouseDown()
    {
        if (!plate.IsFull)
        {
            var herbItem = Instantiate(herbPrefab, plate.transform);
            herbItem.transform.position = transform.position;
            if (herbItem.GetComponent<SpriteRenderer>() is { } sr)
            {
                sr.sprite = pharmacy.HerbSpritesDic[herbContained];
            }
            herbItem.SetActive(true);
            // Add herb immediately, moving to plate is just animation
            plate.TakeHerbImmediately(herbContained);
            
            herbItem.transform.DOMove(plate.transform.position, 0.35f).OnComplete(() =>
            {
                plate.Present(herbItem, herbContained);
            });
            
            SwitchDrawer(true);

            if(closeDrawer != null) StopCoroutine(closeDrawer); 
            
            closeDrawer= StartCoroutine(CloseDrawer());
            
        }
        else
        {
            Debug.Log("盘子已满");
        }
    }

    IEnumerator CloseDrawer()
    {
        yield return new WaitForSeconds(1);
        ClearAndReset();
    }
    
    public void ClearAndReset()
    {
        SwitchDrawer(false);
    }

    public void SwitchDrawer(bool value)
    {
        ClosedDrawerSprites.ForEach(_ => _.gameObject.SetActive(!value));
        OpenedDrawerSprites.ForEach(_ => _.gameObject.SetActive(value));
    }
}
