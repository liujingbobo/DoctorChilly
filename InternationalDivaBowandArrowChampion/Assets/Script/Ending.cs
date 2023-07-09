using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Image image;
    public Text successAmountTxt;
    
    public void Fill(PharmacyResult result, int successamount)
    {
        successAmountTxt.text = successamount.ToString();
        image.sprite = GameManager.Instance.Config.ResultDic[result];
        // Play Anim
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("LHTest");
    }
}
