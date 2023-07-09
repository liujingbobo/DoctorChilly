using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CutScenePlayer : MonoBehaviour
{
    public Animator animator;
    public Image mainImage;
    
    private Action _callBack;
    [Button]
    public void PlayAnim(Action onEndCallBack)
    {
        mainImage.gameObject.SetActive(true);
        animator.Play("CutSceneAnimation", 0,0);
        _callBack = onEndCallBack;
    }

    public void Ding()
    {
        Debug.Log("Ding");
        GameManager.Instance.SeManager.PlaySE(SEManager.SEType.Emm);

    }

    public void OpenDrawer()
    {
        GameManager.Instance.SeManager.PlaySE(SEManager.SEType.OpenDrawer);
    }

    public void Emm()
    {
        GameManager.Instance.SeManager.PlaySE(SEManager.SEType.Emm);

    }
    public void Ended()
    {
        Debug.Log("AnimEnded");
        mainImage.gameObject.SetActive(false);
        _callBack?.Invoke();
    }
    
}
