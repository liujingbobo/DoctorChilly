using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UnityEngine;

public class VibrationPlayer : MonoBehaviour
{
    public const int total_beat = 8;
    
    [SerializeField] private float beat_duration = 0.2f;
    [SerializeField] private float gap_duration = 0.05f;
    [SerializeField] private int max_loop_times = 100;
    [SerializeField] private float baseFrequency = 0.5f;

    private Coroutine _playing;

    public bool Playing => _playing != null;

    public void Play(DiagnosisPattern[] patterns)
    {
        Stop();
        _playing = StartCoroutine(Process(patterns));
    }

    public void Stop()
    {
        if (_playing != null)
        {
            StopCoroutine(_playing);
        }
    }

    private IEnumerator Process(DiagnosisPattern[] patterns)
    {
        Debug.Log($"Start Vibration");
        for (int loop = 0; loop < max_loop_times; loop++)
        {
            for (int beat = 0; beat < total_beat; beat++)
            {
                var v = MixVibrations(patterns, beat);
                if (v.Amp > 0f)
                {
                    HapticPatterns.PlayConstant(v.Amp, baseFrequency, beat_duration);
                }

                yield return new WaitForSeconds(beat_duration + gap_duration);
            }
        }

        Debug.Log($"Finish Vibration");
        _playing = null;
    }

    DiagnosisPattern.Vibration MixVibrations(DiagnosisPattern[] patterns, int phase)
    {
        var vibrations = patterns.Select(_ => _.GetVibration(phase)).ToArray();
        return new DiagnosisPattern.Vibration()
        {
            Amp = vibrations.Sum(_ => _.Amp)
        };
    }

    private void OnDisable()
    {
        if (Playing)
        {
            Debug.LogWarning("Vibration stopped automatically");
            Stop();
        }
    }
}