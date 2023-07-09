using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beginning : MonoBehaviour
{


    public void PlayMoveDown()
    {
        GameManager.Instance.SeManager.PlaySE(SEManager.SEType.MoveDown);
    }
}
