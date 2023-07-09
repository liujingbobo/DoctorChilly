using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Image image;
    public Text successAmountTxt;
    public Text timeText;
    
    public void Fill(PharmacyResult result, int successamount)
    {
        successAmountTxt.text = successamount.ToString();
        var t = TimeSpan.FromSeconds((Time.time - GameManager.Instance.GameStartTime));
        string formattedTime = string.Format("{0}:{1:00}", (int)t.TotalMinutes, t.Seconds);
        timeText.text = formattedTime;
        image.sprite = GameManager.Instance.Config.ResultDic[result];
        // Play Anim
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("LHTest");
    }
}
