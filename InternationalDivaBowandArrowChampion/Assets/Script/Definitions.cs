using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 疾病/症状
public enum Symptom
{
    Cold = 0,
    Fever = 10,
    Exhausted = 20,
    Insomnia = 30 // 和一名老师学的
}


// 药草
public enum Herb
{
    Red = 0, // 人参
    Yellow = 10, // 当归
    Blue = 20, // 龙胆
    Green = 30, // 黄连
}

public enum PharmacyResult
{
    Best, 
    Good, 
    Normal, 
    Bad 
}