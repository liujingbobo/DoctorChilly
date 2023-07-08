using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GamePlay1 : MonoBehaviour
{
    public Transform hand;
    public List<GameObject> flags;

    private int _count;

    [Button]
    public void MoveHand()
    {
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
    
    public void StartCountdown()
    {
        StartCoroutine(GamePlay1Routine());
    }

    private IEnumerator GamePlay1Routine()
    {
        MoveHand();
        yield return new WaitForSeconds(2);

        //todo：诊断逻辑
        yield return new WaitForSeconds(20);

    }

}
