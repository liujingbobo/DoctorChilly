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
                CommonUIManager.Instance.loadingPanel.gameObject.SetActive(true);
                CommonUIManager.Instance.gameState2Panel.gameObject.SetActive(false);
                SceneManager.LoadScene(0);
                break;
            case GameState.State1:
                SceneManager.LoadScene(1);
                CommonUIManager.Instance.StartState1(5);
                CommonUIManager.Instance.gameState1Panel.gameObject.SetActive(true);
                break;
            case GameState.State2:
                SceneManager.LoadScene(2);
                CommonUIManager.Instance.gameState1Panel.gameObject.SetActive(false);
                CommonUIManager.Instance.gameState2Panel.gameObject.SetActive(true);
                CommonUIManager.Instance.StartState2();
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