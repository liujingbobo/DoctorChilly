using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
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
    public SEManager SeManager;
    public GameConfig Config;

    public GamePlay1 gamePlay1;
    public Pharmacy pharmacy;
    public Ending ending;
    public GameConfig.HandPack CurrentHandPack;
    public GameConfig.HandPack PrevHandPack;

    [SerializeField] private Animator startScreenanimator;
    [SerializeField] private CutScenePlayer cutScenePlayer;
    [SerializeField] private Tutorial tutorial;

    public Image Fade;
    
    private List<Symptom> CurrentSymtoms = new List<Symptom>();

    public int correct;

    public int patientCount = 10;
    public int doubleSymptomAfterPatientCount = 5;

    public GameState CurrentState;
    
    private bool locked;
    private void Start()
    {
        CurrentState = GameState.StartScreen;
        // ChangeState(GameState.StartScreen);
    }

    public void StartGame()
    {
        correct = 0;
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
        tutorial.StartTutorial();
    }

    public void UnLocked()
    {
        locked = false;
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        BubbleManager.singleton.CloseSpeechBubble();
        BubbleManager.singleton.CloseEndSpeechBubble();
        BubbleManager.singleton.CloseEmojiBubble();
        switch (newState)
        {
            case GameState.Tutorial:
                break;
            case GameState.State1:
                patientCount--;
                
                //select hands and select symptoms
                CurrentHandPack = Config.RandomPickHandExcludeGiven(PrevHandPack);
                CurrentSymtoms = patientCount < doubleSymptomAfterPatientCount?
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
                cutScenePlayer.PlayAnim(() =>
                {
                    StartCoroutine(PlayFadeInOut(endCallBack: () =>
                    {
                        SeManager.PlaySE(SEManager.SEType.CameraMove);
                        ChangeState(GameState.State2);
                    }));
                });
                break;
            case GameState.Settlement:
                ending.gameObject.SetActive(true);
                PharmacyResult result = PharmacyResult.Bad;
                if (correct == 10)
                {
                    result = PharmacyResult.Best;
                }else if (correct >= 7)
                {
                    result = PharmacyResult.Good;
                }else if (correct >= 4)
                {
                    result = PharmacyResult.Normal;
                }
                ending.Fill(result, correct);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void EndPharmacy(bool result)
    {
        if (result)
        {
            correct++;
            gamePlay1.AddFlag();
        }
        
        if (patientCount == 0)
        {
            ChangeState(GameState.Settlement);
        }
        else
        {
            StartCoroutine(PlayFadeInOut(endCallBack: () =>
            {
                SeManager.PlaySE(SEManager.SEType.CameraMove);
                ChangeState(GameState.State1);
            }));

        }
    }

    public IEnumerator PlayFadeInOut(float time = 0.1f, float gap = 0.5f, Action endCallBack = null)
    {
        if (Fade)
        {
            Fade.DOColor(new Color(0,0,0, 1), time).From(new Color(0,0,0,  0));
            yield return new WaitForSeconds(gap);
            endCallBack?.Invoke();
            Fade.DOColor(new Color(0,0,0,  0), time).From(new Color(0,0,0,  1));
        }
    }
}

public enum GameState
{
    StartScreen,
    Tutorial,
    State1,
    State2,
    Animation,
    Settlement
}