using System.Collections;
using TMPro;
using UnityEngine;
using Util;

public class CommonUIManager : Singleton<CommonUIManager>
{
    public GameObject loadingPanel;
    public GameObject gameState1Panel;
    public GameObject gameState2Panel;

    public TMP_Text countdown;
    public TMP_Text state1InfoText;
    
    public TMP_Text state2InfoText;
    public GameObject settlement;


    #region LoadingScene

    public void StartGame()
    {
        loadingPanel.gameObject.SetActive(false);
        GameManager.Instance.ChangeState(GameState.State1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region State1诊脉阶段

    public void StartState1(int countdownTime)
    {
        StartCoroutine(State1Routine(countdownTime));
    }

    private IEnumerator State1Routine(int countdownTime)
    {
        //todo:过场动画效果
        yield return new WaitForSeconds(3);
        
        state1InfoText.text = "诊脉阶段";
        countdown.gameObject.SetActive(true);

        while (countdownTime > 0)
        {
            countdown.text = countdownTime.ToString() + "s"; 
            yield return new WaitForSeconds(1);
            countdownTime--; 
        }

        countdown.text = "0s";
        GameManager.Instance.ChangeState(GameState.State2);
    }

    #endregion

    #region State2抓药阶段

    public void StartState2()
    {
        StartCoroutine(State2Routine());
    }

    private IEnumerator State2Routine()
    {
        //todo:抓药阶段
        yield return new WaitForSeconds(3);
        state2InfoText.gameObject.SetActive(false);
        settlement.SetActive(true);
    }

    public void BackToLoadingScene()
    {
        GameManager.Instance.ChangeState(GameState.Loading);
    }

    #endregion

}
