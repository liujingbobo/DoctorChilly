using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using CanvasGroup = UnityEngine.CanvasGroup;

public class BubbleManager : MonoBehaviour
{
    public static BubbleManager singleton;

    public GameObject speechBubble;
    public TextTypeWriterEffect bubbleWriterEffect;

    public GameObject endSpeechBubble;
    public TextTypeWriterEffect endSpeechBubbleWriterEffect;
    
    public GameObject emojiBubble;
    public Image[] emojiImage;
    public Image _bubbleImage;
    public Image _bubbleCornerImage;
    
    public Color successColor;
    public Color failColor;

    public float bubbleSpeed = 0.5f;
    public float wordGap = 0.01f;
    
    public void Awake()
    {
        singleton = this;
        speechBubble.transform.localScale = Vector3.zero;
        endSpeechBubble.transform.localScale = Vector3.zero;
        emojiBubble.transform.localScale = Vector3.zero;
    }

    private Tween _speechBubbleTween;
    private Tween _endSpeechBubbleTween;
    private Tween _emojiBubbleTween;
    
    private Tween _delayCloseTween1;
    private Tween _delayCloseTween2;
    private Tween _delayCloseTween3;
    public void ShowSpeechBubble(string targetString)
    {
        if (string.IsNullOrEmpty(targetString)) return;
        speechBubble.transform.localScale = Vector3.zero;
        if(_speechBubbleTween != null) _speechBubbleTween.Kill();
        if (speechBubble.GetComponentInChildren<CanvasGroup>() is { } cg)
        {
            cg.alpha = 1;
        }
        _speechBubbleTween = speechBubble.transform.DOScale(1, bubbleSpeed);
        bubbleWriterEffect.StartTypeWriteEffectWithInterval(targetString, wordGap);
        _delayCloseTween1 = DOVirtual.DelayedCall(1.5f, () =>
        {
            CloseSpeechBubble();
        });
    }

    public void CloseSpeechBubble()
    {
        if(_delayCloseTween1 != null) _delayCloseTween1.Kill();
        if(_speechBubbleTween != null) _speechBubbleTween.Kill();
        if (speechBubble.GetComponentInChildren<CanvasGroup>() is { } cg)
        {
            _speechBubbleTween = cg.DOFade(0, 0.1f);
        }
        // _speechBubbleTween = speechBubble.transform.DOScale(0, 0.1f);
    }

    public void ShowEndSpeechBubble(string targetString)
    {
        if (string.IsNullOrEmpty(targetString)) return;
        endSpeechBubble.transform.localScale = Vector3.zero;
        if(_endSpeechBubbleTween != null) _endSpeechBubbleTween.Kill();
        if (endSpeechBubble.GetComponentInChildren<CanvasGroup>() is { } cg)
        {
            cg.alpha = 1;
        }

        _endSpeechBubbleTween = endSpeechBubble.transform.DOScale(1, bubbleSpeed);
        endSpeechBubbleWriterEffect.StartTypeWriteEffectWithInterval(targetString, wordGap);
        _delayCloseTween2 = DOVirtual.DelayedCall(1.5f, () =>
        {
            CloseEndSpeechBubble();
        });
    }

    public void CloseEndSpeechBubble()
    {
        if(_delayCloseTween2 != null) _delayCloseTween2.Kill();
        if(_endSpeechBubbleTween != null) _endSpeechBubbleTween.Kill();
        // _endSpeechBubbleTween = endSpeechBubble.transform.DOScale(0, 0.1f);
        if (endSpeechBubble.GetComponentInChildren<CanvasGroup>() is { } cg)
        {
            _endSpeechBubbleTween = cg.DOFade(0, 0.1f);
        }
        // _endSpeechBubbleTween = endSpeechBubble.transform.DOScale(0, 0.1f);
    }
    
    public void ShowEmojiBubble(EmojiType emojiType, bool success, float time = 3f)
    {
        if (success)
        {
            _bubbleImage.color = successColor;
            _bubbleCornerImage.color = successColor;
        }
        else
        {
            _bubbleImage.color = failColor;
            _bubbleCornerImage.color = failColor;
        }
    
        foreach (var emoji in emojiImage)
        {
            emoji.gameObject.SetActive(false);
        }
        emojiImage[(int)emojiType].gameObject.SetActive(true);
        
        
        emojiBubble.transform.localScale = Vector3.zero;
        if(_emojiBubbleTween != null) _emojiBubbleTween.Kill();
        _emojiBubbleTween = emojiBubble.transform.DOScale(1, bubbleSpeed);
        
        _delayCloseTween3 = DOVirtual.DelayedCall(time, () =>
        {
            CloseEmojiBubble();
        });
    }
    public void CloseEmojiBubble()
    {
        if(_delayCloseTween3 != null) _delayCloseTween3.Kill();
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
        ShowEmojiBubble(EmojiType.Happy, true);
    }

}

public enum EmojiType
{
    Happy, Sad, Ill
}
