using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public const float PLAY_TIME_IN_MINUTE = 3f;

    public GameConfig Config;

    public GamePlay1 gamePlay1;
    public Pharmacy pharmacy;
    
    public GameConfig.HandPack CurrentHandPack;
    public GameConfig.HandPack PrevHandPack;
    private List<Symptom> CurrentSymtoms = new List<Symptom>();

    public int patientCount = 0;
    public int doubleSymptomAfterPatientCount = 5;
    private void Start()
    {
        ChangeState(GameState.State1);
    }

    public void ChangeState(GameState newState)
    {
        BubbleManager.singleton.CloseSpeechBubble();
        BubbleManager.singleton.CloseEmojiBubble();
        switch (newState)
        {
            case GameState.State1:
                patientCount++;
                
                //select hands and select symptoms
                CurrentHandPack = Config.RandomPickHandExcludeGiven(PrevHandPack);
                CurrentSymtoms = patientCount > doubleSymptomAfterPatientCount?
                    Config.RandomlyGetSymtoms(2):
                    Config.RandomlyGetSymtoms(1);
                
                gamePlay1.SetUpHandAndSymptoms(CurrentHandPack, CurrentSymtoms);
                pharmacy.gameObject.SetActive(false);
                gamePlay1.gameObject.SetActive(true);
                gamePlay1.StartPlay1Routine(CurrentSymtoms);
                break;
            case GameState.State2:
                PrevHandPack = CurrentHandPack;
                gamePlay1.gameObject.SetActive(false);
                pharmacy.gameObject.SetActive(true);
                pharmacy.Init(Config.GetHerbs(CurrentSymtoms));
                break;
            case GameState.Animation:
                //toDo:Animation Logic
                ChangeState(GameState.State2);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void EndPharmacy(bool result)
    {
        ChangeState(GameState.State1);
        // if (result)
        // {
        //     BubbleManager.singleton.ShowEmojiBubble(EmojiType.Happy);
        // }
        // else
        // {
        //     BubbleManager.singleton.ShowEmojiBubble(EmojiType.Ill);
        // }
    }
}

public enum GameState
{
    State1,
    State2,
    Animation
}