using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTypeWriterEffect : MonoBehaviour
{
    public TMP_Text text;

    private Coroutine _effectCoroutine;
    private string _fullString;
    public void StartTypeWriteEffectWithInterval(string targetString, float interval)
    {
        if (string.IsNullOrEmpty(targetString)) return;
        _fullString = targetString;
        if (_effectCoroutine != null) StopCoroutine(_effectCoroutine);
        _effectCoroutine = StartCoroutine(TypeWriterEffect(interval));
    }
    public void StartTypeWriterEffect(string targetString, float duration)
    {
        if (string.IsNullOrEmpty(targetString)) return;

        _fullString = targetString;

        var count = targetString.Length;
        var interval = duration / count;

        if (_effectCoroutine != null) StopCoroutine(_effectCoroutine);
        _effectCoroutine = StartCoroutine(TypeWriterEffect(interval));
    }

    public IEnumerator TypeWriterEffect(float interval)
    {
        var startTime = Time.time;
        text.text = string.Empty;
        while (true)
        {
            var curTime = Time.time;
            var currentIndex = Mathf.Clamp(Mathf.FloorToInt((curTime - startTime) / interval), 0,
                _fullString.Length);
            text.text = _fullString.Substring(0, currentIndex);

            if (text.text.Length == _fullString.Length)
            {
                Debug.Log((text.text.Length));
                break;
            }
            yield return new WaitForSeconds(interval);
        }
    }
}