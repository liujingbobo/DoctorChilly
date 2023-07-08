using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ChillyDoctor/DiagnosisPattern")]
public class DiagnosisPattern : ScriptableObject
{
    [SerializeField, PropertyRange(0, 1f), LabelText("0")]
    private float Beat_0;

    [SerializeField, PropertyRange(0, 1f), LabelText("1")]
    private float Beat_1;

    [SerializeField, PropertyRange(0, 1f), LabelText("2")]
    private float Beat_2;

    [SerializeField, PropertyRange(0, 1f), LabelText("3")]
    private float Beat_3;

    [SerializeField, PropertyRange(0, 1f), LabelText("4")]
    private float Beat_4;

    [SerializeField, PropertyRange(0, 1f), LabelText("5")]
    private float Beat_5;

    [SerializeField, PropertyRange(0, 1f), LabelText("6")]
    private float Beat_6;

    [SerializeField, PropertyRange(0, 1f), LabelText("7")]
    private float Beat_7;

    private float GetBeat(int phase)
    {
        phase %= 8;
        return phase switch
        {
            0 => Beat_0,
            1 => Beat_1,
            2 => Beat_2,
            3 => Beat_3,
            4 => Beat_4,
            5 => Beat_5,
            6 => Beat_6,
            7 => Beat_7,
            _ => 0f
        };
    }

    public Vibration GetVibration(int phase)
    {
        return new Vibration()
        {
            Amp = GetBeat(phase)
        };
    }

    public struct Vibration
    {
        public float Amp;
    }

    [Serializable]
    public class DiagnosisGroup
    {
        public DiagnosisPattern[] Patterns;
    }
}