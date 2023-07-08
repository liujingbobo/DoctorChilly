using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Tutorial : MonoBehaviour
{
    public Image[] Images;
    public Image BGImages;
    public Button button;
    public GameManager manager;

    public int index;
    private void Start()
    {
        index = 0;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(NextPage);
        
        BGImages.gameObject.SetActive(true);
        button.gameObject.SetActive(false);
        foreach (var Image in Images)
        {
            Image.gameObject.SetActive(false);
            Image.color = new Color(1,1,1,0);
        }
    }

    [Button]
    public void StartTutorial()
    {
        index = 0;
        NextPage();
        button.gameObject.SetActive(true);
    }

    public void NextPage()
    {
        if (index < 2)
        {
            ShowPage(index);
            index++;
        }
        else
        {
            //TODO call manager to state 1 here
            BGImages.gameObject.SetActive(false);
            manager.ChangeState(GameState.State1);
        }
    }

    private void ShowPage(int index)
    {
        button.interactable = false;
        foreach (var Image in Images)
        {
            Image.gameObject.SetActive(false);
        }
        Images[index].gameObject.SetActive(true);
        var tween = Images[index].DOColor(Color.white, 0.5f);
        tween.OnComplete(() =>
        {
            button.interactable = true;
        });
    }
}
