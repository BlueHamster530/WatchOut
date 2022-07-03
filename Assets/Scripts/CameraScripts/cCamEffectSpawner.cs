using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CamZoomEffectNode
{
    public CamZoomEffectNode(float _time, float Start, float end, float targettime, float returntime)
    {
        time = _time;
        StartOrtSize = Start;
        TargetOrtSize = end;
        TargetOrtSizeTime = targettime;
        ReturnOrtSizeTime = returntime;
        IsSpawned = false;
    }
    public float time;
    public float StartOrtSize;
    public float TargetOrtSize;
    public float TargetOrtSizeTime;
    public float ReturnOrtSizeTime;
    public bool IsSpawned;
}
public class cCamEffectSpawner : MonoBehaviour
{

    [SerializeField]
    bool IsSandBox= false;
    [SerializeField]
    CamEffectInfo CamEffectNode;

    [SerializeField]
    CameraController CamController;

    CamZoomEffectNode[] ZoomEffects;
    // Start is called before the first frame update
    void Start()
    {
        if(IsSandBox == false)
             CamEffectNode = cGameDataManager.instance.GetCamEffectNodeData();//일반시사용


        CamEffectNodeSetUp();
    }
    private void CamEffectNodeSetUp()
    {
        int _EffectsNodeIndex = 0;
        #region EffectNodeLenthSetUp
        for (int i = 0; i < CamEffectNode.CamOrhNodes.Length; i++)
        {
            if (CamEffectNode.CamOrhNodes[i]._Type == 0)//줌효과일때
            {
                if (CamEffectNode.CamOrhNodes[i].IsRepeat == true)
                {
                    int _MaxIndex = Mathf.RoundToInt(
                        (CamEffectNode.CamOrhNodes[i].DisableTimeOnRepeat - CamEffectNode.CamOrhNodes[i].EnableTime)
                        * (1.0f / CamEffectNode.CamOrhNodes[i].TargetOrtSizeTime));

                    _EffectsNodeIndex += _MaxIndex;
                }
                else
                {
                    _EffectsNodeIndex++;
                }
            }
        }
        #endregion

        ZoomEffects = new CamZoomEffectNode[_EffectsNodeIndex];

        _EffectsNodeIndex = 0;
        for (int i = 0; i < CamEffectNode.CamOrhNodes.Length; i++)
        {
            if (CamEffectNode.CamOrhNodes[i]._Type == 0)//줌효과일때
            {
                if (CamEffectNode.CamOrhNodes[i].IsRepeat == true)
                {
                    int _MaxIndex = Mathf.RoundToInt(
                        (CamEffectNode.CamOrhNodes[i].DisableTimeOnRepeat - CamEffectNode.CamOrhNodes[i].EnableTime)
                        * (1.0f / CamEffectNode.CamOrhNodes[i].TargetOrtSizeTime));
                    for (int ii = 0; ii < _MaxIndex; ii++)
                    {
                        CamZoomEffectNode clone = new CamZoomEffectNode(CamEffectNode.CamOrhNodes[i].EnableTime + (ii * CamEffectNode.CamOrhNodes[i].TargetOrtSizeTime),
                        CamEffectNode.CamOrhNodes[i].StartOrtSize, CamEffectNode.CamOrhNodes[i].TargetOrtSize,
                        CamEffectNode.CamOrhNodes[i].TargetOrtSizeTime, CamEffectNode.CamOrhNodes[i].ReturnOrtSizeTime);
                        ZoomEffects[_EffectsNodeIndex] = clone;
                        _EffectsNodeIndex++;
                    }

                }
                else
                {
                    CamZoomEffectNode clone = new CamZoomEffectNode(CamEffectNode.CamOrhNodes[i].EnableTime + (CamEffectNode.CamOrhNodes[i].TargetOrtSizeTime),
                    CamEffectNode.CamOrhNodes[i].StartOrtSize, CamEffectNode.CamOrhNodes[i].TargetOrtSize,
                    CamEffectNode.CamOrhNodes[i].TargetOrtSizeTime, CamEffectNode.CamOrhNodes[i].ReturnOrtSizeTime);
                    ZoomEffects[_EffectsNodeIndex] = clone;
                    _EffectsNodeIndex++;
                }
            }
        }
    }
    private void CamEffectSpawn()
    {
        for (int i = 0; i < ZoomEffects.Length; i++)
        {
            if (cGameManager.instance.CurrnetPlayTime >= ZoomEffects[i].time&& ZoomEffects[i].IsSpawned == false)
            {
                CamController.CloseUpForTime(ZoomEffects[i].StartOrtSize, ZoomEffects[i].TargetOrtSize, ZoomEffects[i].TargetOrtSizeTime, ZoomEffects[i].ReturnOrtSizeTime);
                ZoomEffects[i].IsSpawned = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CamEffectSpawn();
    }
}
