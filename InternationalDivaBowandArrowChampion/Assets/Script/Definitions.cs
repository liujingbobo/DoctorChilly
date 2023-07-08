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
    Ginseng = 0, // 人参
    Angelica = 10, // 当归
    Gentian = 20, // 龙胆
    Coptis = 30, // 黄连
}

public enum PharmacyResult
{
    Good, // 全中
    Normal, // 对部分
    Bad // 
}