using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeskClickHold : MonoBehaviour
{
    public GamePlay1 gameplay1;

    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.CurrentState == GameState.State1)
        {
            gameplay1.TriggerVibration();
        }
    }

    private void OnMouseUp()
    {
        if (GameManager.Instance.CurrentState == GameState.State1)
        {
            gameplay1.StopVibration();
        }
    }
}
