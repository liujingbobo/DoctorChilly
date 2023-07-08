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

    [SerializeField] private GameObject Front;

    private Coroutine closeDrawer;

    [SerializeField] private Ease _ease;

    [SerializeField] private float moveTime = 0.35f;

    [SerializeField] private float targetY = -0.09f;

    [SerializeField] private float drawerSpeed = 0.1f;

    [SerializeField] private float timeTillCLose = 1f;

    [SerializeField] private Ease openEase;
    

    private Tween anim;
    
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
            var pos= plate.TakeHerbImmediately(herbContained);
            
            herbItem.transform.DOMove(pos, moveTime).SetEase(_ease).OnComplete(() =>
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
        yield return new WaitForSeconds(timeTillCLose);
        ClearAndReset();
    }
    
    public void ClearAndReset()
    {
        SwitchDrawer(false);
    }

    public void SwitchDrawer(bool value)
    {
        anim?.Kill();
        
        if (value)
        {
            anim = Front.transform.DOLocalMoveY(targetY, drawerSpeed).SetEase(openEase);
        }
        else
        {
            anim = Front.transform.DOLocalMoveY(0, drawerSpeed).SetEase(openEase);
        }
    }
}
