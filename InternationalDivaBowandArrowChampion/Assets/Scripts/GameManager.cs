using System;
using Util;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Loading:
                break;
            case GameState.State1:
                break;
            case GameState.State2:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

}

public enum GameState
{
    Loading,
    State1,//诊脉
    State2//抓腰
}