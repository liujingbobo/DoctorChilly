using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BubbleManager : MonoBehaviour
{
    public static BubbleManager singleton;

    public GameObject speechBubble;
    public TextTypeWriterEffect bubbleWriterEffect;

    public GameObject emojiBubble;
    public Image[] emojiImage;

    public float bubbleSpeed = 0.5f;
    public float wordGap = 0.05f;
    
    public void Awake()
    {
        singleton = this;
        speechBubble.transform.localScale = Vector3.zero;
        emojiBubble.transform.localScale = Vector3.zero;
    }

    private Tween _speechBubbleTween;
    private Tween _emojiBubbleTween;
    public void ShowSpeechBubble(string targetString)
    {
        speechBubble.transform.localScale = Vector3.zero;
        if(_speechBubbleTween != null) _speechBubbleTween.Kill();
        _speechBubbleTween =speechBubble.transform.DOScale(1, bubbleSpeed);
        bubbleWriterEffect.StartTypeWriteEffectWithInterval(targetString, wordGap);
        DOVirtual.DelayedCall(5f, () =>
        {
            CloseSpeechBubble();
        });
    }

    public void CloseSpeechBubble()
    {
        if(_speechBubbleTween != null) _speechBubbleTween.Kill();
        _speechBubbleTween = speechBubble.transform.DOScale(0, 0.2f);
    }

    public void ShowEmojiBubble(EmojiType emojiType)
    {
        foreach (var emoji in emojiImage)
        {
            emoji.gameObject.SetActive(false);
        }
        emojiImage[(int)emojiType].gameObject.SetActive(true);
        
        
        emojiBubble.transform.localScale = Vector3.zero;
        if(_emojiBubbleTween != null) _emojiBubbleTween.Kill();
        _emojiBubbleTween = emojiBubble.transform.DOScale(1, bubbleSpeed);
        DOVirtual.DelayedCall(3f, () =>
        {
            CloseEmojiBubble();
        });
    }
    public void CloseEmojiBubble()
    {
        if(_emojiBubbleTween != null) _emojiBubbleTween.Kill();
        _emojiBubbleTween = emojiBubble.transform.DOScale(0, 0.2f);
    }
    
    
    

    [ContextMenu("TestSpeechBubble")]
    public void TestSpeechBubble()
    {
        ShowSpeechBubble("大夫我要死了!测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试");
    }



    [ContextMenu("TestEmojiBubble")]
    public void TestEmojiBubble()
    {
        ShowEmojiBubble(EmojiType.Happy);
    }

}

public enum EmojiType
{
    Happy, Sad, Ill
}
