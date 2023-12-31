﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FMODUnity;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    public StudioEventEmitter MainMusic;

    public StudioEventEmitter GamePlayMusic;
    
    public SEManager SeManager;
    public GameConfig Config;

    public GamePlay1 gamePlay1;
    public Pharmacy pharmacy;
    public Ending ending;
    public GameConfig.HandPack CurrentHandPack;

    public CreditPage creditPage;

    [SerializeField] private Animator startScreenanimator;
    [SerializeField] private CutScenePlayer cutScenePlayer;
    [SerializeField] private Tutorial tutorial;

    public Image Fade;

    private List<Symptom> CurrentSymtoms = new List<Symptom>();

    public int correct;

    public int patientRemain = 10;
    
    public int doubleSymptomAfterPatientCount = 6;

    public GameState CurrentState;

    private bool locked;

    public float GameStartTime;
    public Image heavySickIndicator;
    public Image medicHeavySickIndicator;

    public TMP_Text patientCountTxt;

    private List<GameConfig.HandPack> usedHands = new List<GameConfig.HandPack>();
    private Queue<List<Symptom>> symtoms = new Queue<List<Symptom>>();
    
    private void Update()
    {
        if (CurrentState == GameState.State1)
        {
            patientCountTxt.gameObject.SetActive((true));
            patientCountTxt.text = $"待诊: {GameManager.Instance.patientRemain}";
        }
        else
        {
            patientCountTxt.gameObject.SetActive((false));
        }
    }

    private void Start()
    {
        MainMusic.Play();
        CurrentState = GameState.StartScreen;
        // ChangeState(GameState.StartScreen);
    }

    public void GenerateConfig()
    {
        symtoms.Clear();
        var baseSymtoms = new List<Symptom>(){Symptom.Cold,Symptom.Exhausted,Symptom.Fever,Symptom.Insomnia};
        baseSymtoms.MMShuffle();
        baseSymtoms.ForEach(_ =>
        {
            symtoms.Enqueue(new List<Symptom>(){_});
        });
        while (symtoms.Count < 10)
        {
            baseSymtoms.MMShuffle();
            symtoms.Enqueue(new List<Symptom>(){baseSymtoms[0], baseSymtoms[1]});
        }
    }
    
    
    public void StartGame()
    {
        GenerateConfig();
        correct = 0;
        if (!locked)
        {
            locked = true;
            StartCoroutine(WaitTillEnd());
        }

        GameStartTime = Time.time;
        heavySickIndicator.gameObject.SetActive(false);
        medicHeavySickIndicator.gameObject.SetActive(false);
        usedHands = new List<GameConfig.HandPack>();
    }

    public void OpenCredit()
    {
        // TODO"
        creditPage.Open();
    }

    public void Exit()
    {
        // TODO:
        Application.Quit();
    }

    IEnumerator WaitTillEnd()
    {
        startScreenanimator.SetTrigger("Trigger");
        yield return new WaitForSeconds(2f);
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
        heavySickIndicator.gameObject.SetActive(false);
        medicHeavySickIndicator.gameObject.SetActive(false);
        
        switch (newState)
        {
            case GameState.Tutorial:
                break;
            case GameState.State1:
                patientRemain--;

                //select hands and select symptoms
                CurrentHandPack = patientRemain >= 8
                    ? Config.normalHandPack
                    : Config.RandomPickHandExcludeGiven(usedHands);
                
                var heavySick = patientRemain <= doubleSymptomAfterPatientCount;

                var sym = symtoms.Dequeue();
                ;
                sym.ForEach(_ => print(_.ToString()));
                
                CurrentSymtoms = sym;

                //show heavy sick indicator
                if (heavySick)
                {
                    heavySickIndicator.gameObject.SetActive(true);
                }

                gamePlay1.SetUpHandAndSymptoms(CurrentHandPack,
                    CurrentSymtoms.Select(x => Config.SymptomPacks[x]).ToList());
                pharmacy.gameObject.SetActive(false);
                gamePlay1.gameObject.SetActive(true);
                gamePlay1.StartPlay1Routine(CurrentSymtoms);
                break;
            case GameState.State2:
                //show heavy sick indicator
                if (CurrentSymtoms.Count > 1)
                {
                    medicHeavySickIndicator.gameObject.SetActive(true);
                }

                usedHands.Add(CurrentHandPack);
                gamePlay1.gameObject.SetActive(false);
                pharmacy.gameObject.SetActive(true);
                pharmacy.Init(Config.GetHerbs(CurrentSymtoms));
                break;
            case GameState.Animation:
                //toDo:Animation Logic
                cutScenePlayer.PlayAnim(CurrentSymtoms.Count > 1? 1:0,() =>
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
                }
                else if (correct >= 7)
                {
                    result = PharmacyResult.Good;
                }
                else if (correct >= 4)
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

        medicHeavySickIndicator.gameObject.SetActive(false);
        if (patientRemain == 0)
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
            Fade.DOColor(new Color(0, 0, 0, 1), time).From(new Color(0, 0, 0, 0));
            yield return new WaitForSeconds(gap);
            endCallBack?.Invoke();
            Fade.DOColor(new Color(0, 0, 0, 0), time).From(new Color(0, 0, 0, 1));
        }
    }

    public void PlayTap()
    {
        GameManager.Instance.SeManager.PlaySE(SEManager.SEType.Tap);
    }

    public void PlaySlightTap()
    {
        GameManager.Instance.SeManager.PlaySE(SEManager.SEType.SlightTap);
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