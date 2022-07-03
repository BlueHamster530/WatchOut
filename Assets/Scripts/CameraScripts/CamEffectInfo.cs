using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraEffectNode
{
    public float EnableTime;
    public bool IsRepeat;
    public float DisableTimeOnRepeat;
    public int _Type;
    public float StartOrtSize;
    public float TargetOrtSize;
    public float TargetOrtSizeTime;
    public float ReturnOrtSizeTime;
}
[CreateAssetMenu]
public class CamEffectInfo : ScriptableObject
{
    [SerializeField]
    public CameraEffectNode[] CamOrhNodes;
}
