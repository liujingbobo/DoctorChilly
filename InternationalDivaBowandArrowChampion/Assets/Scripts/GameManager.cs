using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private Animator startScreenanimator;
    
    private List<Symptom> CurrentSymtoms = new List<Symptom>();

    public int patientCount = 10;
    public int doubleSymptomAfterPatientCount = 5;

    private bool locked;
    private void Start()
    {
        ChangeState(GameState.StartScreen);
    }

    public void StartGame()
    {
        if (!locked)
        {
            locked = true;
            StartCoroutine(WaitTillEnd());
        }
    }

    public void OpenCredit()
    {
        // TODO"
    }

    public void Exit()
    {
        // TODO:
    }

    IEnumerator WaitTillEnd()
    {
        startScreenanimator.SetTrigger("Trigger");
        yield return new WaitForSeconds(2.1f);
        ChangeState(GameState.State1);
        locked = false;
    }

    public void ChangeState(GameState newState)
    {
        BubbleManager.singleton.CloseSpeechBubble();
        BubbleManager.singleton.CloseEmojiBubble();
        switch (newState)
        {
            case GameState.StartScreen:
                break;
            case GameState.State1:
                patientCount--;
                
                //select hands and select symptoms
                CurrentHandPack = Config.RandomPickHandExcludeGiven(PrevHandPack);
                CurrentSymtoms = patientCount > doubleSymptomAfterPatientCount?
                    Config.RandomlyGetSymtoms(2):
                    Config.RandomlyGetSymtoms(1);
                
                gamePlay1.SetUpHandAndSymptoms(CurrentHandPack, CurrentSymtoms.Select(x=>Config.SymptomPacks[x]).ToList());
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
            case GameState.Settlement:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void EndPharmacy(bool result)
    {
        if (result)
        {
            gamePlay1.AddFlag();
        }
        
        if (patientCount == 0)
        {
            ChangeState(GameState.Settlement);
        }
        else
        {
            ChangeState(GameState.State1);
        }
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
    StartScreen,
    State1,
    State2,
    Animation,
    Settlement
}