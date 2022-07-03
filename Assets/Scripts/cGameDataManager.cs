using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class cGameDataManager : MonoBehaviour
{
    public static cGameDataManager instance;

    [SerializeField]
    string PathData;

    MusicNodeData StageNodeData;

    [SerializeField]
    AudioClip StageMusicClip;
    [SerializeField]
    GameObject test;
    // Start is called before the first frame update
    CamEffectInfo CamEffectNode;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetMusic(AudioClip _clip, CamEffectInfo _Camnodes, int _Difficult)
    {
        CamEffectNode = _Camnodes;
        StageMusicClip = _clip;
        string Difficult = "_Normal";
        switch(_Difficult)
        {
            case 0:
                Difficult = "_Easy";
                break;
            case 1:
                Difficult = "_Normal";
                break;
            case 2:
                Difficult = "_Hard";
                break;
        }
        PathData = Application.streamingAssetsPath + "/NodeBuild/" + StageMusicClip.name + 
            "/" + StageMusicClip.name+".mp3"+Difficult + ".json";

       if (File.Exists(PathData))
       {
           print("Load Done");
           string FromJsonData = File.ReadAllText(PathData);
           StageNodeData = JsonUtility.FromJson<MusicNodeData>(FromJsonData);
       }
    }

    public MusicNodeData GetMusicnodeData()
    {
        return StageNodeData;
    }
    public CamEffectInfo GetCamEffectNodeData()
    {
        return CamEffectNode;
    }
    public AudioClip GetStageMusicClip()
    {
        return StageMusicClip;
    }
}
