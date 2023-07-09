using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditPage : MonoBehaviour
{
    public Image creditImg;
    public Button closeButton;
    
    void Start(){
        
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(Close);
    }
    
    public void Open(){
        creditImg.gameObject.SetActive(true);
    }
    
    public void Close(){
        creditImg.gameObject.SetActive(false);
    }
}
