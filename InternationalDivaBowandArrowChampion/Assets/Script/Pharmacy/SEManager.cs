using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    [SerializeField]
    public List<SEPack> SEPacks = new List<SEPack>();
    
    [Serializable]
    public struct SEPack
    {
        public SEType SEType;
        public StudioEventEmitter SE;
    }
    public enum SEType
    {
        OpenDrawer,
        CloseDrawer,
        PutOnPlate,
        Packing,
        Bell,
        CameraMove,
        Ding
    }
    
    public void PlaySE(SEType t)
    {
        if (SEPacks.Any(_ => _.SEType == t))
        {
            SEPacks.FirstOrDefault(_ => _.SEType == t).SE.Play();
        }
    }
}
    