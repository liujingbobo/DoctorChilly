using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GamePlay1 : MonoBehaviour
{
    public Transform hand;
    public List<GameObject> flags;
    public Pharmacy pharmacy;
    public SpriteRenderer handRenderer;
    public VibrationPlayer vibrationPlayer;
    
    public GameConfig.HandPack pickedHandPack;
    public List<GameConfig.SymptomPack> pickedSymptomsPacks;
    public TMP_Text patientCountTxt;
    
    private int _count;

    private void Update()
    {
        patientCountTxt.text = $"待诊: {GameManager.Instance.patientCount}";
    }


    private Coroutine _countDownCoroutine;
    public void SetUpHandAndSymptoms(GameConfig.HandPack handPack, List<GameConfig.SymptomPack> symptoms)
    {
        ResetHand();
        pickedHandPack = handPack;
        pickedSymptomsPacks = symptoms;

        handRenderer.sprite = handPack.Hand;
    }

    public void TriggerVibration()
    {
        Debug.Log($"Triggering Vibration");
        vibrationPlayer.Play(pickedSymptomsPacks.Select(x=>x.diagnosisPattern).ToArray());
    }

    public void StopVibration()
    {
        Debug.Log($"Stopping Vibration");
        vibrationPlayer.Stop();
    }
    
    [Button]
    public void MoveHand()
    {
        hand.DOKill();
        hand.DOLocalMove(Vector3.zero, 2);
    }
    
    [Button]
    public void ResetHand()
    {
        hand.position = new Vector3(0, 0.6f, 0);
    }

    [Button]
    public void AddFlag()
    {
        if (_count < flags.Count)
        {
            flags[_count++].SetActive(true);
        }
    }
    
    [Button]
    public void ResetFlag()
    {
        _count = 0;
        foreach (var flag in flags)
        {
            flag.gameObject.SetActive(false);
        }
    }

    public void GoToMedicine()
    {
        if(_countDownCoroutine != null) StopCoroutine(_countDownCoroutine);
        GameManager.Instance.ChangeState(GameState.Animation);
    }
    public void StartPlay1Routine(List<Symptom> symptoms)
    {
        if(_countDownCoroutine != null) StopCoroutine(_countDownCoroutine);
        _countDownCoroutine = StartCoroutine(GamePlay1Routine());
    }

    private IEnumerator GamePlay1Routine()
    {
        GameManager.Instance.SeManager.PlaySE(pickedHandPack.SeType);
        MoveHand();
        //speech bubble
        BubbleManager.singleton.ShowSpeechBubble(
            pickedHandPack.StartDialog);
        yield return new WaitForSeconds(2);
        
        //todo：音效，手，病随机出来
        
        yield return new WaitForSeconds(20);
        GameManager.Instance.ChangeState(GameState.Animation);
    }

}
